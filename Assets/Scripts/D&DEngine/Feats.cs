using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class Feats {

	public enum Feat {
		CombatCasting,
		Dodge,
		GreatFortitude,
		ImprovedCritical,
		ImprovedInitiative,
		ImprovedUnarmedStrike,
		DeflectArrows,
		IronWill,
		LightningReflexes,
		PointBlankShot,
		PreciseShot,
		RapidShot,
		PowerAttack,
		Cleave,
		GreatCleave,
		SpellFocus,
		GreaterSpellFocus,
		Toughness,
		TwoWeaponFighting,
		TwoWeaponDefense,
		WeaponFinesse,
		WeaponFocus,
		WeaponSpecialization
	}

	public static List<Feat> notImplemented = new List<Feat> {
		Feat.ImprovedUnarmedStrike,
		Feat.DeflectArrows,
		Feat.PointBlankShot,
		Feat.PreciseShot,
		Feat.RapidShot,
		Feat.PowerAttack,
		Feat.Cleave,
		Feat.GreatCleave,
	};

	public static Dictionary<Feat, string> featToString = new  Dictionary<Feat, string> {
		{ Feat.CombatCasting, "Combat Casting" },
		{ Feat.Dodge, "Dodge" },
		{ Feat.GreatFortitude, "Great Fortitude" },
		{ Feat.ImprovedCritical, "Improved Critical" },
		{ Feat.ImprovedInitiative, "Improved Initiative" },
		{ Feat.ImprovedUnarmedStrike, "Improved Unarmed Strike" },
		{ Feat.DeflectArrows, "Deflect Arrows" },
		{ Feat.IronWill, "Iron Will" },
		{ Feat.LightningReflexes, "Lightning Reflexes" },
		{ Feat.PointBlankShot, "Point Blank Shot" },
		{ Feat.PreciseShot, "Precise Shot" },
		{ Feat.RapidShot, "Rapid Shot" },
		{ Feat.PowerAttack, "Power Attack" },
		{ Feat.Cleave, "Cleave" },
		{ Feat.GreatCleave, "Great Cleave" },
		{ Feat.SpellFocus, "Spell Focus" },
		{ Feat.GreaterSpellFocus, "Greater Spell Focus" },
		{ Feat.Toughness, "Toughness" },
		{ Feat.TwoWeaponFighting, "Two Weapon Fighting" },
		{ Feat.TwoWeaponDefense, "Two Weapon Defense" },
		{ Feat.WeaponFinesse, "Weapon Finesse" },
		{ Feat.WeaponFocus, "Weapon Focus" },
		{ Feat.WeaponSpecialization, "Weapon Specialization" },
	};

	public static Dictionary<Feat, string> featToDescription = new Dictionary<Feat, string> {
		{ Feat.CombatCasting, "+4 bonus on Concentration checks." },
		{ Feat.Dodge, "+1 bonus on Armor Class." },
		{ Feat.GreatFortitude, "+2 bonus on Fortitude saves." },
		{ Feat.ImprovedCritical, "Double threat range of weapon." },
		{ Feat.ImprovedInitiative, "+4 bonus on Initiative checks." },
		{ Feat.ImprovedUnarmedStrike, "Allows you to attack without a weapon." },
		{ Feat.DeflectArrows, "Deflect one ranged attack per round." },
		{ Feat.IronWill, "+2 bonus on Will saves." },
		{ Feat.LightningReflexes, "+2 bonus on Reflex saves." },
		{ Feat.PointBlankShot, "+1 bonus on To Hit and Damage for ranged attacks within 6 squares." },
		{ Feat.PreciseShot, "No -4 To Hit penalty when shooting into melee." },
		{ Feat.RapidShot, "One extra ranged attack each round." },
		{ Feat.PowerAttack, "Trade To Hit bonus for damage (up to base attack bonus)." },
		{ Feat.Cleave, "Extra melee attack after dropping target." },
		{ Feat.GreatCleave, "No limit to cleave attacks each round." },
		{ Feat.SpellFocus, "+1 bonus on DC for saving throws against all your spells." },
		{ Feat.GreaterSpellFocus, "+1 bonus on DC for saving throws against all your spells." },
		{ Feat.Toughness, "+3 hit points." },
		{ Feat.TwoWeaponFighting, "Allows you to fight with two light weapons, at a -2 To Hit penalty." },
		{ Feat.TwoWeaponDefense, "Your off-hand weapon grants +1 bonus to Armor Class." },
		{ Feat.WeaponFinesse, "Use your Dexterity modifier on To Hit roll instead of Strength with light weapons." },
		{ Feat.WeaponFocus, "+1 To Hit bonus with weapon." },
		{ Feat.WeaponSpecialization, "+2 Damage bonus with weapon." },
	};

	public static bool isEligible (Feat feat, CharacterSheet sheet) {
		switch (feat) {
		case Feat.Dodge:
			if (sheet.dexterity >= 13) {
				return true;
			} else {
				return false;
			}
		case Feat.ImprovedCritical:
			if (sheet.base_attack >= 8) {
				return true;
			} else {
				return false;
			}
		case Feat.DeflectArrows:
			if (sheet.feats.Contains (Feat.ImprovedUnarmedStrike)) {
				return true;
			} else {
				return false;
			}
		case Feat.PreciseShot:
			if (sheet.feats.Contains (Feat.PointBlankShot)) {
				return true;
			} else {
				return false;
			}
		case Feat.RapidShot:
			if (sheet.feats.Contains (Feat.PointBlankShot) && sheet.dexterity >= 13) {
				return true;
			} else {
				return false;
			}
		case Feat.PowerAttack:
			if (sheet.strength >= 13) {
				return true;
			} else {
				return false;
			}
		case Feat.Cleave:
			if (sheet.feats.Contains (Feat.PowerAttack)) {
				return true;
			} else {
				return false;
			}
		case Feat.GreatCleave:
			if (sheet.feats.Contains (Feat.Cleave) && sheet.base_attack >= 4) {
				return true;
			} else {
				return false;
			}
		case Feat.TwoWeaponFighting:
			if (sheet.dexterity >= 15) {
				return true;
			} else {
				return false;
			}
		case Feat.TwoWeaponDefense:
			if (sheet.feats.Contains (Feat.TwoWeaponFighting)) {
				return true;
			} else {
				return false;
			}
		case Feat.WeaponFinesse:
			if (sheet.base_attack >= 1) {
				return true;
			} else {
				return false;
			}
		case Feat.WeaponFocus:
			if (sheet.base_attack >= 1) {
				return true;
			} else {
				return false;
			}
		case Feat.WeaponSpecialization:
			if (sheet.feats.Contains (Feat.WeaponFocus) && sheet.character_class == Helpers.CharacterClass.Fighter && sheet.level >= 4) {
				return true;
			} else {
				return false;
			}
		default:
			return true;
		}
	}

	public static List<Feat> allEligible (CharacterSheet sheet) {
		List<Feat> eligibleFeats = new List<Feat> ();
		foreach (Feat feat in System.Enum.GetValues(typeof(Feat))) {
			if (isEligible(feat, sheet) && !notImplemented.Contains(feat)) {
				eligibleFeats.Add(feat);
			}
		}

		return eligibleFeats;
	}

	public static List<string> allEligibleString (CharacterSheet sheet) {
		List<Feat> eligibleFeats = allEligible (sheet);
		List<string> strings = eligibleFeats.Select (x => featToString [x]) as List<string>;
		return strings;
	}

	public static List<string> featsToString (List<Feat> feats) {
		List<string> strings = new List<string> ();
		foreach (Feat feat in feats) {
			strings.Add (featToString [feat]);
		}
		return strings;
	}
}
