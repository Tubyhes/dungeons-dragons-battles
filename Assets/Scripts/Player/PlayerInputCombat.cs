using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputCombat : Combatant {

	public CombatActionSelectorController actionSelector;
	public GameObject tileHighlight;

	private bool hasTurn;
	private bool isBusy;
	private bool selectingAction;

	private CombatManager combatManager;
	private PlayerMotionCombat pmc;

	private SoundManager soundManager;

	void Awake () {
		soundManager = FindObjectOfType (typeof(SoundManager)) as SoundManager;
	}

	public override void SetCharacterSheet (CharacterSheet sheet) {
		characterSheet = sheet;
		characterSheet.combatStatusChangedDelegate += CombatStatusChanged;
		actionSelector.sheet = sheet;
	}

	void Start () {
		hasTurn = false;
		isBusy = false;

		pmc = GetComponent<PlayerMotionCombat> ();
		combatManager = FindObjectOfType (typeof(CombatManager)) as CombatManager;
		if (team == Helpers.Teams.Away) {
			pmc.TurnLeft ();
		}
	}

	// Update is called once per frame
	void Update () {

		if (isBusy || 
			!hasTurn || 
			characterSheet.GetCombatState () != Helpers.CombatState.Alive || 
			selectingAction || 
			combatManager.combatEnded) {

			return;
		}

		if (characterSheet.moves_left == 0 && characterSheet.attacks_left == 0) {
			EndTurn ();
			return;
		}

		if (Input.GetButtonDown (Helpers.Fire1(team))) {
			OpenActionSelector ();
			return;
		}

		int horizontal = (int)(Input.GetAxisRaw (Helpers.Horizontal(team)));
		int vertical = horizontal != 0 ? 0 : (int)(Input.GetAxisRaw (Helpers.Vertical(team)));
		if (horizontal != 0 || vertical != 0) {
			StartCoroutine (Move (horizontal, vertical));
		}
			
	}

	public void ForceEndTurn () {
		characterSheet.moves_left = 0;
		characterSheet.attacks_left = 0;
	}

	private void EndTurn () {
		hasTurn = false;
		hasTurnChanged (false);
		tileHighlight.SetActive (false);
		combatManager.EndTurn ();
	}

	public IEnumerator Attack (Vector3 target) {
		isBusy = true;

		CharacterSheet c = combatManager.GetCombatantAtGridPosition (target);
		combatManager.combatLogController.Log (characterSheet.character_name + " attacks " + c.character_name + ".");

		pmc.TurnTo (target);
		pmc.StartAttackAnimation ();
		soundManager.PlayAttack ();

		yield return new WaitForSeconds (0.5f);

		Vector3 dir = target - transform.position;
		Vector3 flanker = target + dir;
		bool isFlanking = combatManager.GetCombatantOfTeamAtGridPosition (flanker, team) != null;

		characterSheet.MeleeAttack (c, isFlanking);
		characterSheet.attacks_left--;

		combatManager.combatLogController.Log ("\n");
		isBusy = false;
	}

	public IEnumerator CastSpell (Spell spell, Vector3 target) {
		isBusy = true;

		pmc.TurnTo (target);
		pmc.StartAttackAnimation ();

		bool inMelee = combatManager.IsCombatantInMelee (transform.position, team);

		if (inMelee && !characterSheet.ConcentrationCheck (spell)) {
			// spell fizzles
			combatManager.combatLogController.Log (characterSheet.character_name + " tries to cast " + spell.spellName + ", but it FIZZLES!");
			yield return new WaitForSeconds(0.5f);
			characterSheet.spells_left--;
		} else {
			CharacterSheet c = combatManager.GetCombatantAtGridPosition (target);
			combatManager.combatLogController.Log (characterSheet.character_name + " casts " + spell.spellName + " on " + c.character_name + ".");
			yield return spell.PlayAnimation (transform.position, target);

			// TODO: find all enemies within the area of effect of the spell
			characterSheet.CastSpell (c, spell);
		}

		characterSheet.attacks_left = 0;
		combatManager.combatLogController.Log ("\n");
		isBusy = false;
	}

	public void GoFullDefense () {
		characterSheet.GoFullDefense ();
		combatManager.combatLogController.Log (characterSheet.character_name + " goes Full Defense (AC +4).\n");
		ForceEndTurn ();
	}

	private IEnumerator Move (int horizontal, int vertical) {
		if (horizontal < 0) {
			pmc.TurnLeft ();
		} else if (horizontal > 0) {
			pmc.TurnRight ();
		} else if (vertical < 0) {
			pmc.TurnDown ();
		} else {
			pmc.TurnUp ();
		}

		Vector3 target = FacingGridPosition ();
		if (!combatManager.IsGridPositionFree (target)) {
			return false;
		}

		if (characterSheet.moves_left == 0) {
			characterSheet.moves_left += (characterSheet.speed / characterSheet.num_attacks);
			characterSheet.attacks_left--;
		}

		isBusy = true;

		if (combatManager.IsCombatantInMelee (transform.position, team)) {
			characterSheet.moves_left = 0;
		} else {
			characterSheet.moves_left--;
		}
		combatManager.teams [team] [characterSheet] = target;
		soundManager.PlayWalk ();
		yield return StartCoroutine (pmc.MoveTo (target));

		isBusy = false;
	}
		
	private void CombatStatusChanged () {
		if (characterSheet.GetCombatState () != Helpers.CombatState.Alive) {
			pmc.Die ();
			soundManager.PlayCharacterDeath ();
		} else if (characterSheet.GetCombatState () == Helpers.CombatState.Alive) {
			pmc.Live ();
		}
	}

	private Vector3 GridPosition () {
		return combatManager.teams [team] [characterSheet];
	}

	private Vector3 FacingGridPosition () {
		Vector3 pos = GridPosition ();
		Helpers.Direction dir = pmc.FacingDirection ();

		if (dir == Helpers.Direction.Up) {
			pos.y += 1f;
		} else if (dir == Helpers.Direction.Down) {
			pos.y -= 1f;
		} else if (dir == Helpers.Direction.Left) {
			pos.x -= 1f;
		} else if (dir == Helpers.Direction.Right) {
			pos.x += 1f;
		}

		return pos;
	}

	/**
	 * Action Selector methods
	 */
	private void OpenActionSelector () {
		selectingAction = true;
		tileHighlight.SetActive (false);
		actionSelector.DisplaySelector ();
	}

	public void SelectorClosed () {
		selectingAction = false;
		tileHighlight.SetActive (true);
	}


	/*
	 * Combatant Abstract class overrides
	 */
	public override string GetCombatTag () {
		return "Player1";
	}

	public override void GiveTurn () {
		characterSheet.UpdateCombatEffects ();

		if (characterSheet.GetCombatState () != Helpers.CombatState.Alive) {
			combatManager.EndTurn ();
			return;
		}

		characterSheet.moves_left = characterSheet.speed; 
		characterSheet.attacks_left = characterSheet.num_attacks;
		hasTurn = true;
		hasTurnChanged (hasTurn);
		tileHighlight.SetActive (true);

	}

}
