using UnityEngine;
using System.Collections;

public class Bless : Spell {

	public Bless () {
		spellName = "Bless";
		description = "+1 on attack roles, saves against fear.";
		icon = "bless";
		range = 1;
		aoe = 1;
		casterLvl = 1;
		spellLvl = 1;
		characterClasses = new [] { Helpers.CharacterClass.Cleric };
	}

	public override string DealDamage (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string DoHealing (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string ApplyEffect (CharacterSheet caster, CharacterSheet target) {
		target.AddCombatEffect (CombatEffects.CombatEffect.Blessed, 100);
		return "Bless adds +1 to attack roles and saves against fear.";
	}

	public override Coroutine PlayAnimation (Vector3 origin, Vector3 target) {
		SoundManager soundManager = GameObject.FindObjectOfType (typeof(SoundManager)) as SoundManager;
		soundManager.PlayBless ();

		GameObject bless = GameObject.Instantiate (Resources.Load ("Null"), target, Quaternion.identity) as GameObject;
		NullFX fx = bless.GetComponent<NullFX> ();
		return fx.StartFX ();
	}

	public override Spell Duplicate () {
		return new Bless ();
	}
}
