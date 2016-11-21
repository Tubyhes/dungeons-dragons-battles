using UnityEngine;
using System.Collections;

public class Weapon {

	// weapon stats
	private Helpers.DiceRoll damage_roll;
	private int damage_bonus;
	private int attack_bonus;
	private int crit_range;
	private int crit_modifier;
	private string weapon_tag;

	// public getters for weapon stats
	public Helpers.DiceRoll DamageRoll {
		get {
			return damage_roll;
		}
	}
	public int DamageBonus {
		get {
			return damage_bonus;
		}
	}
	public int AttackBonus {
		get {
			return attack_bonus;
		}
	}
	public int CritRange {
		get {
			return crit_range;
		}
	}
	public int CritModifier {
		get {
			return crit_modifier;
		}
	}
	public string WeaponTag {
		get {
			return weapon_tag;
		}
	}

	public static Weapon ShortSword () {
		Weapon weapon = new Weapon ();
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 6 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 2;
		weapon.crit_modifier = 2;
		weapon.weapon_tag = "Short Sword";
		return weapon;
	}

	public static Weapon LongSword () {
		Weapon weapon = new Weapon ();
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 8 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 2;
		weapon.crit_modifier = 2;
		weapon.weapon_tag = "Long Sword";
		return weapon;
	}

	public static Weapon GreatSword () {
		Weapon weapon = new Weapon ();
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 2, sizeDice = 6 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 2;
		weapon.crit_modifier = 2;
		weapon.weapon_tag = "Great Sword";
		return weapon;
	}

	public static Weapon Bite () {
		Weapon weapon = new Weapon ();
		weapon.damage_roll = new Helpers.DiceRoll{ numDice = 1, sizeDice = 4 };
		weapon.damage_bonus = 0;
		weapon.attack_bonus = 0;
		weapon.crit_range = 1;
		weapon.crit_modifier = 2;
		weapon.weapon_tag = "Bite";
		return weapon;
	}
}
