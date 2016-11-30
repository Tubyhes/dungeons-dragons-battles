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

	public Dictionary<Helpers.Teams, List<string>> teamCombatants;

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
		teamCombatants = new Dictionary<Helpers.Teams, List<string>> ();
		teamCombatants [Helpers.Teams.Home] = new List<string> ();
		teamCombatants [Helpers.Teams.Away] = new List<string> ();

		teamCombatants [Helpers.Teams.Home].Add ("PlayerClericHome");
		teamCombatants [Helpers.Teams.Home].Add ("PlayerMageHome");
//		teamCombatants [Helpers.Teams.Home].Add ("PlayerKnightAway");
//		teamCombatants [Helpers.Teams.Home].Add ("PlayerKnightHome");

//		teamCombatants [Helpers.Teams.Away].Add (hotSeat ? "PlayerKnightAway" : "EnemyCombat");
		teamCombatants [Helpers.Teams.Away].Add (hotSeat ? "PlayerKnightAway" : "EnemyCombat");
//		teamCombatants [Helpers.Teams.Away].Add (hotSeat ? "PlayerKnightAway" : "EnemyCombat");
//		teamCombatants [Helpers.Teams.Away].Add (hotSeat ? "PlayerKnightAway" : "EnemyCombat");

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
}
