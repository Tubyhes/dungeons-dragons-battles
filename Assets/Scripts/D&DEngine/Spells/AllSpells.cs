using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class AllSpells  {

	public static List<Spell> spells = new List<Spell> () {
		new BurningHands (),
		new CureLightWounds(),
		new MagicMissile (),
	};

	public static List<Spell> EligibleSpells (CharacterSheet sheet) {
		List<Spell> eligibleSpells = spells.FindAll (x => x.casterLvl <= sheet.Level && x.characterClasses.Contains(sheet.character_class));
		return eligibleSpells;
	}
}
