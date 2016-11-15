using UnityEngine;
using System.Collections;

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

	public string playerType;

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
		playerType = "PlayerKnight";
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
