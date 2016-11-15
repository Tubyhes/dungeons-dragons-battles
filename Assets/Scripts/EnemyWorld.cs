using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyWorld : Enemy {

	private string enemyType;

	protected override void Start () {
		enemyType = "Enemy";
	}

	public void Attacked () {
		GameManager.Instance ().combat.enemyType = enemyType;
		Invoke ("EnterCombat", 2f);
	}

	private void EnterCombat () {
		SceneManager.LoadSceneAsync ("CombatArea", LoadSceneMode.Single);
	}
}
