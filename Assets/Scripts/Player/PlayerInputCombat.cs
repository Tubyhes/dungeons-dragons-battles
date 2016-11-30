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
	private CharacterSheet characterSheet;

	private SoundManager soundManager;

	void Awake () {
		characterSheet = GetComponent<CharacterSheet> ();
		characterSheet.InitCharacterSheet ();
		characterSheet.combatStatusChangedDelegate += CombatStatusChanged;

		soundManager = FindObjectOfType (typeof(SoundManager)) as SoundManager;
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

		if (characterSheet.MovesLeft == 0 && characterSheet.AttacksLeft == 0) {
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
		characterSheet.MovesLeft = 0;
		characterSheet.AttacksLeft = 0;
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
		combatManager.combatLogController.Log (characterSheet.Name + " attacks " + c.Name + ".");

		pmc.TurnTo (target);
		pmc.StartAttackAnimation ();
		soundManager.PlayAttack ();

		yield return new WaitForSeconds (0.5f);

		Vector3 dir = target - transform.position;
		Vector3 flanker = target + dir;
		bool isFlanking = combatManager.GetCombatantOfTeamAtGridPosition (flanker, team) != null;

		characterSheet.MeleeAttack (c, isFlanking);
		characterSheet.AttacksLeft--;

		combatManager.combatLogController.Log ("\n");
		isBusy = false;
	}

	public IEnumerator CastSpell (Spell spell, Vector3 target) {
		isBusy = true;

		CharacterSheet c = combatManager.GetCombatantAtGridPosition (target);
		combatManager.combatLogController.Log (characterSheet.Name + " casts " + spell.spellName + " on " + c.Name + ".");

		pmc.TurnTo (target);
		pmc.StartAttackAnimation ();

		yield return spell.PlayAnimation (transform.position, target);

		// TODO: find all enemies within the area of effect of the spell
		characterSheet.CastSpell (c, spell);
		characterSheet.AttacksLeft = 0;

		combatManager.combatLogController.Log ("\n");
		isBusy = false;
	}

	public void GoFullDefense () {
		characterSheet.GoFullDefense ();
		combatManager.combatLogController.Log (characterSheet.Name + " goes Full Defense (AC +4).\n");
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

		if (characterSheet.MovesLeft == 0) {
			characterSheet.MovesLeft += (characterSheet.Speed / characterSheet.NumAttacks);
			characterSheet.AttacksLeft--;
		}

		isBusy = true;

		characterSheet.MovesLeft--;
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

		characterSheet.MovesLeft = characterSheet.Speed; 
		characterSheet.AttacksLeft = characterSheet.NumAttacks;
		hasTurn = true;
		hasTurnChanged (hasTurn);
		tileHighlight.SetActive (true);

	}

}
