using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

	public CombatArea area;
	public PortraitController[] playerPortraits;
	public PortraitController[] enemyPortraits;
	public CombatLogController combatLogController;

	private SortedList<int, ICombatant> combatants;
	private IEnumerator enumCombatants;

	private Dictionary<ICombatant, Vector3> players;
	public Dictionary<ICombatant, Vector3> Players {
		get {
			return players;
		}
	}

	private Dictionary<ICombatant, Vector3> enemies;
	public Dictionary<ICombatant, Vector3> Enemies {
		get {
			return enemies;
		}
	}

	private float combatStartTime = 1f;

	void Awake () {
		combatants = new SortedList<int, ICombatant> (new Helpers.DescComparer<int> ());
		players = new Dictionary<ICombatant, Vector3> ();
		enemies = new Dictionary<ICombatant, Vector3> ();
	}

	void Start () {
		combatants.Clear ();
		players.Clear ();
		enemies.Clear ();
		area.SetupCombatArea (GameManager.Instance ().combat.groundType);
		Invoke("Initiative", combatStartTime);
	}

	private void Initiative () {
		foreach (KeyValuePair<int, ICombatant> combatant in combatants) {
			string logstring = combatant.Value.GetCombatTag () + " with Initiative: " + combatant.Key + '\n';
			LogToCombatLog (logstring);
		}
		enumCombatants = combatants.GetEnumerator ();

		enumCombatants.MoveNext ();
		KeyValuePair<int, ICombatant> next = (KeyValuePair<int, ICombatant>) enumCombatants.Current;
		next.Value.GiveTurn ();
	}

	public void RegisterPlayer (ICombatant combatant, Vector3 gridPosition, out PortraitController portraitController) {
		int initiative = combatant.GetInitiativeBonus () + Helpers.RollD20 ();
		combatants.Add (initiative, combatant);
		portraitController = playerPortraits [players.Count];
		players.Add (combatant, gridPosition);
	}

	public void RegisterEnemy (ICombatant combatant, Vector3 gridPosition, out PortraitController portraitController) {
		int initiative = combatant.GetInitiativeBonus () + Helpers.RollD20 ();
		combatants.Add (initiative, combatant);
		portraitController = enemyPortraits [enemies.Count];
		enemies.Add (combatant, gridPosition);
	}

	public void EndTurn () {
		if (!enumCombatants.MoveNext ()) {
			enumCombatants = combatants.GetEnumerator();
			enumCombatants.MoveNext ();
		}

		KeyValuePair<int, ICombatant> next = (KeyValuePair<int, ICombatant>) enumCombatants.Current;
		next.Value.GiveTurn ();
	}

	public void CheckEndCombat () {
		int playersAlive = players.Where (x => x.Key.GetCurrentState() == Helpers.CombatState.Alive).Count ();
		int enemiesAlive = enemies.Where (x => x.Key.GetCurrentState() == Helpers.CombatState.Alive).Count ();

		if (playersAlive == 0 || enemiesAlive == 0) {
			Invoke("EndCombat", 1f);
		}
	}

	public void MeleeAttack (ICombatant attacker, ICombatant target) {
		int targetAC = target.GetArmorClass ();
		int attackerToHit = attacker.GetToHitBonus () + Helpers.RollD20 ();

		if (attackerToHit >= targetAC) {
			int damage = attacker.GetDamageBonus () + Helpers.RollDice (attacker.GetDamageRoll ());
			target.DealDamage (damage);
			string logstring = "HIT: " + attacker.GetCombatTag () + " ToHit: " + attackerToHit + ", " + target.GetCombatTag () + " AC: " + targetAC + "\n" + "Damage: " + damage + '\n';
			LogToCombatLog (logstring);
		} else {
			string logstring = "MISS: " + attacker.GetCombatTag () + " ToHit: " + attackerToHit + ", " + target.GetCombatTag () + " AC: " + targetAC + '\n';
			LogToCombatLog (logstring);
		}
	}

	public bool IsGridPositionFree (Vector3 position) {

		// check if it is taken by blocking objects
		if (!area.IsGridPositionFree (position)) {
			return false;
		}

		// check if it is taken by a player
		foreach (KeyValuePair<ICombatant, Vector3> player in players) {
			if (player.Value.Equals (position)) {
				return false;
			}
		}

		// check if it is taken by a player
		foreach (KeyValuePair<ICombatant, Vector3> enemy in enemies) {
			if (enemy.Value.Equals (position)) {
				return false;
			}
		}

		// no other things could block a grid position
		return true;
	}

	public ICombatant GetEnemyAtGridPosition (Vector3 position) {
		foreach (KeyValuePair<ICombatant, Vector3> enemy in enemies) {
			if (enemy.Value.Equals (position)) {
				return enemy.Key;
			}
		}

		return null;
	}

	public ICombatant GetPlayerAtGridPosition (Vector3 position) {
		foreach (KeyValuePair<ICombatant, Vector3> player in players) {
			if (player.Value.Equals (position)) {
				return player.Key;
			}
		}

		return null;

	}

	private void EndCombat () {
		// TODO: handle consequences of combat (hp-loss, exp, etc)
		SceneManager.LoadSceneAsync ("StartArea", LoadSceneMode.Single);
	}

	private void LogToCombatLog (string log) {
		combatLogController.Log (log);
	}

}
