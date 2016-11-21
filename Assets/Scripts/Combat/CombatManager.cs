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

	private Dictionary<CharacterSheet, Vector3> players;
	public Dictionary<CharacterSheet, Vector3> Players {
		get {
			return players;
		}
	}

	private Dictionary<CharacterSheet, Vector3> enemies;
	public Dictionary<CharacterSheet, Vector3> Enemies {
		get {
			return enemies;
		}
	}

	private float combatStartTime = 1f;

	void Awake () {
		combatants = new SortedList<int, ICombatant> (new Helpers.DescComparer<int> ());
		players = new Dictionary<CharacterSheet, Vector3> ();
		enemies = new Dictionary<CharacterSheet, Vector3> ();
	}

	void Start () {
		combatants.Clear ();
		players.Clear ();
		enemies.Clear ();
		area.SetupCombatArea (GameManager.Instance ().combat.groundType);

		AddPlayer ();
		AddEnemy ();

		Invoke("Initiative", combatStartTime);
	}

	private void AddPlayer () {
		string playerResource = GameManager.Instance().playerType + "Combat";
		float xPos = 2;
		float yPos = area.height / 2; 
		GameObject player = Instantiate (Resources.Load(playerResource), new Vector3 (xPos, yPos, 0), Quaternion.identity) as GameObject;
		player.transform.SetParent (area.area);

		PlayerInputCombat pic = player.GetComponent<PlayerInputCombat> ();
		CharacterSheet sheet = player.GetComponent<CharacterSheet> ();

		int initiative = sheet.GetInitiativeModifier () + Helpers.RollD20 ();
		combatants.Add (initiative, pic);

		playerPortraits [players.Count].Enable ();
		sheet.registerHitpointsDelegate (playerPortraits [players.Count].SetHitpointsLeft);
		sheet.registerMovesLeftDelegate (playerPortraits [players.Count].SetMovesLeft);
		sheet.registerAttacksLeftDelegate (playerPortraits [players.Count].SetAttacksLeft);
		sheet.combatStatusChangedDelegate += CheckEndCombat;
		pic.hasTurnChanged = playerPortraits [players.Count].SetHighlighted;
		playerPortraits [players.Count].SetImage (Resources.Load<Sprite> (sheet.Portrait));

		players.Add (sheet, player.transform.position);
	}

	private void AddEnemy () {
		string enemyResource = GameManager.Instance ().combat.enemyType + "Combat";
		float xPos = area.width - 3;
		float yPos = area.height / 2; 
		GameObject enemy = Instantiate (Resources.Load(enemyResource), new Vector3 (xPos, yPos, 0), Quaternion.identity) as GameObject;
		enemy.transform.SetParent (area.area);

		EnemyCombat ec = enemy.GetComponent<EnemyCombat> ();
		CharacterSheet sheet = enemy.GetComponent<CharacterSheet> ();

		int initiative = sheet.GetInitiativeModifier () + Helpers.RollD20 ();
		combatants.Add (initiative, ec);

		enemyPortraits [enemies.Count].Enable ();
		sheet.registerHitpointsDelegate (enemyPortraits [enemies.Count].SetHitpointsLeft);
		sheet.registerMovesLeftDelegate (enemyPortraits [enemies.Count].SetMovesLeft);
		sheet.registerAttacksLeftDelegate (enemyPortraits [enemies.Count].SetAttacksLeft);
		sheet.combatStatusChangedDelegate += CheckEndCombat;
		ec.hasTurnChanged = enemyPortraits [enemies.Count].SetHighlighted;
		enemyPortraits [enemies.Count].SetImage (Resources.Load<Sprite> (sheet.Portrait));

		enemies.Add (sheet, enemy.transform.position);
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

	public void EndTurn () {
		if (!enumCombatants.MoveNext ()) {
			enumCombatants = combatants.GetEnumerator();
			enumCombatants.MoveNext ();
		}

		KeyValuePair<int, ICombatant> next = (KeyValuePair<int, ICombatant>) enumCombatants.Current;
		next.Value.GiveTurn ();
	}

	public void CheckEndCombat () {
		int playersAlive = players.Where (x => x.Key.GetCombatState() == Helpers.CombatState.Alive).Count ();
		int enemiesAlive = enemies.Where (x => x.Key.GetCombatState() == Helpers.CombatState.Alive).Count ();

		if (playersAlive == 0 || enemiesAlive == 0) {
			Invoke("EndCombat", 1f);
		}
	}

	public void MeleeAttack (CharacterSheet attacker, CharacterSheet target) {
		int targetAC = target.GetArmorClass ();
		int attackerToHit = attacker.GetToHitBonus () + Helpers.RollD20 ();

		if (attackerToHit >= targetAC) {
			int damage = attacker.GetAttackDamage (false);
			target.DealDamage (damage, Helpers.DamageType.Physical);
			string logstring = "HIT: attacker ToHit: " + attackerToHit + ", defender AC: " + targetAC + "\n" + "Damage: " + damage + '\n';
			LogToCombatLog (logstring);
		} else {
			string logstring = "MISS: attacker ToHit: " + attackerToHit + ", defender AC: " + targetAC + '\n';
			LogToCombatLog (logstring);
		}
	}

	public bool IsGridPositionFree (Vector3 position) {

		// check if it is taken by blocking objects
		if (!area.IsGridPositionFree (position)) {
			return false;
		}

		// check if it is taken by a player
		foreach (KeyValuePair<CharacterSheet, Vector3> player in players) {
			if (player.Value.Equals (position)) {
				return false;
			}
		}

		// check if it is taken by a player
		foreach (KeyValuePair<CharacterSheet, Vector3> enemy in enemies) {
			if (enemy.Value.Equals (position)) {
				return false;
			}
		}

		// no other things could block a grid position
		return true;
	}

	public CharacterSheet GetEnemyAtGridPosition (Vector3 position) {
		foreach (KeyValuePair<CharacterSheet, Vector3> enemy in enemies) {
			if (enemy.Value.Equals (position)) {
				return enemy.Key;
			}
		}

		return null;
	}

	public CharacterSheet GetPlayerAtGridPosition (Vector3 position) {
		foreach (KeyValuePair<CharacterSheet, Vector3> player in players) {
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
