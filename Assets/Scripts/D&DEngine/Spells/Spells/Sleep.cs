using UnityEngine;
using System.Collections;

public class Sleep : Spell {

	public Sleep () {
		spellName = "Sleep";
		description = "Causes target to be helpless for 10 rounds. Damage or help from ally dispels.";
		icon = "sleep";
		range = 8;
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
		int spell_dc = caster.GetSpellDC () + spellLvl;
		int will_save = Helpers.RollD20 () + target.GetWill ();

		if (will_save >= spell_dc) {
			return "Successful Will Save, spell effect is negated.";
		} else {
			target.AddCombatEffect (CombatEffects.CombatEffect.Helpless, 10);
			return "Target is helpless for 10 rounds.";
		}
	}

	public override Coroutine PlayAnimation (Vector3 origin, Vector3 target) {
		SoundManager soundManager = GameObject.FindObjectOfType (typeof(SoundManager)) as SoundManager;
		soundManager.PlayFear ();

		GameObject sleep = GameObject.Instantiate (Resources.Load ("Null"), target, Quaternion.identity) as GameObject;
		NullFX fx = sleep.GetComponent<NullFX> ();
		return fx.StartFX ();
	}

	public override Spell Duplicate () {
		return new Sleep ();
	}
}
