using UnityEngine;
using System.Collections;

public class MagicMissile : Spell {

	public MagicMissile () {
		spellName = "Magic Missile";
		description = "1d4+1 damage; +1 missile per two levels above 1st (max 5).";
		icon = "magic_missile";
		range = 8;
		aoe = 1;
		casterLvl = 1;
		spellLvl = 1;
		characterClasses = new [] { Helpers.CharacterClass.Wizard };
	}

	public override string DealDamage (CharacterSheet caster, CharacterSheet target) {
		int numMissiles = (caster.level + 1) / 2;
		int dmg = Helpers.RollDice (new Helpers.DiceRoll () { numDice = numMissiles, sizeDice = 4 }) + numMissiles;
		target.DealDamage (dmg, Helpers.DamageType.Magic);
		return "Magic Missile deals " + dmg + " damage.";
	}

	public override string DoHealing (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string ApplyEffect (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override Coroutine PlayAnimation (Vector3 origin, Vector3 target) {
		SoundManager soundManager = GameObject.FindObjectOfType (typeof(SoundManager)) as SoundManager;
		soundManager.PlayMagicMissile ();

		GameObject missile = GameObject.Instantiate (Resources.Load ("MagicMissile"), origin, Quaternion.identity) as GameObject;
		MagicMissileFX fx = missile.GetComponent<MagicMissileFX> ();
		return fx.SetTarget (target);
	}

	public override Spell Duplicate () {
		return new MagicMissile ();
	}

}
