using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSheet {

	// reference to combat log
	public CombatLogController log;

	// personalisation
	public string character_name;

	// generic stuff
	public int level;
	public Helpers.CharacterClass character_class;
	public Helpers.CharacterSize character_size;

	// primary abilities
	public int strength;
	public int dexterity;
	public int constitution;
	public int intelligence;
	public int wisdom;
	public int charisma;

	// life
	public int max_hitpoints;
	public int hit_die;

	// combat
	public int base_attack;
	public int speed;
	public int num_attacks;
	public int natural_armor;

	// spell casting
	public int num_spells;
	public int concentration;

	// saves
	public int fortitude;
	public int reflex;
	public int will;

	// highly dynamic stats
	// TODO: cant assign self in setter, will cause endless loop.
	private int _hitpoints;
	public int hitpoints {
		get {
			return _hitpoints;
		} 
		set {
			_hitpoints = value;
			if (hitpointsUpdatedDelegate != null) {
				hitpointsUpdatedDelegate (_hitpoints, max_hitpoints);
			}
		}
	}
	private int _moves_left;
	public int moves_left {
		get {
			return _moves_left;
		}
		set {
			_moves_left = value;
			if (movesLeftUpdatedDelegate != null) {
				movesLeftUpdatedDelegate (_moves_left, GetSpeed());
			}
		}
	}
	private int _attacks_left;
	public int attacks_left {
		get {
			return _attacks_left;
		}
		set {
			_attacks_left = value;
			if (attacksLeftUpdatedDelegate != null) {
				attacksLeftUpdatedDelegate (_attacks_left, num_attacks);
			}
		}
	}
	private int _spells_left;
	public int spells_left {
		get {
			return _spells_left;
		}
		set {
			_spells_left = value;
			if (spellsLeftUpdatedDelegate != null) {
				spellsLeftUpdatedDelegate (_spells_left, num_spells);
			}
		}
	}

	// equipment
	public Weapon weapon_main = null;
	public Weapon weapon_off = null;
	public Armor armor = null; 
	public Armor shield = null;

	// feats
	public int num_feats;
	public List<Feats.Feat> feats;

	// temporary effects
	public Dictionary<CombatEffects.CombatEffect, int> effects;

	// delegates for updating UI if anything changes
	public delegate void HitpointsUpdatedDelegate (int hitpoints, int maxHitpoints);
	private HitpointsUpdatedDelegate hitpointsUpdatedDelegate;
	public void registerHitpointsDelegate (HitpointsUpdatedDelegate new_delegate) {
		hitpointsUpdatedDelegate += new_delegate;
		new_delegate (hitpoints, max_hitpoints);
	}
	public delegate void MovesLeftUpdatedDelegate (int movesLeft, int maxMoves);
	private MovesLeftUpdatedDelegate movesLeftUpdatedDelegate;
	public void registerMovesLeftDelegate (MovesLeftUpdatedDelegate new_delegate) {
		movesLeftUpdatedDelegate += new_delegate;
		new_delegate (moves_left, GetSpeed());
	}
	public delegate void AttacksLeftUpdatedDelegate (int attacksLeft, int maxAttacks);
	private AttacksLeftUpdatedDelegate attacksLeftUpdatedDelegate;
	public void registerAttacksLeftDelegate (AttacksLeftUpdatedDelegate new_delegate) {
		attacksLeftUpdatedDelegate += new_delegate;
		new_delegate(attacks_left, num_attacks);
	}
	public delegate void SpellsLeftUpdatedDelegate (int spellsLeft, int maxSpells);
	private SpellsLeftUpdatedDelegate spellsLeftUpdatedDelegate;
	public void registerSpellsLeftDelegate (SpellsLeftUpdatedDelegate new_delegate) {
		spellsLeftUpdatedDelegate += new_delegate;
		new_delegate(spells_left, num_spells);
	}
	public delegate void CombatStatusChangedDelegate ();
	public CombatStatusChangedDelegate combatStatusChangedDelegate;

	public CharacterSheet () {
		effects = new Dictionary<CombatEffects.CombatEffect, int> ();
		feats = new List<Feats.Feat> ();
		strength = 8;
		dexterity = 8;
		constitution = 8;
		intelligence = 8;
		wisdom = 8;
		charisma = 8;
		num_feats = 2;

	}

	public static CharacterSheet NewCharacterSheet (Helpers.CharacterClass character_class) {
		switch (character_class) {
		case Helpers.CharacterClass.Fighter:
			return Fighter ();
		case Helpers.CharacterClass.Cleric:
			return Cleric();
		case Helpers.CharacterClass.Rogue:
			return Rogue ();
		case Helpers.CharacterClass.Wizard:
			return Wizard ();
		default:
			return null;
		}
	}

	public static CharacterSheet Fighter () {
		CharacterSheet sheet = new CharacterSheet ();
		sheet.character_name =	"Barabas";
		sheet.level = 			1;
		sheet.character_size = 	Helpers.CharacterSize.Medium;
		sheet.character_class = Helpers.CharacterClass.Fighter;
		sheet.hit_die = 			10;
		sheet.base_attack = 		1;
		sheet.speed = 				4;
		sheet.num_attacks = 		1;
		sheet.natural_armor = 		0;
		sheet.fortitude = 			2;
		sheet.reflex = 				0;
		sheet.will = 				0;
		sheet.CalcMaxHitpoints ();
		sheet.hitpoints = sheet.max_hitpoints;
		sheet.moves_left = 0;
		sheet.attacks_left = 0;
		sheet.num_feats = 3;

		return sheet;
	}

	public static CharacterSheet FighterReady () {
		CharacterSheet sheet = Fighter ();
		sheet.strength = 16;
		sheet.dexterity = 14;
		sheet.constitution = 16;
		sheet.armor = Armor.Breastplate ();
		sheet.weapon_main = Weapon.GreatAxe ();
		sheet.CalcMaxHitpoints ();
		sheet.hitpoints = sheet.max_hitpoints;

		return sheet;
	}

	public static CharacterSheet Wizard () {
		CharacterSheet sheet = new CharacterSheet ();
		sheet.character_name =	"Piét";
		sheet.level = 			1;
		sheet.character_size = 	Helpers.CharacterSize.Medium;
		sheet.character_class = Helpers.CharacterClass.Wizard;
		sheet.hit_die = 			4;
		sheet.base_attack = 		0;
		sheet.speed = 				4;
		sheet.num_attacks = 		1;
		sheet.natural_armor = 		0;
		sheet.fortitude = 			0;
		sheet.reflex = 				0;
		sheet.will = 				2;
		sheet.CalcMaxHitpoints ();
		sheet.CalcMaxSpells ();
		sheet.hitpoints = sheet.max_hitpoints;
		sheet.moves_left = 0;
		sheet.attacks_left = 0;

		return sheet;
	}

	public static CharacterSheet WizardReady () {
		CharacterSheet sheet = Wizard ();
		sheet.dexterity = 14;
		sheet.constitution = 12;
		sheet.intelligence = 16;
		sheet.charisma = 12;
		sheet.armor = Armor.LeatherArmor ();
		sheet.weapon_main = Weapon.Quarterstaff ();
		sheet.CalcMaxHitpoints ();
		sheet.CalcMaxSpells ();
		sheet.hitpoints = sheet.max_hitpoints;
		return sheet;
	}

	public static CharacterSheet Cleric () {
		CharacterSheet sheet = new CharacterSheet ();
		sheet.character_name =	"Lorisc";
		sheet.level = 			1;
		sheet.character_size = 	Helpers.CharacterSize.Medium;
		sheet.character_class = Helpers.CharacterClass.Cleric;
		sheet.hit_die = 			8;
		sheet.base_attack = 		0;
		sheet.speed = 				4;
		sheet.num_attacks = 		1;
		sheet.natural_armor = 		0;
		sheet.fortitude = 			2;
		sheet.reflex = 				0;
		sheet.will = 				2;
		sheet.CalcMaxHitpoints ();
		sheet.CalcMaxSpells ();
		sheet.hitpoints = sheet.max_hitpoints;
		sheet.moves_left = 0;
		sheet.attacks_left = 0;

		return sheet;
	}

	public static CharacterSheet ClericReady () {
		CharacterSheet sheet = Cleric ();
		sheet.strength = 14;
		sheet.constitution = 14;
		sheet.wisdom = 16;
		sheet.charisma = 12;
		sheet.armor = Armor.Breastplate();
		sheet.weapon_main = Weapon.HeavyMace ();
		sheet.shield = Armor.HeavyShield ();
		sheet.CalcMaxHitpoints ();
		sheet.CalcMaxSpells ();
		sheet.hitpoints = sheet.max_hitpoints;
		return sheet;
	}

	public static CharacterSheet Rogue () {
		CharacterSheet sheet = new CharacterSheet ();
		sheet.character_name =	"Kalvin";
		sheet.level = 			1;
		sheet.character_size = 	Helpers.CharacterSize.Medium;
		sheet.character_class = Helpers.CharacterClass.Rogue;
		sheet.hit_die = 			6;
		sheet.base_attack = 		0;
		sheet.speed = 				4;
		sheet.num_attacks = 		1;
		sheet.natural_armor = 		0;
		sheet.fortitude = 			0;
		sheet.reflex = 				2;
		sheet.will = 				0;
		sheet.CalcMaxHitpoints ();
		sheet.hitpoints = sheet.max_hitpoints;
		sheet.moves_left = 0;
		sheet.attacks_left = 0;

		return sheet;
	}

	public static CharacterSheet RogueReady () {
		CharacterSheet sheet = Rogue ();
		sheet.strength = 10;
		sheet.dexterity = 18;
		sheet.constitution = 14;
		sheet.feats.Add (Feats.Feat.WeaponFinesse);
		sheet.feats.Add (Feats.Feat.TwoWeaponDefense);
		sheet.armor = Armor.LeatherArmor ();
		sheet.weapon_main = Weapon.ShortSword ();
		sheet.weapon_off = Weapon.Dagger ();
		sheet.CalcMaxHitpoints ();
		sheet.hitpoints = sheet.max_hitpoints;
		return sheet;
	}

	/**
	 * Setup methods
	 */
	public void CalcMaxHitpoints () {
		max_hitpoints = 0;
		if (character_class == Helpers.CharacterClass.Monster) {
			max_hitpoints += Helpers.RollDice (new Helpers.DiceRoll{ numDice = 1, sizeDice = hit_die });
			max_hitpoints += GetConstitutionModifier ();
		} else {
			max_hitpoints += hit_die;
			max_hitpoints += GetConstitutionModifier ();
		}

		for (int i = 1; i < level; i++) {
			max_hitpoints += Helpers.RollDice (new Helpers.DiceRoll{ numDice = 1, sizeDice = hit_die });
			max_hitpoints += GetConstitutionModifier ();
		}

		if (feats.Contains (Feats.Feat.Toughness)) {
			max_hitpoints += 3;
		}

		hitpoints = max_hitpoints;
	}
	public void CalcMaxSpells () {
		num_spells = 0;

		if (character_class == Helpers.CharacterClass.Cleric) {
			num_spells = 2 + GetWisdomModifier () + (level - 1);
		} 
		if (character_class == Helpers.CharacterClass.Wizard) {
			num_spells = 2 + GetIntelligenceModifier () + (level - 1);
		}

		spells_left = num_spells;
	}


	/**
	 * Ability modifiers
	 */
	private int AbilityModifier (int score) {
		return (int)Mathf.Floor ((score - 10) * 0.5f);
	}
	public int GetStrengthModifier () {
		// base strength
		int str = strength;

		// check if character is strength drained
		if (effects.ContainsKey (CombatEffects.CombatEffect.StrengthDrain)) {
			str -= 4;
		}

		return AbilityModifier (strength);
	}
	public int GetDexterityModifier () {
		// max dex modifiers from armor
		int armor_max_dex = armor == null ? 10 : armor.max_dexterity;
		int shield_max_dex = shield == null ? 10 : shield.max_dexterity;

		// base dexterity
		int dex = dexterity;

		// check if character is helpless
		if (effects.ContainsKey (CombatEffects.CombatEffect.Helpless)) {
			dex = 0;
		}

		return Mathf.Min (AbilityModifier (dex), armor_max_dex, shield_max_dex);	
	}
	public int GetConstitutionModifier () {
		return AbilityModifier (constitution);
	}
	public int GetIntelligenceModifier () {
		return AbilityModifier (intelligence);
	}
	public int GetWisdomModifier () {
		return AbilityModifier (wisdom);
	}
	public int GetCharsimaModifier () {
		return AbilityModifier (charisma);
	}

	/**
	 * Saves
	 */
	public int GetFortitude () {
		int x = fortitude + GetConstitutionModifier ();
		if (feats.Contains (Feats.Feat.GreatFortitude)) {
			x += 2;
		}
		// check for shaken / frightened
		if (effects.ContainsKey(CombatEffects.CombatEffect.Frightened) || effects.ContainsKey(CombatEffects.CombatEffect.Shaken)) {
			x -= 2;
		}
		return x;

	}
	public int GetReflex () {
		int x = reflex + GetDexterityModifier ();
		if (feats.Contains (Feats.Feat.LightningReflexes)) {
			x += 2;
		}
		// check for shaken / frightened
		if (effects.ContainsKey(CombatEffects.CombatEffect.Frightened) || effects.ContainsKey(CombatEffects.CombatEffect.Shaken)) {
			x -= 2;
		}
		return x;
	}
	public int GetWill () {
		int x = will + GetWisdomModifier ();
		if (feats.Contains (Feats.Feat.IronWill)) {
			x += 2;
		}
		// check for shaken / frightened
		if (effects.ContainsKey(CombatEffects.CombatEffect.Frightened) || effects.ContainsKey(CombatEffects.CombatEffect.Shaken)) {
			x -= 2;
		}
		return x;
	}

	/**
	 * Combat Scores
	 */
	public int GetToHitBonus (bool mainHand) {
		// base attack
		int to_hit = base_attack + (int)character_size;

		// two-weapon fighting incurs -2 penalty
		if (weapon_main != null && weapon_off != null) {
			to_hit -= 2;
		}

		// check for weapon finesse
		Weapon weapon = mainHand ? weapon_main : weapon_off;
		if (feats.Contains (Feats.Feat.WeaponFinesse) && weapon.weapon_size == Weapon.WeaponSize.Light) {
			to_hit += GetDexterityModifier ();
		} else {
			to_hit += GetStrengthModifier ();
		}

		// check for weapon focus
		if (feats.Contains (Feats.Feat.WeaponFocus)) {
			to_hit += 1;
		}

		// add any magical properties from weapon
		to_hit += mainHand ? weapon_main.attack_bonus : weapon_off.attack_bonus;

		// check for shaken / frightened
		if (effects.ContainsKey(CombatEffects.CombatEffect.Frightened) || effects.ContainsKey(CombatEffects.CombatEffect.Shaken)) {
			to_hit -= 2;
		}

		// check for TrueStrike
		if (effects.ContainsKey (CombatEffects.CombatEffect.TrueStrike)) {
			to_hit += 20;
			effects.Remove (CombatEffects.CombatEffect.TrueStrike);
		}

		// check for bless
		if (effects.ContainsKey (CombatEffects.CombatEffect.Blessed)) {
			to_hit += 1;
		}

		// check for magic weapon
		if (effects.ContainsKey (CombatEffects.CombatEffect.MagicWeapon)) {
			to_hit += 1;
		}

		return to_hit;
	}

	public int GetCritRange (bool mainHand) {
		int crit_range = mainHand ? weapon_main.crit_range : weapon_off.crit_range;

		// check for Improved Critical feat
		if (feats.Contains (Feats.Feat.ImprovedCritical)) {
			crit_range *= 2;
		}

		return crit_range;
	}

	public int GetAttackDamage (bool isFlanking, bool mainHand) {
		Weapon weapon = mainHand ? weapon_main : weapon_off;

		// base weapon damage
		int dmg = Helpers.RollDice (weapon.damage_roll) + weapon.damage_bonus;

		// strength based damage
		if (weapon.weapon_size == Weapon.WeaponSize.TwoHanded) {
			dmg += GetStrengthModifier () * 2;
		} else {
			dmg += GetStrengthModifier ();
		}

		// backstab damage for rogues
		if (character_class == Helpers.CharacterClass.Rogue && isFlanking) {
			dmg += Helpers.RollDice (new Helpers.DiceRoll{ numDice = (level + 1) / 2, sizeDice = 6 });
		}

		// check for weapon specialization
		if (feats.Contains (Feats.Feat.WeaponSpecialization)) {
			dmg += 2;
		}

		// check for magic weapon
		if (effects.ContainsKey (CombatEffects.CombatEffect.MagicWeapon)) {
			dmg += 1;
		}

		return dmg;
	}

	public int GetArmorClass () {
		// base armor class
		int ac = 10 + GetDexterityModifier () + natural_armor + (int)character_size;

		// add armor bonus from chest armor or mage armor spell
		if (effects.ContainsKey (CombatEffects.CombatEffect.MageArmor)) {
			ac += 4;
		} else if (armor != null) {
			ac += armor.armor_bonus;
		}

		// add armor bonus from shield or shield spell
		if (effects.ContainsKey (CombatEffects.CombatEffect.Shield)) {
			ac += 4;
		} else if (shield != null) {
			ac += shield.armor_bonus;
		}

		// check for Dodge and Two Weapon Fighting feats
		if (feats.Contains (Feats.Feat.Dodge)) {
			ac += 1;
		}
		if (weapon_main != null && weapon_off != null && feats.Contains (Feats.Feat.TwoWeaponDefense)) {
			ac += 1;
		}

		// check for full defense
		if (effects.ContainsKey (CombatEffects.CombatEffect.FullDefense)) {
			ac += 4;
		}

		return ac;
	}

	public int GetInitiativeModifier () {
		if (feats.Contains (Feats.Feat.ImprovedInitiative)) {
			return GetDexterityModifier () + 4;
		} else {
			return GetDexterityModifier ();
		}
	}

	public int GetConcentrationModifier () {
		int concentration = 3 + level + GetConstitutionModifier ();
		if (feats.Contains (Feats.Feat.CombatCasting)) {
			concentration += 4;
		}
		return concentration;
	}

	public int GetSpellDC () {
		int spell_dc = 10;

		if (character_class == Helpers.CharacterClass.Cleric) {
			spell_dc += GetWisdomModifier ();
		} else if (character_class == Helpers.CharacterClass.Wizard) {
			spell_dc += GetIntelligenceModifier ();
		}

		if (feats.Contains (Feats.Feat.SpellFocus)) {
			spell_dc++;
		}
		if (feats.Contains (Feats.Feat.GreaterSpellFocus)) {
			spell_dc++;
		}

		return spell_dc;
	}

	public int GetSpeed () {
		int armor_max_speed = armor == null ? 10 : armor.max_speed;
		int shield_max_speed = shield == null ? 10 : shield.max_speed;
		return Mathf.Min (speed, armor_max_speed, shield_max_speed);
	}

	/**
	 * Hitpoint related stuff
	 */
	public int GetMaxHitpoints () {
		return max_hitpoints;
	}
	public int GetCurrentHitpoints () {
		return hitpoints;
	}
	public Helpers.CombatState GetCombatState () {
		if (hitpoints > 0) {
			return Helpers.CombatState.Alive;
		} else {
			return Helpers.CombatState.Dead;
		}
	}
	public void DealDamage (int dmg, Helpers.DamageType dmgType) {
		if (effects.ContainsKey (CombatEffects.CombatEffect.Helpless)) {
			effects.Remove (CombatEffects.CombatEffect.Helpless);
		}

		int hp_pre = hitpoints;
		hitpoints -= dmg;
		if (hp_pre > 0 && hitpoints <= 0) {
			combatStatusChangedDelegate ();
		}
	}
	public void DoHealing (int healing) {
		if (effects.ContainsKey (CombatEffects.CombatEffect.Helpless)) {
			effects.Remove (CombatEffects.CombatEffect.Helpless);
		}

		int hp_pre = hitpoints;
		hitpoints = Mathf.Min (max_hitpoints, hitpoints + healing);
		if (hp_pre <= 0 && hitpoints > 0) {
			combatStatusChangedDelegate ();
		}
	}
	public void AdministerAid () {
		if (effects.ContainsKey (CombatEffects.CombatEffect.Helpless)) {
			effects.Remove (CombatEffects.CombatEffect.Helpless);
		}

		int hp_pre = hitpoints;
		hitpoints = Mathf.Min (max_hitpoints, hitpoints + 1);
		if (hp_pre <= 0 && hitpoints > 0) {
			combatStatusChangedDelegate ();
		}
	}

	/**
	 * CombatEffect related stuff
	 */
	public void UpdateCombatEffects () {
		foreach (CombatEffects.CombatEffect effect in effects.Keys) {
			effects [effect] -= 1;
			if (effects [effect] == 0) {

				switch (effect) {
				case CombatEffects.CombatEffect.EnlargePerson:
					character_size--;
					break;
				case CombatEffects.CombatEffect.ReducePerson:
					character_size++;
					break;
				}

				effects.Remove (effect);
			}
		}
	}
	public void AddCombatEffect (CombatEffects.CombatEffect effect, int duration) {
		effects [effect] = duration;
	}

	/**
	 * Some methods for finding out whether this character is eligble for stuff
	 */
	public bool CanAttack () {
		return attacks_left > 0;
	}
	public bool CanFullDefense () {
		return attacks_left == num_attacks;
	}
	public bool CanCastSpell () {
		List <Helpers.CharacterClass> spellCasters = new List<Helpers.CharacterClass> () {
			Helpers.CharacterClass.Wizard,
			Helpers.CharacterClass.Cleric
		};
		if (spellCasters.Contains (character_class)) {
			if (attacks_left == num_attacks && spells_left > 0) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
	public bool CanUseItem () {
		return false;
	}

	/**
	 * Methods for actually doing stuff
	 */
	public void GoFullDefense () {
		AddCombatEffect (CombatEffects.CombatEffect.FullDefense, 1);
	}

	public void MeleeAttack (CharacterSheet target, bool isFlanking) {
		// main hand attack
		OneAttack (target, isFlanking, true);

		// off hand attack, if applicable
		if (weapon_off != null) {
			OneAttack (target, isFlanking, false);
		}
	}

	public void OneAttack (CharacterSheet target, bool isFlanking, bool mainHand) {

		Weapon weapon = mainHand ? weapon_main : weapon_off;
		bool isCrit = false;
		bool isHit = false;

		int targetAC = target.GetArmorClass ();
		int roll = Helpers.RollD20 ();
		int toHit = roll + GetToHitBonus (mainHand) + (isFlanking ? 2 : 0);

		if (roll == 20 || (roll > 20 - GetCritRange (mainHand) && toHit >= targetAC)) {
			// potential crit
			int confirm = GetToHitBonus (mainHand) + Helpers.RollD20 () + (isFlanking ? 2 : 0);
			if (confirm >= targetAC) {
				isCrit = true;
			} else {
				isHit = true;
			}
		} else {
			if (toHit >= targetAC) {
				isHit = true;
			}
		}

		int damage = 0;
		if (isCrit) {
			for (int i = 0; i < weapon.crit_modifier; i++) {
				damage += GetAttackDamage (isFlanking, mainHand);
			}
			target.DealDamage (damage, Helpers.DamageType.Physical);
			log.Log ("CRIT - Roll: " + roll + ", Damage: " + damage);
		} else if (isHit) {
			damage += GetAttackDamage (isFlanking, mainHand);
			target.DealDamage (damage, Helpers.DamageType.Physical);
			log.Log ("HIT - ToHit: " + toHit + ", AC: " + targetAC + ", Damage: " + damage);
		} else {
			log.Log ("MISS - ToHit: " + toHit + ", AC: " + targetAC);
		}
	}

	public void CastSpell (CharacterSheet target, Spell spell) {
		string d = spell.DealDamage (this, target);
		string h = spell.DoHealing (this, target);
		string e = spell.ApplyEffect (this, target);
		if (d != "") { log.Log (d);	}
		if (h != "") { log.Log (h); }
		if (e != "") { log.Log (e); } 
		spells_left--;
	}

	public bool ConcentrationCheck (Spell spell) {
		int concentration = GetConcentrationModifier () + Helpers.RollD20 ();
		if (concentration < 0 + spell.spellLvl) {
			return false;
		} else {
			return true;
		}
	}

	public bool TurnStart () {
		UpdateCombatEffects ();

		// check if character is alive
		if (GetCombatState () != Helpers.CombatState.Alive) {
			return false;
		}
		// check if character is frightened, if so, 50% chance to lose turn
		if (effects.ContainsKey (CombatEffects.CombatEffect.Frightened)) {
			if (Random.Range (0, 2) < 1) {
				log.Log (character_name + " cowers in fear!");
				return false;
			}
		}
		// check if character is helpless
		if (effects.ContainsKey (CombatEffects.CombatEffect.Helpless)) {
			log.Log (character_name + " is asleep!");
			return false;
		}

		// character will have its turn
		moves_left = GetSpeed(); 
		attacks_left = num_attacks;
		return true;
	}
}
