using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Weapon : Equipment {

	public enum WeaponType {
		Simple,
		Martial
	}
	public enum WeaponSize {
		Light,
		OneHanded,
		TwoHanded
	}

	// weapon stats
	public Helpers.DiceRoll damage_roll { get; private set; }
	public WeaponType weapon_type  { get; private set; }
	public WeaponSize weapon_size  { get; private set; }
	public int damage_bonus  { get; private set; }
	public int attack_bonus  { get; private set; }
	public int crit_range  { get; private set; }
	public int crit_modifier  { get; private set; }

	/**
	 * Simple Weapons
	 */
	// Light weapons
	public static Weapon Dagger () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Simple;
		weapon.weapon_size = WeaponSize.Light;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 4 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 2;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Dagger";
		return weapon;
	}
	public static Weapon LightMace () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Simple;
		weapon.weapon_size = WeaponSize.Light;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 6 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Light Mace";
		return weapon;
	}
	// One Handed Weapons
	public static Weapon Club () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Simple;
		weapon.weapon_size = WeaponSize.OneHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 6 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Club";
		return weapon;
	}
	public static Weapon HeavyMace () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Simple;
		weapon.weapon_size = WeaponSize.OneHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 8 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Heavy Mace";
		return weapon;
	}
	// Two Handed weapons
	public static Weapon Quarterstaff () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Simple;
		weapon.weapon_size = WeaponSize.TwoHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 8 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Quarterstaff";
		return weapon;
	}
	public static Weapon Spear () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Simple;
		weapon.weapon_size = WeaponSize.TwoHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 8 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 3;
		weapon.equipment_tag = "Spear";
		return weapon;
	}

	/**
	 * Martial Weapons
	 */
	// Light weapons
	public static Weapon ShortSword () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.Light;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 6 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 2;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Short Sword";
		return weapon;
	}
	public static Weapon HandAxe () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.Light;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 6 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 3;
		weapon.equipment_tag = "Hand Axe";
		return weapon;
	}
	public static Weapon LightPick () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.Light;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 4 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 4;
		weapon.equipment_tag = "Light Pick";
		return weapon;
	}
	// One Handed Weapons
	public static Weapon BattleAxe () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.OneHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 8 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 3;
		weapon.equipment_tag = "Battle Axe";
		return weapon;
	}
	public static Weapon LongSword () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.OneHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 8 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 2;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Long Sword";
		return weapon;
	}
	public static Weapon HeavyPick () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.OneHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 6 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 4;
		weapon.equipment_tag = "Heavy Pick";
		return weapon;
	}
	public static Weapon Scimitar () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.OneHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 6 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 3;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Scimitar";
		return weapon;
	}
	public static Weapon Warhammer () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.OneHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 8 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 3;
		weapon.equipment_tag = "Warhammer";
		return weapon;
	}
	// Tho Handed Weapons
	public static Weapon GreatAxe () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.TwoHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 12 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 3;
		weapon.equipment_tag = "Great Axe";
		return weapon;
	}
	public static Weapon GreatSword () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.TwoHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 2, sizeDice = 6 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 2;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Great Sword";
		return weapon;
	}
	public static Weapon GreatHammer () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.TwoHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 12 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 3;
		weapon.equipment_tag = "Great Hammer";
		return weapon;
	}
	public static Weapon Falchion () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.TwoHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 2, sizeDice = 4 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 3;
		weapon.crit_modifier = 2;
		weapon.equipment_tag = "Falchion";
		return weapon;
	}
	public static Weapon Scythe () {
		Weapon weapon = new Weapon ();
		weapon.weapon_type = WeaponType.Martial;
		weapon.weapon_size = WeaponSize.TwoHanded;
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 2, sizeDice = 4 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 4;
		weapon.equipment_tag = "Scythe";
		return weapon;
	}

	public static List<Weapon> weapons = new List<Weapon> () {
		// simple weapons
		Dagger (),
		LightMace (),
		Club (),
		HeavyMace (),
		Quarterstaff (),
		Spear (),

		// martial weapons
		ShortSword (),
		HandAxe (),
		LightPick (),
		BattleAxe (),
		LongSword (),
		HeavyPick (),
		Scimitar (),
		Warhammer (),
		GreatAxe (),
		GreatSword (),
		GreatHammer (),
		Falchion (),
		Scythe (),
	};

	public static List<Weapon> eligibleMainHandWeapons (Helpers.CharacterClass character_class) {
		if (character_class == Helpers.CharacterClass.Fighter) {
			return weapons;
		}
		if (character_class == Helpers.CharacterClass.Cleric) {
			return weapons.FindAll (x => x.weapon_type == WeaponType.Simple);
		}
		if (character_class == Helpers.CharacterClass.Rogue) {
			return weapons.FindAll(x => x.weapon_type == WeaponType.Simple || x.weapon_size == WeaponSize.Light);
		}
		if (character_class == Helpers.CharacterClass.Wizard) {
			return new List<Weapon> () {
				Dagger (),
				Club (),
				Quarterstaff (),
			};
		}

		return new List<Weapon> ();
	}

	public static List<Weapon> eligibleOffHandWeapons (Helpers.CharacterClass character_class) {
		return eligibleMainHandWeapons (character_class).FindAll (x => x.weapon_size == WeaponSize.Light);
	}
}
