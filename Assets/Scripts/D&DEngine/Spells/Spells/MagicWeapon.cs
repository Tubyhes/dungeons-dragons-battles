using UnityEngine;
using System.Collections;

public class MagicWeapon : Spell {

	public MagicWeapon () {
		spellName = "Magic Weapon";
		description = "Give +1 on to hit and damage for 10 rounds.";
		icon = "magic_weapon";
		range = 1;
		aoe = 1;
		casterLvl = 1;
		spellLvl = 1;
		characterClasses = new [] { Helpers.CharacterClass.Cleric, Helpers.CharacterClass.Wizard};
	}

	public override string DealDamage (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string DoHealing (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string ApplyEffect (CharacterSheet caster, CharacterSheet target) {
		target.AddCombatEffect (CombatEffects.CombatEffect.MagicWeapon, 10);
		return "Target gains +1 on to hit and damage for 10 rounds";
	}

	public override Coroutine PlayAnimation (Vector3 origin, Vector3 target) {
		SoundManager soundManager = GameObject.FindObjectOfType (typeof(SoundManager)) as SoundManager;
		soundManager.PlayBless ();

		GameObject magicWeapon = GameObject.Instantiate (Resources.Load ("Null"), target, Quaternion.identity) as GameObject;
		NullFX fx = magicWeapon.GetComponent<NullFX> ();
		return fx.StartFX ();
	}

	public override Spell Duplicate () {
		return new MagicWeapon ();
	}
}
