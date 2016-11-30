using UnityEngine;
using System.Collections;

public class CureLightWounds : Spell {

	public CureLightWounds () {
		spellName = "Cure Light Wounds";
		description = "Cures 1d8 damage +1/level (max 5).";
		icon = "cure_light_wounds";
		range = 1;
		aoe = 1;
		casterLvl = 1;
		characterClasses = new [] { Helpers.CharacterClass.Cleric };
	}

	protected Helpers.DiceRoll roll = new Helpers.DiceRoll() {numDice = 1, sizeDice = 8};
	protected int maxLevelBonus = 5;

	public override string DealDamage (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string DoHealing (CharacterSheet caster, CharacterSheet target) {
		int healing = Helpers.RollDice (roll) + Mathf.Max (caster.Level, maxLevelBonus);
		target.DoHealing (healing);
		return "Cure Light Wounds heals for " + healing + ".";
	}

	public override string ApplyEffect (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override Coroutine PlayAnimation (Vector3 origin, Vector3 target) {
		SoundManager soundManager = GameObject.FindObjectOfType (typeof(SoundManager)) as SoundManager;
		soundManager.PlayHeal ();

		GameObject heal = GameObject.Instantiate (Resources.Load ("CureWounds"), target, Quaternion.identity) as GameObject;
		HealFX fx = heal.GetComponent<HealFX> ();
		return fx.StartFX ();
	}

	public override Spell Duplicate () {
		return new CureLightWounds ();
	}
}
