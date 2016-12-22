using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager {

	// Singleton GameManager instance
	private static GameManager instance;

	// Struct for holding CombatSettings
	public struct CombatSettings {
		public Helpers.GroundTypes groundType;
		public string enemyType;
	}
	// Persistent CombatSettings variable for next combat
	public CombatSettings combat;

	public Dictionary<Helpers.Teams, List<CharacterSheet>> teamCombatants;

	public int maxTeamSize = 3;
	private bool hotSeat = true;
	private bool isInteracting;
	public bool IsInteracting {
		get {
			return isInteracting;
		}
		set {
			isInteracting = value;
		}
	}

	private GameManager () {
		teamCombatants = new Dictionary<Helpers.Teams, List<CharacterSheet>> ();
		teamCombatants [Helpers.Teams.Home] = new List<CharacterSheet> ();
		teamCombatants [Helpers.Teams.Away] = new List<CharacterSheet> ();

		combat = new CombatSettings () { groundType = Helpers.GroundTypes.Grassland, enemyType = "Enemy" };
	}

	// Singleton accessor method
	public static GameManager Instance () {
		if (instance == null) {
			GameManager newManager = new GameManager();
			instance = newManager;
			Debug.Log ("Created a new GameManager!");
		}

		return instance;
	}

	public bool AddPlayerToTeam (CharacterSheet character_sheet, Helpers.Teams team) {
		teamCombatants [team].Add (character_sheet);
		if (teamCombatants [team].Count == maxTeamSize &&
		    teamCombatants [Helpers.otherTeam (team)].Count == maxTeamSize) {
			return true;
		} else {
			return false;
		}

	}

	public void PopulateTeams () {
		teamCombatants [Helpers.Teams.Home].Clear ();
		teamCombatants [Helpers.Teams.Home].Add (CharacterSheet.RogueReady ());
		teamCombatants [Helpers.Teams.Home].Add (CharacterSheet.WizardReady ());
		teamCombatants [Helpers.Teams.Home].Add (CharacterSheet.FighterReady ());

		teamCombatants [Helpers.Teams.Away].Clear ();
		teamCombatants [Helpers.Teams.Away].Add (CharacterSheet.RogueReady ());
		teamCombatants [Helpers.Teams.Away].Add (CharacterSheet.WizardReady ());
		teamCombatants [Helpers.Teams.Away].Add (CharacterSheet.FighterReady ());
	}
}
