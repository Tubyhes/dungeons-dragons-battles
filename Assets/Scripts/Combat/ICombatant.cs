using UnityEngine;
using System.Collections;

public interface ICombatant {

	Helpers.CombatState GetCurrentState ();
	string GetCombatTag ();
	int GetInitiativeBonus ();
	int GetArmorClass ();
	int GetToHitBonus ();
	int GetDamageBonus ();

	Helpers.DiceRoll GetDamageRoll ();	

	void DealDamage (int dmg);

	void GiveTurn ();
}
