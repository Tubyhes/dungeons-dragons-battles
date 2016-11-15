using UnityEngine;
using System.Collections;

public class PlayerWorld : Player {

	public float moveSpeed = 5f;

	// Update is called once per frame
	void Update () {

		if (isAttacking || GameManager.Instance().IsInteracting) {
			return;
		}

		if (Input.GetButton ("Fire1")) {
			RaycastHit2D hitObject = FacingObject ();
			if (hitObject.collider == null) {
//				Debug.Log ("Nothing but air!");
				StartAttackAnimation ();
			} else if (hitObject.transform.tag == "InanimateObject") {
//				Debug.Log ("Chopping down a tree!");
				StartAttackAnimation ();
			} else if (hitObject.transform.tag == "NPC") {
//				Debug.Log ("Talking to an NPC!");
				NPC npc = hitObject.transform.GetComponent<NPC> ();
				StartInteracting (npc);
			} else if (hitObject.transform.tag == "Enemy") {
//				Debug.Log ("Killing an enemy!");
				EnemyWorld enemy = hitObject.transform.GetComponent<EnemyWorld> ();
				StartAttackAnimation ();
				enemy.Attacked ();
			}
				
			return;
		}

		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		if (horizontal == 0 && vertical == 0) {
			animator.SetBool ("walk", false);
			return;
		} 

		if (Mathf.Abs (horizontal) >= Mathf.Abs (vertical)) {
			if (horizontal < 0) {
				TurnLeft ();
				animator.SetInteger ("direction", (int)Helpers.Direction.Horizontal);
			} else {
				TurnRight ();
				animator.SetInteger ("direction", (int)Helpers.Direction.Horizontal);
			}
		} else {
			if (vertical < 0) {
				animator.SetInteger ("direction", (int)Helpers.Direction.Down);
			} else {
				animator.SetInteger ("direction", (int)Helpers.Direction.Up);
			}
		}
		animator.SetBool ("walk", true);

		Vector3 direction = new Vector3 (horizontal, vertical, 0).normalized;
		float magnitude = moveSpeed * Time.deltaTime;
		Vector3 delta = direction * magnitude;
		rb2d.MovePosition (transform.position + delta);
	}

//	private string ToString (Vector3 v) {
//		return "(" + v.x + ";" + v.y + ";" + v.z + ")";
//	}
}

