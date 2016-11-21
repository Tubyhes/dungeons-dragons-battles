using UnityEngine;
using System.Collections;

public class Armor {

	private int armor_bonus;
	private int max_dexterity;
	private int max_speed;
	private string armor_tag;

	public int ArmorBonus {
		get {
			return armor_bonus;
		}
	}
	public int MaxDexterity {
		get {
			return max_dexterity;
		}
	}
	public int MaxSpeed {
		get {
			return max_speed;
		}
	}
	public string ArmorTag {
		get {
			return armor_tag;
		}
	}

	public static Armor LeatherArmor () {
		Armor armor = new Armor ();
		armor.armor_bonus = 2;
		armor.max_dexterity = 6;
		armor.max_speed = 4;
		armor.armor_tag = "Leather Armor";
		return armor;
	}

	public static Armor StuddedLeather () {
		Armor armor = new Armor ();
		armor.armor_bonus = 3;
		armor.max_dexterity = 5;
		armor.max_speed = 4;
		armor.armor_tag = "Studded Leather Armor";
		return armor;
	}

	public static Armor ChainShirt () {
		Armor armor = new Armor ();
		armor.armor_bonus = 4;
		armor.max_dexterity = 4;
		armor.max_speed = 4;
		armor.armor_tag = "Chain Shirt";
		return armor;
	}

	public static Armor Breastplate () {
		Armor armor = new Armor ();
		armor.armor_bonus = 5;
		armor.max_dexterity = 3;
		armor.max_speed = 3;
		armor.armor_tag = "Breastplate";
		return armor;
	}

	public static Armor LightShield () {
		Armor armor = new Armor ();
		armor.armor_bonus = 1;
		armor.max_dexterity = int.MaxValue;
		armor.max_speed = int.MaxValue;
		armor.armor_tag = "Light Shield";
		return armor;
	}

	public static Armor HeavyShield () {
		Armor armor = new Armor ();
		armor.armor_bonus = 2;
		armor.max_dexterity = int.MaxValue;
		armor.max_speed = int.MaxValue;
		armor.armor_tag = "Heavy Shield";
		return armor;
	}

	public static Armor TowerShield () {
		Armor armor = new Armor ();
		armor.armor_bonus = 4;
		armor.max_dexterity = 2;
		armor.max_speed = int.MaxValue;
		armor.armor_tag = "Tower Shield";
		return armor;
	}

	public static Armor None () {
		Armor armor = new Armor ();
		armor.armor_bonus = 0;
		armor.max_dexterity = int.MaxValue;
		armor.max_speed = int.MaxValue;
		armor.armor_tag = "None";
		return armor;
	}
}
