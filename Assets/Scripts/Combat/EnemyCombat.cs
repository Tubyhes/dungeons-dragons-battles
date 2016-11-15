using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyCombat : Enemy, ICombatant {

	public float moveSpeed = 5f;
	public Sprite portrait;

	private bool isMoving;
	private bool hasTurn;
	private bool isAttacking;
	private bool hasTarget;

	private Vector3 target;

	private CombatManager combatManager;
	private PortraitController portraitController;

	private int movesLeft;
	private int MovesLeft {
		get {
			return movesLeft;
		}
		set {
			movesLeft = value;
			portraitController.SetMovesLeft (movesLeft, maxMoves);
		}
	}
	private int attacksLeft;
	private int AttacksLeft {
		get {
			return attacksLeft;
		}
		set {
			attacksLeft = value;
			portraitController.SetAttacksLeft (attacksLeft, maxAttacks);
		}
	}
	private int hpLeft;
	private int HpLeft {
		get {
			return hpLeft;
		}
		set {
			hpLeft = value;
			portraitController.SetHitpointsLeft (hpLeft, maxHitPoints);
		}
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		hasTurn = false;
		isMoving = false;
		isAttacking = false;
		hasTarget = false;

		// register self as combatant in this combat
		combatManager = FindObjectOfType (typeof(CombatManager)) as CombatManager;
		combatManager.RegisterEnemy (this, transform.position, out portraitController);

		// populate the portrait
		portraitController.Enable();
		portraitController.SetImage (portrait);
		MovesLeft = 0;
		AttacksLeft = 0;
		HpLeft = maxHitPoints;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasTurn || isMoving || isAttacking) {
			return;
		}

		if (MovesLeft == 0 && AttacksLeft == 0) {
			EndTurn ();
			return;
		}

		if (!hasTarget) {
			// if we dont have a target yet, try to find one
			if (!FindTarget ()) {
				// if we dont succeed in finding a target, end turn
				EndTurn ();
				return;
			}
		}

		DoTurn ();
	}

	private IEnumerator SmoothMovement (Vector3 target) 
	{
		while ((transform.position - target).sqrMagnitude > 0.00001f) {
			Vector3 newPosition = Vector3.MoveTowards (rb2d.position, target, Time.deltaTime * moveSpeed);
			rb2d.MovePosition (newPosition);
			yield return null;
		}

		isMoving = false;
	}

	private Vector3 GridPosition () {
		return combatManager.Enemies [this];
	}

	private Vector3 NextMove () {
		Vector3 here = GridPosition ();
		float diff_x = target.x - here.x;
		float diff_y = target.y - here.y;

		Vector3 verticalOption = GridPosition ();
		if (diff_y < 0f) {
			verticalOption.y -= 1f;
		} else if (diff_y > 0f) {
			verticalOption.y += 1f;
		}

		Vector3 horizontalOption = GridPosition ();
		if (diff_x < 0f) {
			horizontalOption.x -= 1f;
		} else if (diff_x > 0f) {
			horizontalOption.x += 1f;
		}

		if (Mathf.Abs (diff_x) > Mathf.Abs (diff_y)) {
			if (verticalOption.y != here.y) {
				if (combatManager.IsGridPositionFree (verticalOption)) {
					return verticalOption;
				}
			}
			if (horizontalOption.x != here.x) {
				if (combatManager.IsGridPositionFree (horizontalOption)) {
					return horizontalOption;
				}
			}
		} else {
			if (horizontalOption.x != here.x) {
				if (combatManager.IsGridPositionFree (horizontalOption)) {
					return horizontalOption;
				}
			}
			if (verticalOption.y != here.y) {
				if (combatManager.IsGridPositionFree (verticalOption)) {
					return verticalOption;
				}
			}
		}

		return here;
	}

	private bool FindTarget () {
		float minDistance = float.MaxValue;
		Vector3 here = GridPosition ();

		foreach (KeyValuePair<ICombatant, Vector3> player in combatManager.Players) {
			float distance = Helpers.EditingDistance (here, player.Value);
			if (distance < minDistance && player.Key.GetCurrentState() == Helpers.CombatState.Alive) {
				target = player.Value;
				minDistance = distance;
				hasTarget = true;
			}
		}

		return hasTarget;
	}

	private void DoTurn () {
		if (Helpers.EditingDistance(target, GridPosition()) == 1) {
			// we are adjacent to the target location, time to attack!
			if (AttacksLeft > 0) {
				// if we still have attacks left, then attack!
				ICombatant player = combatManager.GetPlayerAtGridPosition (target);
				combatManager.MeleeAttack (this, player);
				AttacksLeft--;
				return;
			}
			if (AttacksLeft == 0) {
				// if we dont have attacks left, end the turn. no post combat movement for now
				EndTurn ();
				return;
			}
		} else {
			// we are not adjacent to the target location yet, so lets move
			Vector3 nextMove = NextMove();

			// if our strategy does not provide a valid next move, end turn
			if (nextMove == GridPosition()) {
				EndTurn ();
				return;
			}

			// if it does, then we move there
			if (MovesLeft == 0) {
				MovesLeft += (maxMoves / maxAttacks);
				AttacksLeft--;
			}
			MovesLeft--;
			isMoving = true;
			combatManager.Enemies [this] = nextMove;
			StartCoroutine (SmoothMovement (nextMove));
		}
	}

	private void EndTurn () {
		hasTurn = false;
		hasTarget = false;
		portraitController.SetHighlighted (false);
		combatManager.EndTurn ();
	}

	private void StartAttacking () {
		isAttacking = true;
		Invoke ("EndAttacking", 0.5f);
	}

	private void EndAttacking () {
		isAttacking = false;
	}

	// Combatant interface implementation
	public Helpers.CombatState GetCurrentState () {
		if (HpLeft > 0) {
			return Helpers.CombatState.Alive;
		} else {
			return Helpers.CombatState.Dead;
		}
	}

	public string GetCombatTag () {
		return "Enemy1";
	}

	public int GetInitiativeBonus () {
		return 2;
	}

	public int GetArmorClass() {
		return 10;
	}

	public int GetToHitBonus() {
		return 2;
	}

	public int GetDamageBonus() {
		return 1;
	}

	public Helpers.DiceRoll GetDamageRoll () {
		return new Helpers.DiceRoll { numDice = 1, sizeDice = 6 };
	}

	public void DealDamage (int dmg) {
		HpLeft-=dmg;
		if (HpLeft <= 0) {
			Debug.Log ("Enemy Vanquished!");
			gameObject.SetActive (false);
			combatManager.CheckEndCombat ();
		}
	}
		
	public void GiveTurn () {
		if (GetCurrentState () != Helpers.CombatState.Alive) {
			combatManager.EndTurn ();
		}
		portraitController.SetHighlighted (true);

		MovesLeft = maxMoves;
		AttacksLeft = maxAttacks;
		hasTurn = true;
	}
}
