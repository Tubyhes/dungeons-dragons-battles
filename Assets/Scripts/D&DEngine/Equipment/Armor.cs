using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Armor : Equipment {

	public enum ArmorSize
	{
		Light,
		Medium,
		Heavy
	}

	public ArmorSize armor_size { get; private set; }
	public int armor_bonus { get; private set; }
	public int max_dexterity { get; private set; }
	public int max_speed { get; private set; }

	public static Armor LeatherArmor () {
		Armor armor = new Armor ();
		armor.armor_size = ArmorSize.Light;
		armor.armor_bonus = 2;
		armor.max_dexterity = 4;
		armor.max_speed = 4;
		armor.equipment_tag = "Leather Armor";
		return armor;
	}

	public static Armor StuddedLeather () {
		Armor armor = new Armor ();
		armor.armor_size = ArmorSize.Light;
		armor.armor_bonus = 3;
		armor.max_dexterity = 3;
		armor.max_speed = 4;
		armor.equipment_tag = "Studded Leather";
		return armor;
	}

	public static Armor ChainMail () {
		Armor armor = new Armor ();
		armor.armor_size = ArmorSize.Medium;
		armor.armor_bonus = 4;
		armor.max_dexterity = 2;
		armor.max_speed = 3;
		armor.equipment_tag = "Chain Mail";
		return armor;
	}

	public static Armor Breastplate () {
		Armor armor = new Armor ();
		armor.armor_size = ArmorSize.Medium;
		armor.armor_bonus = 5;
		armor.max_dexterity = 1;
		armor.max_speed = 3;
		armor.equipment_tag = "Breastplate";
		return armor;
	}

	public static Armor FullPlate () {
		Armor armor = new Armor ();
		armor.armor_size = ArmorSize.Heavy;
		armor.armor_bonus = 6;
		armor.max_dexterity = 0;
		armor.max_speed = 2;
		armor.equipment_tag = "Full Plate";
		return armor;
	}


	public static Armor LightShield () {
		Armor armor = new Armor ();
		armor.armor_size = ArmorSize.Light;
		armor.armor_bonus = 1;
		armor.max_dexterity = 6;
		armor.max_speed = 4;
		armor.equipment_tag = "Light Shield";
		return armor;
	}

	public static Armor HeavyShield () {
		Armor armor = new Armor ();
		armor.armor_size = ArmorSize.Medium;
		armor.armor_bonus = 2;
		armor.max_dexterity = 6;
		armor.max_speed = 3;
		armor.equipment_tag = "Heavy Shield";
		return armor;
	}

	public static Armor TowerShield () {
		Armor armor = new Armor ();
		armor.armor_size = ArmorSize.Heavy;
		armor.armor_bonus = 4;
		armor.max_dexterity = 2;
		armor.max_speed = 2;
		armor.equipment_tag = "Tower Shield";
		return armor;
	}

	public static List<Armor> chestArmor = new List<Armor> () {
		LeatherArmor (),
		StuddedLeather (),
		ChainMail (),
		Breastplate (),
		FullPlate (),
	};

	public static List<Armor> shields = new List<Armor> () {
		LightShield (),
		HeavyShield (),
		TowerShield (),
	};

	public static List<Armor> eligibleChestArmor (Helpers.CharacterClass character_class) {
		if (character_class == Helpers.CharacterClass.Fighter || 
			character_class == Helpers.CharacterClass.Cleric) {
			return chestArmor;
		}
		if (false) {
			return chestArmor.FindAll (x => x.armor_size != ArmorSize.Heavy);
		}
		if (character_class == Helpers.CharacterClass.Wizard ||
			character_class == Helpers.CharacterClass.Rogue) {
			return chestArmor.FindAll (x => x.armor_size == ArmorSize.Light);
		}

		return new List<Armor> ();
	}

	public static List<Armor> eligibleShields (Helpers.CharacterClass character_class) {
		if (character_class == Helpers.CharacterClass.Fighter) {
			return shields;
		}
		if (character_class == Helpers.CharacterClass.Cleric) {
			return shields.FindAll (x => x.armor_size != ArmorSize.Heavy);
		}
		if (false) {
			return shields.FindAll (x => x.armor_size == ArmorSize.Light);
		}

		return new List<Armor> ();
	}

}
