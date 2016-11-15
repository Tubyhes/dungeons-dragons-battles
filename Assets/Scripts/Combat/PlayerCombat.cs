using UnityEngine;
using System.Collections;

public class PlayerCombat : Player, ICombatant {

	public float moveSpeed = 50f;
	public Sprite portrait;
	public CombatActionSelectorController actionSelector;

	private bool isMoving;
	private bool hasTurn;
	private bool selectingAction;

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

	protected override void Start () {
		base.Start ();

		hasTurn = false;
		isMoving = false;
		isAttacking = false;

		// always use a range of 1 in Combat Mode
		range = 1f;

		// register self as combatant in this combat
		combatManager = FindObjectOfType (typeof(CombatManager)) as CombatManager;
		combatManager.RegisterPlayer (this, transform.position, out portraitController);

		// populate the portrait
		portraitController.Enable();
		portraitController.SetImage (portrait);
		MovesLeft = 0; 
		AttacksLeft = 0;
		HpLeft = maxHitPoints;

	}

	// Update is called once per frame
	void Update () {

		if (isMoving || isAttacking || !hasTurn || GetCurrentState() != Helpers.CombatState.Alive || selectingAction) {
			return;
		}

		if (movesLeft == 0 && attacksLeft == 0) {
			EndTurn ();
			return;
		}

		if (Input.GetButtonDown ("Fire1")) {
			selectingAction = true;
			actionSelector.DisplaySelector ();
			return;
		}

		int horizontal = (int)(Input.GetAxisRaw ("Horizontal"));
		int vertical = horizontal != 0 ? 0 : (int)(Input.GetAxisRaw ("Vertical"));
		if (horizontal != 0 || vertical != 0) {
			Move (horizontal, vertical);
		}
			
	}

	private IEnumerator SmoothMovement (Vector3 target) 
	{
		while ((transform.position - target).sqrMagnitude > 0.00001f) {
			Vector3 newPosition = Vector3.MoveTowards (rb2d.position, target, Time.deltaTime * moveSpeed);
			rb2d.MovePosition (newPosition);
			yield return null;
		}

		animator.SetBool ("walk", false);
		isMoving = false;
	}

	public void ForceEndTurn () {
		MovesLeft = 0;
		AttacksLeft = 0;
	}

	private void EndTurn () {
		hasTurn = false;
		portraitController.SetHighlighted (false);
		combatManager.EndTurn ();
	}

	public void Attack () {
		StartAttackAnimation ();

		if (AttacksLeft == 0) {
			return;
		}

		Vector3 target = FacingGridPosition ();
		ICombatant enemy = combatManager.GetEnemyAtGridPosition (target);
		if (enemy != null) {
			combatManager.MeleeAttack (this, enemy);
			AttacksLeft--;
		}
	}

	private void Move (int horizontal, int vertical) {
		if (horizontal < 0) {
			TurnLeft ();
		} else if (horizontal > 0) {
			TurnRight ();
		} else if (vertical < 0) {
			TurnDown ();
		} else {
			TurnUp ();
		}

		Vector3 target = FacingGridPosition ();
		if (!combatManager.IsGridPositionFree (target)) {
			return;
		}

		if (MovesLeft == 0 && AttacksLeft == 0) {
			return;
		} else if (MovesLeft == 0) {
			MovesLeft += (maxMoves / maxAttacks);
			AttacksLeft--;
		}

		MovesLeft--;
		isMoving = true;
		animator.SetBool ("walk", true);
		combatManager.Players [this] = target;
		StartCoroutine (SmoothMovement (target));
	}

	private Vector3 GridPosition () {
		return combatManager.Players [this];
	}

	private Vector3 FacingGridPosition () {
		Vector3 pos = GridPosition ();
		Helpers.Direction dir = FacingDirection ();

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

	public bool CanAttack () {
		if (attacksLeft > 0) {
			return true;
		}
		return false;
	}

	public bool CanFullDefense () {
		if (attacksLeft == maxAttacks) {
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

	// Combatant interface implementation
	public Helpers.CombatState GetCurrentState () {
		if (hpLeft > 0) {
			return Helpers.CombatState.Alive;
		} else {
			return Helpers.CombatState.Dead;
		}
	}

	public string GetCombatTag () {
		return "Player1";
	}

	public int GetInitiativeBonus () {
		return 20;
	}

	public int GetArmorClass() {
		return 25;
	}

	public int GetToHitBonus() {
		return 4;
	}

	public int GetDamageBonus() {
		return 2;
	}

	public Helpers.DiceRoll GetDamageRoll () {
		return new Helpers.DiceRoll { numDice = 1, sizeDice = 6 };
	}

	public void DealDamage (int dmg) {
		HpLeft -= dmg;
		if (HpLeft <= 0) {
			Debug.Log ("YOU ARE DEAD!!");
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
