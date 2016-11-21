using UnityEngine;
using System.Collections;

public interface ICombatant {

	string GetCombatTag ();
	void GiveTurn ();
}
