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
	public int num_spells;

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
				movesLeftUpdatedDelegate (_moves_left, speed);
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
	public Weapon weapon;
	public Armor armor; 
	public Armor shield;

	public List<Helpers.Feat> feats;

	// temporary effects
	private List<CombatEffect> effects;

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
		new_delegate (moves_left, speed);
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
		effects = new List<CombatEffect> ();
		feats = new List<Helpers.Feat> ();
		strength = 8;
		dexterity = 8;
		constitution = 8;
		intelligence = 8;
		wisdom = 8;
		charisma = 8;
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

	// Two temporary bullshit initializers
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
		sheet.weapon = Weapon.LongSword ();
		sheet.armor = Armor.Breastplate ();
		sheet.shield = Armor.HeavyShield ();
		sheet.CalcMaxHitpoints ();
		sheet.hitpoints = sheet.max_hitpoints;
		sheet.moves_left = 0;
		sheet.attacks_left = 0;

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
		sheet.weapon = Weapon.Quarterstaff ();
		sheet.armor = Armor.None ();
		sheet.shield = Armor.None ();
		sheet.CalcMaxHitpoints ();
		sheet.CalcMaxSpells ();
		sheet.hitpoints = sheet.max_hitpoints;
		sheet.moves_left = 0;
		sheet.attacks_left = 0;

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
		sheet.weapon = Weapon.HeavyMace ();
		sheet.armor = Armor.ChainShirt ();
		sheet.shield = Armor.HeavyShield ();
		sheet.CalcMaxHitpoints ();
		sheet.CalcMaxSpells ();
		sheet.hitpoints = sheet.max_hitpoints;
		sheet.moves_left = 0;
		sheet.attacks_left = 0;

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
		sheet.weapon = Weapon.ShortSword ();
		sheet.armor = Armor.LeatherArmor ();
		sheet.shield = Armor.None ();
		sheet.CalcMaxHitpoints ();
		sheet.hitpoints = sheet.max_hitpoints;
		sheet.moves_left = 0;
		sheet.attacks_left = 0;

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
		return AbilityModifier (strength);
	}
	public int GetDexterityModifier () {
		return Mathf.Min (AbilityModifier (dexterity), armor.MaxDexterity, shield.MaxDexterity);	
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
		return fortitude + GetConstitutionModifier ();
	}
	public int GetReflex () {
		return reflex + GetDexterityModifier ();
	}
	public int GetWill () {
		return will + GetWisdomModifier ();
	}

	/**
	 * Combat Scores
	 */
	public int GetToHitBonus () {
		return 
			base_attack + 
			GetStrengthModifier () + 
			weapon.AttackBonus +
			(int) character_size;
	}
	public int GetCritRange () {
		return weapon.CritRange;
	}
	public int GetAttackDamage (bool isFlanking) {
		int dmg = Helpers.RollDice (weapon.DamageRoll) + weapon.DamageBonus + GetStrengthModifier ();
		if (character_class == Helpers.CharacterClass.Rogue && isFlanking) {
			dmg += Helpers.RollDice (new Helpers.DiceRoll{ numDice = (level + 1) / 2, sizeDice = 6 });
		}
		return dmg;
	}
	public int GetArmorClass () {
		int ac = 10 +
		         GetDexterityModifier () +
		         natural_armor +
		         armor.ArmorBonus +
		         shield.ArmorBonus +
		         (int)character_size;
		
		foreach (CombatEffect effect in effects) {
			ac = effect.ArmorClass (ac);
		}
		return ac;
	}
	public int GetInitiativeModifier () {
		return GetDexterityModifier ();
	}
	public int GetConcentrationModifier () {
		int concentration = 3 + level + GetConstitutionModifier ();
		if (feats.Contains (Helpers.Feat.CombatCasting)) {
			concentration += 4;
		}
		return concentration;
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
		int hp_pre = hitpoints;
		hitpoints -= dmg;
		if (hp_pre > 0 && hitpoints <= 0) {
			combatStatusChangedDelegate ();
		}
	}
	public void DoHealing (int healing) {
		int hp_pre = hitpoints;
		hitpoints = Mathf.Min (max_hitpoints, hitpoints + healing);
		if (hp_pre <= 0 && hitpoints > 0) {
			combatStatusChangedDelegate ();
		}
	}

	/**
	 * CombatEffect related stuff
	 */
	public void UpdateCombatEffects () {
		effects.RemoveAll (x => x.EndsThisTurn ());
	}
	public void AddCombatEffect (CombatEffect effect) {
		effects.Add (effect);
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
		effects.Add (new FullDefense ());
	}

	public void MeleeAttack (CharacterSheet target, bool isFlanking) {

		bool isCrit = false;
		bool isHit = false;

		int targetAC = target.GetArmorClass ();
		int roll = Helpers.RollD20 ();
		int toHit = roll + GetToHitBonus () + (isFlanking ? 2 : 0);

		if (roll == 20 || (roll > 20 - GetCritRange() && toHit >= targetAC)) {
			// potential crit
			int confirm = GetToHitBonus () + Helpers.RollD20 ();
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
			for (int i = 0; i < weapon.CritModifier; i++) {
				damage += GetAttackDamage (isFlanking);
			}
			target.DealDamage (damage, Helpers.DamageType.Physical);
			log.Log ("CRIT - Roll: " + roll + ", Damage: " + damage);
		} else if (isHit) {
			damage += GetAttackDamage (isFlanking);
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
}
