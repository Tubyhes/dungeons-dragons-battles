using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class FeatsEquipmentController : MonoBehaviour {

	public CharacterSheet sheet;

	// feat interaction elements
	public Dropdown[] featDropdowns;
	public MyButton[] setButtons;
	public MyButton[] resetButtons;
	public Text[] descriptions;
	public Image[] featBackgrounds;
	public Text numFeats;

	private List<Feats.Feat> currentFeatOptions;
	private List<Feats.Feat> selectedFeatOptions;

	// equipment interaction elements
	public Dropdown chestDropdown;
	public Text chestDescription;
	public Dropdown mainHandDropdown;
	public Text mainHandDescription;
	public Dropdown offHandDropdown;
	public Text offHandDescription;

	private List<Armor> currentChestOptions;
	private List<Weapon> currentMainHandOptions;
	private List<Equipment> currentOffHandOptions;

	// spell elements
	public Text spellsPerDay;
	public Text concentration;
	public Text spellDC;

	// hitpoint element
	public Text hitPoints;

	public void StartCustomization (CharacterSheet character_sheet) {
		gameObject.SetActive (true);
		ClearFeats ();
		ClearEquipment ();
		sheet = character_sheet;
		PopulateScores ();
		// TODO: clear feats and equipment

		numFeats.text = sheet.num_feats.ToString ();
		selectedFeatOptions = new List<Feats.Feat> ();

		for (int i = 0; i < featDropdowns.Length; i++) {
			featDropdowns [i].interactable = false;
			setButtons [i].interactable = false;
			resetButtons [i].interactable = false;
		}

		for (int i = featDropdowns.Length - 1; i >= sheet.num_feats; i--) {
			featBackgrounds [i].color = new Color (0.8f, 0.8f, 0.8f, 1f);
		}

		ActivateFeatDropdown (0);

	}

	private void ClearFeats () {
		for (int i = 0; i < featDropdowns.Length; i++) {
			featDropdowns [i].ClearOptions ();
			descriptions [i].text = "";
		}
	}

	private void ClearEquipment () {
		chestDropdown.ClearOptions ();
		chestDescription.text = "";
		mainHandDropdown.ClearOptions ();
		mainHandDescription.text = "";
		offHandDropdown.ClearOptions ();
		offHandDescription.text = "";
	}

	private void PopulateScores () {
		sheet.CalcMaxSpells ();
		sheet.CalcMaxHitpoints ();
		spellsPerDay.text = sheet.num_spells.ToString ();
		concentration.text = sheet.GetConcentrationModifier ().ToString ();
		spellDC.text = sheet.GetSpellDC ().ToString ();
		hitPoints.text = sheet.max_hitpoints.ToString ();
	}

	private void PopulateFeatDropdown (int index) {
		currentFeatOptions = Feats.allEligible (sheet);
		currentFeatOptions.RemoveAll(x => selectedFeatOptions.Contains(x));
		List<string> options = Feats.featsToString (currentFeatOptions);
		options.Insert (0, "None");
		featDropdowns [index].ClearOptions ();
		featDropdowns [index].AddOptions (options);
		featDropdowns [index].value = 0;
	}

	private void ActivateFeatDropdown (int index) {
		if (index < featDropdowns.Length && index < sheet.num_feats) {
			// populate the dropdown
			PopulateFeatDropdown (index);

			// enable set button and dropdown
			setButtons [index].interactable = true;
			featDropdowns [index].interactable = true;

			// select the dropdown
			featDropdowns [index].Select ();
		} else {
			ActivateEquipment ();
			return;
		}

		// enable reset button of previous feat if applicable
		if (index > 0) {
			resetButtons [index - 1].interactable = true;
		}
	}

	private void ActivateEquipment () {
		// populate chest dropdown
		PopulateChestArmor ();
		chestDropdown.Select ();

		// populate main hand dropdown
		PopulateMainHand ();

		// off hand dropdown is not interactable until we selected a main hand item
		offHandDropdown.interactable = false;
	}

	private void PopulateChestArmor () {
		currentChestOptions = Armor.eligibleChestArmor(sheet.character_class);
		List<string> options = currentChestOptions.Select (x => x.equipment_tag).ToList ();
		options.Insert (0, "None");
		chestDropdown.ClearOptions ();
		chestDropdown.AddOptions (options);
		chestDropdown.value = 0;
	}

	private void PopulateMainHand () {
		currentMainHandOptions = Weapon.eligibleMainHandWeapons (sheet.character_class);
		List<string> options = currentMainHandOptions.Select (x => x.equipment_tag).ToList ();
		options.Insert (0, "None");
		mainHandDropdown.ClearOptions ();
		mainHandDropdown.AddOptions (options);
		mainHandDropdown.value = 0;
	}

	private void PopulateOffHand () {
		currentOffHandOptions = new List<Equipment> ();
		List<string> options = new List<string> () { "None" };

		if (sheet.weapon_main.weapon_size != Weapon.WeaponSize.TwoHanded &&
		    sheet.feats.Contains (Feats.Feat.TwoWeaponDefense)) {
			currentOffHandOptions.AddRange (Weapon.eligibleOffHandWeapons (sheet.character_class).Cast<Equipment> ());
		}
		if (sheet.weapon_main.weapon_size != Weapon.WeaponSize.TwoHanded) {
			currentOffHandOptions.AddRange (Armor.eligibleShields (sheet.character_class).Cast<Equipment> ());
		}

		options.AddRange (currentOffHandOptions.Select (x => x.equipment_tag).ToList ());
		offHandDropdown.ClearOptions ();
		offHandDropdown.AddOptions (options);
		offHandDropdown.value = 0;
	}

	private string ArmorDescription (Armor armor) {
		return "AC: +" + armor.armor_bonus + ", Max Dex: " + armor.max_dexterity +
		"\n" + "Speed: " + armor.max_speed;
	}

	private string WeaponDescription (bool mainHand) {
		Weapon weapon = mainHand ? sheet.weapon_main : sheet.weapon_off;
		return "ToHit: +" + sheet.GetToHitBonus (mainHand) + ", Dmg: " +
		Helpers.DiceRollToString (weapon.damage_roll) + " " + WeaponCritDescription (weapon);
	}

	private string WeaponCritDescription (Weapon weapon) {
		if (weapon.crit_range == 1) {
			return "Crit: 20/" + weapon.crit_modifier;
		} else {
			return "Crit: " + (21 - weapon.crit_range) + "-20/" + weapon.crit_modifier;
		}
	}

	public void OnDropdownValueChanged (int index) {
		// populate the description for this dropdown
		int value = featDropdowns[index].value - 1;
		if (value < 0) {
			descriptions [index].text = "";
		} else {
			descriptions [index].text = Feats.featToDescription [currentFeatOptions [value]];
		}
	}

	public void OnSetButtonPressed (int index) {
		// add currently selected feat to sheet.feats
		int value = featDropdowns[index].value - 1;
		selectedFeatOptions.Add (currentFeatOptions [value]);
		sheet.feats.Add (currentFeatOptions [value]);
		PopulateScores ();

		// disable set button, disable dropdown
		featDropdowns [index].interactable = false;
		setButtons [index].interactable = false;

		// disable previous feat's reset button if available
		if (index > 0) {
			resetButtons [index - 1].interactable = false;
		}

		// activate next feat dropdown
		ActivateFeatDropdown(index + 1);
	}

	public void OnResetButtonPressed (int index) {
		// remove currently selected feat from sheet.feats
		sheet.feats.Remove (selectedFeatOptions [index]);
		selectedFeatOptions.RemoveAt (index);

		// set selection for this dropdown to None
		featDropdowns[index].value = 0;

		// disable set button and dropdown of next feat if applicable
		if (index < featDropdowns.Length) {
			featDropdowns [index + 1].interactable = false;
			setButtons [index + 1].interactable = false;
		}

		// disable reset button of this feat
		resetButtons[index].interactable = false;

		// activate this feat
		ActivateFeatDropdown (index);
	}

	public void OnChestDropdownValueChanged () {
		int value = chestDropdown.value - 1;
		Debug.Log ("OnChesDropdownValueChanged: " + value);
		if (value < 0) {
			chestDescription.text = "";
			return;
		}

		sheet.armor = currentChestOptions [value];
		chestDescription.text = ArmorDescription (sheet.armor);
	}

	public void OnMainHandDropdownValueChanged () {
		int value = mainHandDropdown.value - 1;
		Debug.Log ("OnMainHandDropdownValueChanged: " + value);
		if (value < 0) {
			// if we set this to None, we cant select an off hand
			mainHandDescription.text = "";
			offHandDropdown.interactable = false;
			return;
		}

		sheet.weapon_main = currentMainHandOptions [value];
		mainHandDescription.text = WeaponDescription (true);

		if (sheet.weapon_main.weapon_size == Weapon.WeaponSize.TwoHanded) {
			offHandDropdown.interactable = false;
		} else {
			// if we dont have a two-handed weapon in main hand, we can populate off hand
			PopulateOffHand ();
			offHandDropdown.interactable = true;
		}
	}

	public void OnOffHandDropdownValueChanged () {
		int value = offHandDropdown.value - 1;
		Debug.Log ("OnOffHandDropdownValueChanged: " + value);
		if (value < 0) {
			// if we set off hand to empty, we can change main hand
			offHandDescription.text = "";
			mainHandDropdown.interactable = true;
			return;
		}

		Equipment e = currentOffHandOptions [value];
		if (e is Weapon) {
			sheet.weapon_off = (Weapon) e;
			offHandDescription.text = WeaponDescription (false);
		} else if (e is Armor) {
			sheet.shield = (Armor) e;
			offHandDescription.text = ArmorDescription (sheet.shield);
		}

		// cant change main hand if off hand is populated
		mainHandDropdown.interactable = false;
	}
}
