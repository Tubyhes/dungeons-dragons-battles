using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Helpers {

	/**
	 * Helpers for rolling dice
	 */
	public struct DiceRoll {
		public int numDice;
		public int sizeDice;
	}

	public static int RollDice (DiceRoll roll) {
		int total = 0;
		for (int i = 0; i < roll.numDice; i++) {
			total += Random.Range (1, roll.sizeDice + 1);
		}
		return total;
	}

	public static int RollD20 () {
		return Random.Range (1, 21);
	}

	/**
	 * Terrain and geographics helpers
	 */
	public enum GroundTypes {
		Grassland,
		Dirt,
		Dungeon
	}

	public enum Direction {
		Horizontal,
		Up,
		Down,
		Left,
		Right
	}

	public static Direction OppositeDirection (Direction dir) {
		if (dir == Direction.Up) {
			return Direction.Down;
		} else if (dir == Direction.Down) {
			return Direction.Up;
		} else if (dir == Direction.Left) {
			return Direction.Right;
		} else if (dir == Direction.Right) {
			return Direction.Left;
		} else {
			return Direction.Horizontal;
		}
	}

	public static float EditingDistance (Vector3 a, Vector3 b) {
		return Mathf.Abs (a.x - b.x) + Mathf.Abs (a.y - b.y);
	}

	/**
	 * Combat helpers
	 */
	public enum CombatState {
		Alive,
		Down,
		Dead
	}

	public enum CombatAction {
		Attack,
		FullDefense,
		CastSpell,
		UseItem,
		EndTurn,
		Cancel
	}

	public enum Teams {
		Home,
		Away
	}

	public static Helpers.Teams otherTeam (Helpers.Teams thisTeam) {
		return thisTeam == Helpers.Teams.Home ? Helpers.Teams.Away : Helpers.Teams.Home;
	}

	/**
	 * Ruleset helpers
	 */
	public enum CharacterClass {
		Fighter,
		Cleric,
		Rogue,
		Wizard,
		Monster
	}

	public enum CharacterSize {
		Tiny = 2,
		Small = 1,
		Medium = 0,
		Large = -1,
		Huge = -2
	}

	public enum DamageType {
		Physical,
		Magic,
		Fire
	}

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

	/**
	 * Miscellaneous
	 */
	public class DescComparer<T> : IComparer<T> {
		public int Compare (T x, T y) {
			return Comparer<T>.Default.Compare (y, x);
		}
	}

	/**
	 * Input axes helpers
	 */
	public static string Horizontal (Helpers.Teams team) {
		return team == Helpers.Teams.Home ? "Horizontal_P1" : "Horizontal_P2";
	}

	public static string Vertical (Helpers.Teams team) {
		return team == Helpers.Teams.Home ? "Vertical_P1" : "Vertical_P2";
	}

	public static string Fire1 (Helpers.Teams team) {
		return team == Helpers.Teams.Home ? "Fire1_P1" : "Fire1_P2";
	}

	public static string Fire2 (Helpers.Teams team) {
		return team == Helpers.Teams.Home ? "Fire2_P1" : "Fire2_P2";
	}

	public static Dictionary<CharacterClass, Sprite> classPortraits = new Dictionary<CharacterClass, Sprite> {
		{ CharacterClass.Fighter, Resources.Load<Sprite> ("varian_wrynn") },
		{ CharacterClass.Cleric, Resources.Load<Sprite> ("anduinn") },
		{ CharacterClass.Rogue, Resources.Load<Sprite> ("edwin_van_cleef") },
		{ CharacterClass.Wizard, Resources.Load<Sprite> ("antonidas") },
	};

	public static Dictionary<string, CharacterClass> stringToClass = new Dictionary<string, CharacterClass> {
		{ "fighter", CharacterClass.Fighter },
		{ "cleric", CharacterClass.Cleric },
		{ "rogue", CharacterClass.Rogue },
		{ "wizard", CharacterClass.Wizard },
	};

	public static Dictionary<CharacterClass, string> classToString = new Dictionary<CharacterClass, string> {
		{ CharacterClass.Fighter, "fighter" },
		{ CharacterClass.Cleric, "cleric" },
		{ CharacterClass.Rogue, "rogue" },
		{ CharacterClass.Wizard, "wizard" },
	};

}
