using UnityEngine;
using System.Collections;

public abstract class Spell {

	public string spellName;
	public string description;
	public string icon;
	public int range;
	public int aoe;
	public int casterLvl;
	public Helpers.CharacterClass [] characterClasses;

	public abstract string DealDamage (CharacterSheet caster, CharacterSheet target);
	public abstract string DoHealing (CharacterSheet caster, CharacterSheet target);
	public abstract string ApplyEffect (CharacterSheet caster, CharacterSheet target);
	public abstract Coroutine PlayAnimation (Vector3 origin, Vector3 target);

	public abstract Spell Duplicate ();
}
