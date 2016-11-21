using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputCombat : MonoBehaviour, ICombatant {

	public CombatActionSelectorController actionSelector;

	private bool hasTurn;
	private bool selectingAction;

	private CombatManager combatManager;
	private PlayerMotionCombat pmc;
	private CharacterSheet characterSheet;

	public delegate void HasTurnChanged (bool hasTurn);
	public HasTurnChanged hasTurnChanged;

	void Awake () {
		characterSheet = GetComponent<CharacterSheet> ();
		characterSheet.FighterCharacterSheet ();
		characterSheet.combatStatusChangedDelegate += CombatStatusChanged;
	}

	void Start () {
		hasTurn = false;
		pmc = GetComponent<PlayerMotionCombat> ();
		combatManager = FindObjectOfType (typeof(CombatManager)) as CombatManager;
	}

	// Update is called once per frame
	void Update () {

		if (pmc.isMoving || pmc.isAttacking || !hasTurn || characterSheet.GetCombatState () != Helpers.CombatState.Alive || selectingAction) {
			return;
		}

		if (characterSheet.MovesLeft == 0 && characterSheet.AttacksLeft == 0) {
			EndTurn ();
			return;
		}

		if (Input.GetButtonDown ("Fire1")) {
			OpenActionSelector ();
			return;
		}

		int horizontal = (int)(Input.GetAxisRaw ("Horizontal"));
		int vertical = horizontal != 0 ? 0 : (int)(Input.GetAxisRaw ("Vertical"));
		if (horizontal != 0 || vertical != 0) {
			Move (horizontal, vertical);
		}
			
	}

	public void ForceEndTurn () {
		characterSheet.MovesLeft = 0;
		characterSheet.AttacksLeft = 0;
	}

	private void EndTurn () {
		hasTurn = false;
		hasTurnChanged (false);
		combatManager.EndTurn ();
	}

	public void Attack (Vector3 target) {
		pmc.TurnTo (target);
		pmc.StartAttackAnimation ();

		CharacterSheet enemy = combatManager.GetEnemyAtGridPosition (target);
		if (enemy != null) {
			combatManager.MeleeAttack (characterSheet, enemy);
			characterSheet.AttacksLeft--;
		}
	}

	public void GoFullDefense () {
		characterSheet.AddCombatEffect (new FullDefense ());
		ForceEndTurn ();
	}

	private void Move (int horizontal, int vertical) {
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
			return;
		}

		if (characterSheet.MovesLeft == 0 && characterSheet.AttacksLeft == 0) {
			return;
		} else if (characterSheet.MovesLeft == 0) {
			characterSheet.MovesLeft += (characterSheet.Speed / characterSheet.NumAttacks);
			characterSheet.AttacksLeft--;
		}

		characterSheet.MovesLeft--;
		combatManager.Players [characterSheet] = target;
		StartCoroutine (pmc.MoveTo (target));
	}
		
	private void CombatStatusChanged () {
		if (characterSheet.GetCombatState () != Helpers.CombatState.Alive) {
			gameObject.SetActive (false);
		}
	}

	private Vector3 GridPosition () {
		return combatManager.Players [characterSheet];
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
		actionSelector.DisplaySelector ();
	}

	public bool CanAttack () {
		if (characterSheet.AttacksLeft > 0) {
			return true;
		}
		return false;
	}

	public bool CanFullDefense () {
		if (characterSheet.AttacksLeft == characterSheet.NumAttacks) {
			return true;
		}
		return false;
	}

	public bool CanCastSpell () {
		return false;
	}

	public bool CanUseItem () {
		return false;
	}

	public void SelectorClosed () {
		selectingAction = false;
	}


	/*
	 * ICombatant interface implementation
	 */
	public string GetCombatTag () {
		return "Player1";
	}

	public void GiveTurn () {
		characterSheet.UpdateCombatEffects ();

		if (characterSheet.GetCombatState () != Helpers.CombatState.Alive) {
			combatManager.EndTurn ();
		}

		characterSheet.MovesLeft = characterSheet.Speed; 
		characterSheet.AttacksLeft = characterSheet.NumAttacks;
		hasTurn = true;
		hasTurnChanged (hasTurn);

	}

}
