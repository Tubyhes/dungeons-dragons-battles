using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatActionSelectorController : MonoBehaviour {

	// populate this in editor for each human-controller combat character
	public PlayerCombat combatant;

	// all possible combat actions and their icons
	public CombatActionController cancel;
	public CombatActionController attack;
	public CombatActionController fullDefense;
	public CombatActionController castSpell;
	public CombatActionController useItem;
	public CombatActionController endTurn;

	// dictionary indicating whether each action is available
	private Dictionary<Vector2, CombatActionController> actions;
	private Vector2 currentSelection;
	private Helpers.CombatAction currentAction;

	public bool axisInUse = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeSelf) {

			// listen to cancel button
			if (Input.GetButtonDown ("Fire2")) {
				QuitSelector ();
				return;
			}

			// listen to select action
			if (Input.GetButtonDown ("Fire1")) {
				ExecuteCurrentAction ();
				return;
			}

			// listen to horizontal / vertical input
			Vector2 dir = GetInput ();
			if (dir.Equals (Vector2.zero)) {
				return;
			} else {
				MoveHighlight (dir);
				return;
			}


		}
	}

	private Vector2 GetInput () {
		float fx = Input.GetAxis ("Horizontal");
		float fy = Input.GetAxis ("Vertical");
		if (Mathf.Abs(fx) > Mathf.Abs(fy)) {
			fx = fx > 0f ? 1f : -1f;
			fy = 0f;
		} else if (Mathf.Abs(fy) > Mathf.Abs(fx))  {
			fx = 0f;
			fy = fy > 0f ? 1f : -1f;
		}

		if ((fx != 0f || fy != 0f) && !axisInUse) {
			axisInUse = true;
			return new Vector2 (fx, fy);
		} 

		if (fx == 0f && fy == 0f && axisInUse) {
			axisInUse = false;
		}

		return Vector2.zero;
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

	private void ExecuteCurrentAction () {
		// here need to call corresponding function on combatant delegate
		// TODO: in case of attack, spell, item, need to also find out the target and the specific spell/item
		if (currentAction == Helpers.CombatAction.Attack) {
			combatant.Attack ();
		} else if (currentAction == Helpers.CombatAction.CastSpell) {

		} else if (currentAction == Helpers.CombatAction.FullDefense) {

		} else if (currentAction == Helpers.CombatAction.UseItem) {

		} else if (currentAction == Helpers.CombatAction.EndTurn) {
			combatant.ForceEndTurn ();
		}

		// disable the ActionSelector and tell Combatant it's not in menu any more
		QuitSelector ();
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
		gameObject.SetActive(true);

	}

}
