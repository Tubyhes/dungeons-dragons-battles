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

	/**
	 * Ruleset helpers
	 */
	public enum CharacterClass {
		Fighter,
		Cleric,
		Rogue,
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

	/**
	 * Miscellaneous
	 */
	public class DescComparer<T> : IComparer<T> {
		public int Compare (T x, T y) {
			return Comparer<T>.Default.Compare (y, x);
		}
	}

}
