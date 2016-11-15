using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	protected Rigidbody2D rb2d;
	protected BoxCollider2D box2d;

	protected int maxAttacks;
	protected int maxMoves;
	protected int maxHitPoints;

	protected virtual void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		box2d = GetComponent<BoxCollider2D> ();

		maxAttacks = 1;
		maxMoves = 2;
		maxHitPoints = 5;
	}

}
