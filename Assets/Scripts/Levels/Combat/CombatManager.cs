using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

	public CombatArea area;
	public PortraitController[] homePortraits;
	public PortraitController[] awayPortraits;
	public CombatLogController combatLogController;
	public SpriteRenderer sr;
	public bool testing = false;

	private struct InitiativeCombatant {
		public int initiative;
		public int bonus;
		public Combatant combatant;
	}
	private List<InitiativeCombatant> combatants;
	private IEnumerator enumCombatants;

	public Dictionary<Helpers.Teams, Dictionary<CharacterSheet, Vector3>> teams;

	private float combatStartTime = 1f;
	private int round = 1;
	public bool combatEnded = false;

	void Awake () {
		combatants = new List<InitiativeCombatant> ();
		teams = new Dictionary<Helpers.Teams, Dictionary<CharacterSheet, Vector3>> ();
		teams [Helpers.Teams.Home] = new Dictionary<CharacterSheet, Vector3> ();
		teams [Helpers.Teams.Away] = new Dictionary<CharacterSheet, Vector3> ();
	}

	void Start () {
		combatants.Clear ();
		teams [Helpers.Teams.Home].Clear ();
		teams [Helpers.Teams.Away].Clear ();

		area.SetupCombatArea (GameManager.Instance ().combat.groundType);

		StartCoroutine ("AddCombatants");
	}

	private IEnumerator AddCombatants () {

		foreach (Helpers.Teams team in new List<Helpers.Teams> (){Helpers.Teams.Home, Helpers.Teams.Away}) {

			// obtain the correct list of combatant types for this team
			if (testing) {
				GameManager.Instance ().PopulateTeams ();
			}
			List<CharacterSheet> combatantSheets = GameManager.Instance ().teamCombatants [team];

			// for each combatant in the list, spawn the correct object
			foreach (CharacterSheet sheet in combatantSheets) {
				GameObject gameObject = SpawnCombatant (sheet.character_class, team);

				// obtain references to the PlayerInputCombat and CharacterSheet components
				Combatant combatant = gameObject.GetComponent<Combatant> ();
				combatant.team = team;
				sheet.log = combatLogController;
				combatant.SetCharacterSheet (sheet);

				// roll initiative for player and add him to the list of combatants
				int initiative = sheet.GetInitiativeModifier () + Helpers.RollD20 ();
				InsertCombatant (new InitiativeCombatant () {
					initiative = initiative,
					bonus = sheet.GetInitiativeModifier (),
					combatant = combatant
				});

				// tie the player to a portrait
				PortraitController[] portraits;
				if (team == Helpers.Teams.Home) {
					portraits = homePortraits;
				} else {
					portraits = awayPortraits;
				}
				int index = teams [team].Count;

				portraits [index].Enable ();
				sheet.registerHitpointsDelegate (portraits [index].SetHitpointsLeft);
				sheet.registerMovesLeftDelegate (portraits [index].SetMovesLeft);
				sheet.registerAttacksLeftDelegate (portraits [index].SetAttacksLeft);
				sheet.combatStatusChangedDelegate += CheckEndCombat;
				combatant.hasTurnChanged = portraits [index].SetHighlighted;
				portraits [index].SetImage (Helpers.classPortraits[sheet.character_class]);

				// add player to this team
				teams [team].Add (sheet, gameObject.transform.position);

				string teamString = team == Helpers.Teams.Home ? "Home" : "Away";
				combatLogController.Log(sheet.character_name + " joined the " + teamString + " team. Initiative: " + initiative); 

				yield return new WaitForSeconds (1.5f);
			}
		}

		Invoke("Initiative", combatStartTime);
	}

	private void InsertCombatant (InitiativeCombatant combatant) {
		int i = 0;
		for (i = 0; i < combatants.Count; i++) {
			if (combatants [i].initiative < combatant.initiative) {
				combatants.Insert (i, combatant);
				return;
			} else if (combatants [i].initiative == combatant.initiative && combatants [i].bonus < combatant.bonus) {
				combatants.Insert (i, combatant);
				return;
			}
		}

		// if we get here, we just insert it in last position
		combatants.Insert (i, combatant);
	}

	private GameObject SpawnCombatant (Helpers.CharacterClass character_class, Helpers.Teams team) {
		int count = teams [team].Count;
		int xPos = team == Helpers.Teams.Home ? 2 : area.width - 3;
		int yPos = area.height / 2 + (count % 2 == 1 ? -(count + 1) / 2 : (count + 1) / 2);

		string resource = Helpers.classToString[character_class] + (team == Helpers.Teams.Home ? "Home" : "Away") + "Prefab";
		GameObject combatant = Instantiate (Resources.Load(resource), new Vector3 (xPos, yPos, 0), Quaternion.identity) as GameObject;
		combatant.transform.SetParent (area.area);

		return combatant;
	}

	private void Initiative () {
		combatLogController.Clear ();
		combatLogController.SetAnouncement ("ROUND 1");

		enumCombatants = combatants.GetEnumerator ();
		enumCombatants.MoveNext ();
		InitiativeCombatant next = (InitiativeCombatant) enumCombatants.Current;
		next.combatant.GiveTurn ();
	}

	public void EndTurn () {
		if (combatEnded) {
			return;
		}

		if (!enumCombatants.MoveNext ()) {
			round++;
			combatLogController.SetAnouncement ("ROUND " + round);
			enumCombatants = combatants.GetEnumerator();
			enumCombatants.MoveNext ();
		}

		InitiativeCombatant next = (InitiativeCombatant) enumCombatants.Current;
		next.combatant.GiveTurn ();
	}

	public void CheckEndCombat () {
		int playersAlive = teams [Helpers.Teams.Home].Where (x => x.Key.GetCombatState () == Helpers.CombatState.Alive).Count ();
		int enemiesAlive = teams [Helpers.Teams.Away].Where (x => x.Key.GetCombatState() == Helpers.CombatState.Alive).Count ();

		if (playersAlive == 0 || enemiesAlive == 0) {
			combatEnded = true;
			Invoke("EndCombat", 1.5f);
		}
	}

	public bool IsGridPositionFree (Vector3 position) {

		// check if it is taken by blocking objects
		if (!area.IsGridPositionFree (position)) {
			return false;
		}

		// check if it is taken by a player
		foreach (KeyValuePair<CharacterSheet, Vector3> combatant in teams [Helpers.Teams.Home]) {
			if (combatant.Value.Equals (position)) {
				return false;
			}
		}

		// check if it is taken by a player
		foreach (KeyValuePair<CharacterSheet, Vector3> combatant in teams [Helpers.Teams.Away]) {
			if (combatant.Value.Equals (position)) {
				return false;
			}
		}

		// no other things could block a grid position
		return true;
	}

	public CharacterSheet GetCombatantOfTeamAtGridPosition (Vector3 position, Helpers.Teams team) {
		foreach (KeyValuePair<CharacterSheet, Vector3> combatant in teams [team]) {
			if (combatant.Value.Equals (position)) {
				return combatant.Key;
			}
		}

		return null;
	}

	public CharacterSheet GetCombatantAtGridPosition (Vector3 position) {
		foreach (KeyValuePair<CharacterSheet, Vector3> combatant in teams [Helpers.Teams.Home]) {
			if (combatant.Value.Equals (position)) {
				return combatant.Key;
			}
		}

		foreach (KeyValuePair<CharacterSheet, Vector3> combatant in teams [Helpers.Teams.Away]) {
			if (combatant.Value.Equals (position)) {
				return combatant.Key;
			}
		}

		return null;
	}

	public bool IsCombatantInMelee (Vector3 position, Helpers.Teams team) {
		foreach (KeyValuePair<CharacterSheet,Vector3> combatant in teams[Helpers.otherTeam(team)]) {
			float distance = Mathf.Abs (position.x - combatant.Value.x) + Mathf.Abs (position.y - combatant.Value.y);
			if (distance <= 1f && combatant.Key.GetCombatState () == Helpers.CombatState.Alive) {
				return true;
			}
		}

		return false;
	}

	private void EndCombat () {
		// TODO: handle consequences of combat (hp-loss, exp, etc)
		combatLogController.SetAnouncement("Combat Ended!");
	}
		
}
