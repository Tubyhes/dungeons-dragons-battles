using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpellPickerItem : MonoBehaviour {

	public Image spellIcon;
	public Text spellTitle;
	public Text spellDescription;
	public GameObject backgroundPanel;

	void Start () {
		backgroundPanel.SetActive (false);
	}

	public void SetSpell (Spell spell) {
		spellTitle.text = spell.spellName;
		spellDescription.text = spell.description;
		spellIcon.sprite = Resources.Load<Sprite> (spell.icon);
	}

	public void SetHighlight (bool highlighted) {
		backgroundPanel.SetActive (highlighted);
	}

}
