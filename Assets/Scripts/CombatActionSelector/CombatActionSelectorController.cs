using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatActionSelectorController : MonoBehaviour {

	// populate this in editor for each human-controller combat character
	public PlayerInputCombat combatant;

	// all possible combat actions and their icons
	public GameObject actionIconHolder;
	public CombatActionController cancel;
	public CombatActionController attack;
	public CombatActionController fullDefense;
	public CombatActionController castSpell;
	public CombatActionController useItem;
	public CombatActionController endTurn;

	// target selector object
	public CombatTargetSelectorController targetSelector;

	// dictionary indicating whether each action is available
	private Dictionary<Vector2, CombatActionController> actions;
	private Vector2 currentSelection;
	private Helpers.CombatAction currentAction;
	private bool isSelectingTarget = false;
	private bool ignoreFirstClick = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.activeSelf || isSelectingTarget) {
			return;
		}

		if (ignoreFirstClick) {
			ignoreFirstClick = false;
			return;
		}

		// listen to cancel button
		if (Input.GetButtonDown ("Fire2")) {
			QuitSelector ();
			return;
		}

		// listen to select action
		if (Input.GetButtonDown ("Fire1")) {
			ActionSelected ();
			return;
		}

		// listen to horizontal / vertical input
		Vector2 dir = AxisButtonDownGetter.GetInput ();
		if (dir.Equals (Vector2.zero)) {
			return;
		} else {
			MoveHighlight (dir);
			return;
		}
	}

	private void MoveHighlight (Vector2 dir) {
		Vector2 newSelection = currentSelection + dir;
		if (actions.ContainsKey (newSelection)) {
			actions [currentSelection].SetHighlight (false);

			CombatActionController newAction = actions [newSelection];
			newAction.SetHighlight (true);
			currentAction = newAction.action;
			currentSelection = newSelection;
		}
	}

	private void ActionSelected () {
		// here need to call corresponding function on combatant delegate
		// TODO: in case of attack, spell, item, need to also find out the target and the specific spell/item
		if (currentAction == Helpers.CombatAction.Attack) {
			StartTargetSelector (1, 0, true);
		} else if (currentAction == Helpers.CombatAction.CastSpell) {

		} else if (currentAction == Helpers.CombatAction.FullDefense) {
			combatant.GoFullDefense ();
			QuitSelector ();
		} else if (currentAction == Helpers.CombatAction.UseItem) {

		} else if (currentAction == Helpers.CombatAction.EndTurn) {
			combatant.ForceEndTurn ();
			QuitSelector ();
		} else if (currentAction == Helpers.CombatAction.Cancel) {
			QuitSelector ();
		}

		// disable the ActionSelector and tell Combatant it's not in menu any more
	}

	private void QuitSelector () {
		actions.Clear ();
		gameObject.SetActive (false);
		combatant.SelectorClosed ();
	}

	public void DisplaySelector () {
		// first empty the dictionary, and fill it up with the currently available actions
		actions = new Dictionary<Vector2, CombatActionController> ();

		if (combatant.CanAttack ()) {
			actions.Add (new Vector2 (0f, 1f), attack);
			attack.SetAvailable (true);
		} else {
			attack.SetAvailable (false);
		}

		if (combatant.CanFullDefense ()) {
			actions.Add (new Vector2 (-1f, 1f), fullDefense);
			fullDefense.SetAvailable (true);
		} else {
			fullDefense.SetAvailable (false);
		}

		if (combatant.CanCastSpell ()) {
			actions.Add (new Vector2 (1f, 0f), castSpell);
			castSpell.SetAvailable (true);
		} else {
			castSpell.SetAvailable (false);
		}

		if (combatant.CanUseItem ()) {
			actions.Add (new Vector2 (0f, -1f), useItem);
			useItem.SetAvailable (true);
		} else {
			useItem.SetAvailable (false);
		}


		actions.Add (new Vector2 (0f, 0f), cancel);
		cancel.SetHighlight (true);
		actions.Add (new Vector2 (-1f, 0f), endTurn);
		endTurn.SetHighlight (false);

		currentSelection = new Vector2 (0f, 0f);
		currentAction = Helpers.CombatAction.Cancel;

		// set self to active
		ignoreFirstClick = true;
		gameObject.SetActive (true);
		actionIconHolder.SetActive (true);
	}

	private void StartTargetSelector (int range, int aoe, bool targetRequired) {
		isSelectingTarget = true;
		actionIconHolder.SetActive (false);
		targetSelector.Enable (range, aoe, targetRequired);
	}

	public void TargetSelectorCancelled () {
		ignoreFirstClick = true;
		actionIconHolder.SetActive (true);
		isSelectingTarget = false;
	}

	public void TargetSelected (Vector3 target) {
		Debug.Log ("Target selected: " + target);
		isSelectingTarget = false;

		if (currentAction == Helpers.CombatAction.Attack) {
			combatant.Attack (target);
		} else if (currentAction == Helpers.CombatAction.CastSpell) {

		} else if (currentAction == Helpers.CombatAction.UseItem) {

		}

		QuitSelector ();
	}
}
