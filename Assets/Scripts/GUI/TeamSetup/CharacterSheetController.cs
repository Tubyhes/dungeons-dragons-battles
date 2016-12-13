using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class CharacterSheetController : MonoBehaviour {

	// sheet to fill
	public CharacterSheet sheet;
	// ability points to spend
	private int points_left = 32;
	public Text points_left_text;

	// first button to select when opening CharacterSheet
	public MyButton strength_up;

	// text field for character details
	public Text character_class;
	public Text character_name;
	public Text hit_die;
	public Text base_attack;

	// text fields for ability scores
	public Text strength;
	public Text strength_modifier;
	public Text dexterity;
	public Text dexterity_modifier;
	public Text constitution;
	public Text constitution_modifier;
	public Text intelligence;
	public Text intelligence_modifier;
	public Text wisdom;
	public Text wisdom_modifier;
	public Text charisma;
	public Text charisma_modifier;

	// text fields for saves
	public Text fortitude_total;
	public Text fortitude_base;
	public Text fortitude_ability;
	public Text reflex_total;
	public Text reflex_base;
	public Text reflex_ability;
	public Text will_total;
	public Text will_base;
	public Text will_ability;

	public void StartCustomization (CharacterSheet character_sheet) {
		gameObject.SetActive (true);
		sheet = character_sheet;
		points_left = 32;
		FillTextFields ();
		strength_up.Select ();
	}

	private void FillTextFields () {
		
		// fill character details
		character_class.text = Helpers.classToString[sheet.character_class];
		character_name.text = sheet.character_name;
		hit_die.text = "d" + sheet.hit_die;
		base_attack.text = "+" + sheet.base_attack;

		// fill ability points left
		points_left_text.text = points_left.ToString();

		// fill ability scores
		strength.text 		= sheet.strength.ToString();
		dexterity.text 		= sheet.dexterity.ToString();
		constitution.text 	= sheet.constitution.ToString();
		intelligence.text 	= sheet.intelligence.ToString();
		wisdom.text 		= sheet.wisdom.ToString();
		charisma.text 		= sheet.charisma.ToString();

		// fill ability modifiers
		strength_modifier.text 		= sheet.GetStrengthModifier ().ToString();
		dexterity_modifier.text 	= sheet.GetDexterityModifier ().ToString();
		constitution_modifier.text 	= sheet.GetConstitutionModifier ().ToString();
		intelligence_modifier.text 	= sheet.GetIntelligenceModifier ().ToString();
		wisdom_modifier.text 		= sheet.GetWisdomModifier ().ToString();
		charisma_modifier.text 		= sheet.GetCharsimaModifier ().ToString();

		// fill saves
		fortitude_total.text 	= sheet.GetFortitude().ToString();
		fortitude_base.text 	= sheet.fortitude.ToString();
		fortitude_ability.text 	= sheet.GetConstitutionModifier ().ToString();
		reflex_total.text 		= sheet.GetReflex ().ToString();
		reflex_base.text 		= sheet.reflex.ToString();
		reflex_ability.text 	= sheet.GetDexterityModifier ().ToString();
		will_total.text 		= sheet.GetWill ().ToString();
		will_base.text 			= sheet.will.ToString();
		will_ability.text 		= sheet.GetWisdomModifier ().ToString();

		sheet.CalcMaxHitpoints ();
	}


	public void OnAbilityUp (string ability) {
		bool change = false;

		switch (ability) {
		case "strength":
			change = AbilityUp (ref sheet.strength);
			break;
		case "dexterity":
			change = AbilityUp (ref sheet.dexterity);
			break;
		case "constitution":
			change = AbilityUp (ref sheet.constitution);
			break;
		case "intelligence":
			change = AbilityUp (ref sheet.intelligence);
			break;
		case "wisdom":
			change = AbilityUp (ref sheet.wisdom);
			break;
		case "charisma":
			change = AbilityUp (ref sheet.charisma);
			break;
		default:
			return;
		}

		if (change) {
			FillTextFields ();
		}
	}

	public void OnAbilityDown (string ability) {
		bool change = false;

		switch (ability) {
		case "strength":
			change = AbilityDown (ref sheet.strength);
			break;
		case "dexterity":
			change = AbilityDown (ref sheet.dexterity);
			break;
		case "constitution":
			change = AbilityDown (ref sheet.constitution);
			break;
		case "intelligence":
			change = AbilityDown (ref sheet.intelligence);
			break;
		case "wisdom":
			change = AbilityDown (ref sheet.wisdom);
			break;
		case "charisma":
			change = AbilityDown (ref sheet.charisma);
			break;
		default:
			return;
		}

		if (change) {
			FillTextFields ();
		}
	}

	private bool AbilityUp (ref int ability) {
		int points_required = Int32.MaxValue;

		if (ability >= 16) {
			points_required = 3;
		} else if (ability >= 14) {
			points_required = 2;
		} else {
			points_required = 1;
		}

		if (points_left >= points_required) {
			points_left -= points_required;
			ability++;
			return true;
		}

		return false;
	}

	private bool AbilityDown (ref int ability) {
		if (ability <= 8) {
			return false;
		}

		int points_earned = 1;
		if (ability > 16) {
			points_earned = 3;
		} else if (ability > 14) {
			points_earned = 2;
		}

		points_left += points_earned;
		ability--;
		return true;
	}

}
