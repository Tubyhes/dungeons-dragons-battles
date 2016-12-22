using UnityEngine;
using System.Collections;

public class Doom : Spell {

	public Doom () {
		spellName = "Doom";
		description = "Causes target to be shaken (-2 on to hit and saves) for 10 rounds.";
		icon = "doom";
		range = 8;
		aoe = 1;
		casterLvl = 1;
		spellLvl = 1;
		characterClasses = new [] { Helpers.CharacterClass.Cleric};
	}

	public override string DealDamage (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string DoHealing (CharacterSheet caster, CharacterSheet target) {
		return "";
	}

	public override string ApplyEffect (CharacterSheet caster, CharacterSheet target) {
		int spell_dc = caster.GetSpellDC () + spellLvl;
		int will_save = Helpers.RollD20 () + target.GetWill ();
		if (target.effects.ContainsKey (CombatEffects.CombatEffect.Blessed)) {
			will_save += 1;
		}

		if (will_save >= spell_dc) {
			return "Successful Will Save, spell effect is negated.";
		} else {
			target.AddCombatEffect (CombatEffects.CombatEffect.Shaken, 10);
			return "Target is shaken for 10 rounds.";
		}
	}

	public override Coroutine PlayAnimation (Vector3 origin, Vector3 target) {
		SoundManager soundManager = GameObject.FindObjectOfType (typeof(SoundManager)) as SoundManager;
		soundManager.PlayFear ();

		GameObject causeFear = GameObject.Instantiate (Resources.Load ("Null"), target, Quaternion.identity) as GameObject;
		NullFX fx = causeFear.GetComponent<NullFX> ();
		return fx.StartFX ();
	}

	public override Spell Duplicate () {
		return new Doom ();
	}
}
