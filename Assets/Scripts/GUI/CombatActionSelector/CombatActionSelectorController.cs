using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatActionSelectorController : MonoBehaviour {

	// populate this in editor for each human-controller combat character
	public PlayerInputCombat combatant;
	public CharacterSheet sheet;

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
	private Spell currentSpell;
	private bool isSelectingTarget = false;
	private bool ignoreFirstClick = true;

	// reference to sound manager to be able to play soundeffects
	private SoundManager soundManager;

	// Use this for initialization
	void Awake () {

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
		if (Input.GetButtonDown (Helpers.Fire2(combatant.team))) {
			soundManager.PlayMenuCancel ();
			QuitSelector ();
			return;
		}

		// listen to select action
		if (Input.GetButtonDown (Helpers.Fire1(combatant.team))) {
			ActionSelected ();
			return;
		}

		// listen to horizontal / vertical input
		Vector2 dir = AxisButtonDownGetter.GetInput (combatant.team);
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

			soundManager.PlayMenuItemSwitch ();
		}
	}

	private void ActionSelected () {
		// here need to call corresponding function on combatant delegate
		// TODO: in case of attack, spell, item, need to also find out the target and the specific spell/item
		if (currentAction == Helpers.CombatAction.Attack) {
			soundManager.PlayMenuItemSelect ();
			StartTargetSelector (1, 0, true);
		} else if (currentAction == Helpers.CombatAction.CastSpell) {
			soundManager.PlayMenuItemSelect ();
			StartSpellPicker ();
		} else if (currentAction == Helpers.CombatAction.FullDefense) {
			soundManager.PlayMenuItemSelect ();
			combatant.GoFullDefense ();
			QuitSelector ();
		} else if (currentAction == Helpers.CombatAction.UseItem) {
			soundManager.PlayMenuItemSelect ();
		} else if (currentAction == Helpers.CombatAction.EndTurn) {
			soundManager.PlayMenuItemSelect ();
			combatant.ForceEndTurn ();
			QuitSelector ();
		} else if (currentAction == Helpers.CombatAction.Cancel) {
			soundManager.PlayMenuCancel ();
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
		if (soundManager == null) {
			soundManager = FindObjectOfType (typeof(SoundManager)) as SoundManager;
		}
		soundManager.PlayMenuOpen ();

		// first empty the dictionary, and fill it up with the currently available actions
		actions = new Dictionary<Vector2, CombatActionController> ();

		if (sheet.CanAttack ()) {
			actions.Add (new Vector2 (0f, 1f), attack);
			attack.SetAvailable (true);
		} else {
			attack.SetAvailable (false);
		}

		if (sheet.CanFullDefense ()) {
			actions.Add (new Vector2 (-1f, 1f), fullDefense);
			fullDefense.SetAvailable (true);
		} else {
			fullDefense.SetAvailable (false);
		}

		if (sheet.CanCastSpell ()) {
			actions.Add (new Vector2 (1f, 0f), castSpell);
			castSpell.SetAvailable (true);
		} else {
			castSpell.SetAvailable (false);
		}

		if (sheet.CanUseItem ()) {
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
			combatant.StartCoroutine(combatant.Attack (target));
		} else if (currentAction == Helpers.CombatAction.CastSpell) {
			combatant.StartCoroutine(combatant.CastSpell (currentSpell, target));
		} else if (currentAction == Helpers.CombatAction.UseItem) {

		}

		QuitSelector ();
	}

	public void StartSpellPicker () {
		isSelectingTarget = true;

		GameObject spellPickerObject = Instantiate (Resources.Load("SpellPickerCanvas")) as GameObject;
		SpellPicker spellPicker = spellPickerObject.GetComponent<SpellPicker> ();
		List<Spell> spellList = AllSpells.EligibleSpells (sheet);
		spellPicker.InitialiseSpellList (spellList, SpellSelected, SpellPickerCancelled, combatant.team);
		spellPicker.transform.position = SpellPickerPosition(spellList.Count, combatant.transform.position);
	}

	private Vector3 SpellPickerPosition (int numSpells, Vector3 origin) {
		Vector3 pos = Vector3.zero;
		int verticalSize = Mathf.Min (numSpells, 8);

		if (origin.x > 5.5f) {
			pos.x = origin.x - 4;
		} else {
			pos.x = origin.x + 4;
		}

		float top_deficit = (8f - origin.y) - ((float)verticalSize / 2f - 0.5f);
		float bot_deficit = (origin.y - 1f) - ((float)verticalSize / 2f - 0.5f);
		if (top_deficit < 0f) {
			pos.y = origin.y + top_deficit;
		} else if (bot_deficit < 0f) {
			pos.y = origin.y - bot_deficit;
		} else {
			pos.y = origin.y;
		}

		return pos;
	}

	public void SpellPickerCancelled () {
		ignoreFirstClick = true;
		isSelectingTarget = false;
	}

	public void SpellSelected (Spell spell) {
		ignoreFirstClick = true;
		Debug.Log (spell.spellName);
		currentSpell = spell;
		StartTargetSelector (currentSpell.range, currentSpell.aoe, true);
	}
}
