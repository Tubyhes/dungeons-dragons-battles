using UnityEngine;
using System.Collections;

public abstract class Combatant : MonoBehaviour {

	public delegate void HasTurnChanged (bool hasTurn);
	public HasTurnChanged hasTurnChanged;

	[HideInInspector]
	public Helpers.Teams team;

	public abstract string GetCombatTag ();
	public abstract void GiveTurn ();

	protected CharacterSheet characterSheet;
	public abstract void SetCharacterSheet (CharacterSheet sheet);
}
