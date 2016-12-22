using UnityEngine;
using System.Collections;

public class CauseFear : Spell {

	public CauseFear () {
		spellName = "Cause Fear";
		description = "Frightens target for 1d4 rounds (-2 on to hit and saves, 50% chance to lose turn).";
		icon = "cause_fear";
		range = 4;
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
		int spell_dc = caster.GetSpellDC () + spellLvl;
		int will_save = Helpers.RollD20 () + target.GetWill ();
		if (target.effects.ContainsKey (CombatEffects.CombatEffect.Blessed)) {
			will_save += 1;
		}

		if (will_save >= spell_dc) {
			target.AddCombatEffect (CombatEffects.CombatEffect.Shaken, 2);
			return "Successful Will Save, target is Shaken for 1 round.";
		} else {
			int duration = Helpers.RollDice (new Helpers.DiceRoll { numDice = 1, sizeDice = 4 });
			target.AddCombatEffect (CombatEffects.CombatEffect.Frightened, duration + 1);
			return "Target is frightened for " + duration + " rounds.";
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
		return new CauseFear ();
	}
}
