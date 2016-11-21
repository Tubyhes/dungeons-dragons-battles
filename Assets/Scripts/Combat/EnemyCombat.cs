using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyCombat : Enemy, ICombatant {

	public float moveSpeed = 5f;

	private bool isMoving;
	private bool hasTurn;
	private bool isAttacking;
	private bool hasTarget;

	private Vector3 target;

	private CombatManager combatManager;
	private CharacterSheet characterSheet;

	public delegate void HasTurnChanged (bool hasTurn);
	public HasTurnChanged hasTurnChanged;

	void Awake () {
		characterSheet = GetComponent<CharacterSheet> ();
		characterSheet.DogCharacterSheet ();
		characterSheet.combatStatusChangedDelegate += CombatStatusChanged;
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		hasTurn = false;
		isMoving = false;
		isAttacking = false;
		hasTarget = false;

		combatManager = FindObjectOfType (typeof(CombatManager)) as CombatManager;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasTurn || isMoving || isAttacking) {
			return;
		}

		if (characterSheet.MovesLeft == 0 && characterSheet.AttacksLeft == 0) {
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
		return combatManager.Enemies [characterSheet];
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

		foreach (KeyValuePair<CharacterSheet, Vector3> player in combatManager.Players) {
			float distance = Helpers.EditingDistance (here, player.Value);
			if (distance < minDistance && player.Key.GetCombatState() == Helpers.CombatState.Alive) {
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
			if (characterSheet.AttacksLeft > 0) {
				// if we still have attacks left, then attack!
				CharacterSheet player = combatManager.GetPlayerAtGridPosition (target);
				combatManager.MeleeAttack (characterSheet, player);
				characterSheet.AttacksLeft--;
				return;
			}
			if (characterSheet.AttacksLeft == 0) {
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
			if (characterSheet.MovesLeft == 0) {
				characterSheet.MovesLeft += (characterSheet.Speed / characterSheet.NumAttacks);
				characterSheet.AttacksLeft--;
			}
			characterSheet.MovesLeft--;
			isMoving = true;
			combatManager.Enemies [characterSheet] = nextMove;
			StartCoroutine (SmoothMovement (nextMove));
		}
	}

	private void EndTurn () {
		hasTurn = false;
		hasTarget = false;
		hasTurnChanged (hasTurn);
		combatManager.EndTurn ();
	}

	private void CombatStatusChanged () {
		if (characterSheet.GetCombatState () != Helpers.CombatState.Alive) {
			gameObject.SetActive (false);
		}
	}

	private void StartAttacking () {
		isAttacking = true;
		Invoke ("EndAttacking", 0.5f);
	}

	private void EndAttacking () {
		isAttacking = false;
	}
		
	public string GetCombatTag () {
		return "Enemy1";
	}
				
	public void GiveTurn () {
		if (characterSheet.GetCombatState () != Helpers.CombatState.Alive) {
			combatManager.EndTurn ();
		}

		characterSheet.MovesLeft = characterSheet.Speed;
		characterSheet.AttacksLeft = characterSheet.NumAttacks;
		hasTurn = true;
		hasTurnChanged (hasTurn);
	}
}
