using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellPicker : MonoBehaviour {

	public GameObject spellPickerList;
	public ScrollRect scrollRect;

	public delegate void SpellSelected (Spell spell);
	private SpellSelected spellSelectedCallback;
	public delegate void SpellPickerCancelled ();
	private SpellPickerCancelled spellPickerCancelledCallback;

	private Helpers.Teams team;
	private List<KeyValuePair<Spell, SpellPickerItem>> spellList;
	private int currentIndex;
	private bool firstHighlight = true;

	private SoundManager soundManager;

	void Awake () {
		spellList = new List<KeyValuePair<Spell, SpellPickerItem>> ();
		currentIndex = 0;
		soundManager = FindObjectOfType (typeof(SoundManager)) as SoundManager;
	}
		
	public void InitialiseSpellList (List<Spell> spells, SpellSelected callback, SpellPickerCancelled cancelCallback, Helpers.Teams _team) {
		if (spells.Count == 0 || callback == null) {
			return;
		}

		for (int i = 0; i < spells.Count; i++) {
			GameObject item = Instantiate (Resources.Load ("SpellPickerItem")) as GameObject;
			SpellPickerItem spellPickerItem = item.GetComponent<SpellPickerItem> ();
			spellPickerItem.transform.SetParent (spellPickerList.transform);
			spellPickerItem.SetSpell (spells [i]);
			spellList.Add (new KeyValuePair<Spell, SpellPickerItem> (spells [i], spellPickerItem));
		}

		transform.localScale = Vector3.one / 100;
		spellSelectedCallback = callback;
		spellPickerCancelledCallback = cancelCallback;
		team = _team;
	}

	// Update is called once per frame
	void Update () {
		if (spellList.Count == 0) {
			return;
		}

		if (firstHighlight) {
			spellList [0].Value.SetHighlight (true);
			firstHighlight = false;
			return;
		}

		if (Input.GetButtonDown (Helpers.Fire1 (team))) {
			soundManager.PlayMenuItemSelect ();
			spellSelectedCallback (spellList [currentIndex].Key);
			Destroy (gameObject);
			return;
		}

		if (Input.GetButtonDown (Helpers.Fire2 (team))) {
			soundManager.PlayMenuCancel ();
			spellPickerCancelledCallback ();
			Destroy (gameObject);
			return;
		}

		Vector2 dir = AxisButtonDownGetter.GetInput (team);
		if (dir.y == 0f) {
			return;
		} else {
			spellList [currentIndex].Value.SetHighlight (false);
			if (dir.y > 0 && currentIndex > 0) {
				soundManager.PlayMenuItemSelect ();
				currentIndex--;
			} else if (dir.y < 0 && currentIndex < spellList.Count - 1) {
				soundManager.PlayMenuItemSelect ();
				currentIndex++;
			}

			spellList [currentIndex].Value.SetHighlight (true);
			return;
		}
	}

}
