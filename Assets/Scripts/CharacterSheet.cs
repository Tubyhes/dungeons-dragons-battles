using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSheet : MonoBehaviour {

	// personalisation
	private string character_name;
	public string Name {
		get {
			return character_name;
		}
	}
	private string portrait;
	public string Portrait {
		get {
			return portrait;
		}
	}

	// generic stuff
	private int level;
	private Helpers.CharacterClass character_class;
	private Helpers.CharacterSize character_size;

	// primary abilities
	private int strength;
	private int dexterity;
	private int constitution;
	private int intelligence;
	private int wisdom;
	private int charisma;

	// life
	private int max_hitpoints;
	public int MaxHitpoints {
		get {
			return max_hitpoints;
		}
	}
	private int hit_die;

	// combat
	private int base_attack;
	private int speed;
	public int Speed {
		get {
			return speed;
		}
	}
	private int num_attacks;
	public int NumAttacks {
		get {
			return num_attacks;
		}
	}
	private int natural_armor;

	// saves
	private int fortitude;
	private int reflex;
	private int will;

	// highly dynamic stats
	private int hitpoints;
	public int Hitpoints {
		get {
			return hitpoints;
		} 
		set {
			hitpoints = value;
			if (hitpointsUpdatedDelegate != null) {
				hitpointsUpdatedDelegate (hitpoints, max_hitpoints);
			}
		}
	}
	private int moves_left;
	public int MovesLeft {
		get {
			return moves_left;
		}
		set {
			moves_left = value;
			if (movesLeftUpdatedDelegate != null) {
				movesLeftUpdatedDelegate (moves_left, speed);
			}
		}
	}
	private int attacks_left;
	public int AttacksLeft {
		get {
			return attacks_left;
		}
		set {
			attacks_left = value;
			if (attacksLeftUpdatedDelegate != null) {
				attacksLeftUpdatedDelegate (attacks_left, num_attacks);
			}
		}
	}

	// equipment
	private Weapon weapon;
	private Armor armor; 
	private Armor shield;

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
	public delegate void CombatStatusChangedDelegate ();
	public CombatStatusChangedDelegate combatStatusChangedDelegate;

	// Two temporary bullshit initializers
	public void FighterCharacterSheet () {
		character_name =	"Herpaderp";
		portrait = 			"varian_legion";
		level = 			1;
		character_class = 	Helpers.CharacterClass.Fighter;
		character_size = 	Helpers.CharacterSize.Medium;
		strength = 			16;
		dexterity = 		12;
		constitution = 		16;
		intelligence = 		10;
		wisdom = 			10;
		charisma = 			12;
		hit_die = 			10;
		base_attack = 		1;
		speed = 			4;
		num_attacks = 		1;
		natural_armor = 	0;
		fortitude = 		2;
		reflex = 			0;
		will = 				0;
		weapon = Weapon.LongSword ();
		armor = Armor.Breastplate ();
		shield = Armor.HeavyShield ();
		CalcMaxHitpoints ();
		Hitpoints = max_hitpoints;
		MovesLeft = 0;
		AttacksLeft = 0;
	}

	public void DogCharacterSheet () {
		character_name =	"Dog";
		portrait = 			"acidic_swamp_ooze";
		level = 			1;
		character_class =	Helpers.CharacterClass.Monster;
		character_size = 	Helpers.CharacterSize.Small;
		strength = 			13;
		dexterity = 		17;
		constitution = 		15;
		intelligence = 		2;
		wisdom = 			12;
		charisma = 			6;
		hit_die = 			8;
		base_attack = 		0;
		speed = 			4;
		num_attacks = 		1;
		natural_armor = 	1;
		fortitude = 		1;
		reflex = 			2;
		will = 				0;
		weapon = Weapon.Bite ();
		armor = Armor.None ();
		shield = Armor.None ();
		CalcMaxHitpoints ();
		Hitpoints = max_hitpoints;
		MovesLeft = 0;
		AttacksLeft = 0;
	}

	void Start () {
		// instantiate list of combat effects
		effects = new List<CombatEffect> ();
	}

	/**
	 * Setup methods
	 */
	private void CalcMaxHitpoints () {
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
		if (Hitpoints > 0) {
			return Helpers.CombatState.Alive;
		} else {
			return Helpers.CombatState.Dead;
		}
	}
	public void DealDamage (int dmg, Helpers.DamageType dmgType) {
		Hitpoints -= dmg;
		if (Hitpoints <= 0) {
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
}
