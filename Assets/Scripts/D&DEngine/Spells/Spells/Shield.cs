using UnityEngine;
using System.Collections;

public class Shield : Spell {

	public Shield () {
		spellName = "Shield";
		description = "+4 AC forever, replaces shield armor bonus.";
		icon = "shield";
		range = 1;
		aoe = 1;
		casterLvl = 1;
		spellLvl = 1;
		characterClasses = new [] { Helpers.CharacterClass.Wizard };
	}

	public override string DealDamage (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string DoHealing (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string ApplyEffect (CharacterSheet caster, CharacterSheet target) {
		target.AddCombatEffect (CombatEffects.CombatEffect.Shield, 100);
		return "Shield applies +4 AC, replacing shield armor bonus.";
	}

	public override Coroutine PlayAnimation (Vector3 origin, Vector3 target) {
		SoundManager soundManager = GameObject.FindObjectOfType (typeof(SoundManager)) as SoundManager;
		soundManager.PlayShield ();

		GameObject shield = GameObject.Instantiate (Resources.Load ("Shield"), target, Quaternion.identity) as GameObject;
		ShieldFX fx = shield.GetComponent<ShieldFX> ();
		return fx.StartFXGreen ();
	}

	public override Spell Duplicate () {
		return new Shield ();
	}
}
