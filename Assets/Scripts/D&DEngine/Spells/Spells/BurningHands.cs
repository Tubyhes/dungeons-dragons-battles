using UnityEngine;
using System.Collections;

public class BurningHands : Spell {

	public BurningHands () {
		spellName = "Burning Hands";
		description = "1d4/level fire damage (max 5d4), in small area.";
		icon = "burning_hands";
		range = 2;
		aoe = 2;
		casterLvl = 1;
		spellLvl = 1;
		characterClasses = new [] { Helpers.CharacterClass.Wizard };
	}

	public override string DealDamage (CharacterSheet caster, CharacterSheet target) {
		int x = Mathf.Min (caster.level, 5);
		int dmg = Helpers.RollDice (new Helpers.DiceRoll () { numDice = x, sizeDice = 4 });
		target.DealDamage (dmg, Helpers.DamageType.Fire);
		return "Burning Hands deals " + dmg + " damage.";
	}

	public override string DoHealing (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string ApplyEffect (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override Coroutine PlayAnimation (Vector3 origin, Vector3 target) {
		SoundManager soundManager = GameObject.FindObjectOfType (typeof(SoundManager)) as SoundManager;
		soundManager.PlayFire ();

		GameObject fire = GameObject.Instantiate (Resources.Load ("Fire"), target, Quaternion.identity) as GameObject;
		FireFX fx = fire.GetComponent<FireFX> ();
		return fx.StartFX ();
	}

	public override Spell Duplicate () {
		return new BurningHands ();
	}
}
