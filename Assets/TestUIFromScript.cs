using UnityEngine;
using System.Collections;

public class TestUIFromScript : MonoBehaviour {

	public GameObject spellPickerPrefab;
	private bool pickingSpell = false;

	// Update is called once per frame
	void Update () {
		if (pickingSpell) {
			return;
		}

		if (Input.GetButtonDown("Fire1_P1")) {
			GameObject spellPickerObject = Instantiate (spellPickerPrefab) as GameObject;
			SpellPicker spellPicker = spellPickerObject.GetComponent<SpellPicker> ();
			spellPicker.InitialiseSpellList (AllSpells.spells, spellSelectedCallback, null, Helpers.Teams.Home);

			spellPicker.transform.position = new Vector3 (5.5f, 2f, 0f);

			pickingSpell = true;
		}
	}

	public void spellSelectedCallback  (Spell spell) {
		Debug.Log ("Selected spell: " + spell.spellName);
		pickingSpell = false;
	}

}
