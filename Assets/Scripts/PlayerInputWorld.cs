using UnityEngine;
using System.Collections;

public class PlayerInputWorld : MonoBehaviour {

	public float moveSpeed = 5f;

	private PlayerMotionWorld pmw;

	void Start () {
		pmw = GetComponent<PlayerMotionWorld> ();
	}

	// Update is called once per frame
	void Update () {

		if (pmw.isAttacking || GameManager.Instance().IsInteracting) {
			return;
		}

		if (Input.GetButton ("Fire1")) {
			RaycastHit2D hitObject = pmw.FacingObject ();
			if (hitObject.collider == null) {
				pmw.StartAttackAnimation ();
			} else if (hitObject.transform.tag == "InanimateObject") {
				pmw.StartAttackAnimation ();			
			} else if (hitObject.transform.tag == "NPC") {
				NPC npc = hitObject.transform.GetComponent<NPC> ();
				pmw.StartInteracting (npc);
			} else if (hitObject.transform.tag == "Enemy") {
				pmw.StartAttackAnimation ();
				EnemyWorld enemy = hitObject.transform.GetComponent<EnemyWorld> ();
				enemy.Attacked ();
			}
				
			return;
		}

		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		if (horizontal == 0 && vertical == 0) {
			pmw.StopWalkingAnimation ();
			return;
		} 

		if (Mathf.Abs (horizontal) >= Mathf.Abs (vertical)) {
			if (horizontal < 0) {
				pmw.TurnLeft ();
			} else {
				pmw.TurnRight ();
			}
		} else {
			if (vertical < 0) {
				pmw.TurnDown ();
			} else {
				pmw.TurnUp ();
			}
		}
		pmw.StartWalkingAnimation ();
		pmw.MoveTo(new Vector3 (horizontal, vertical, 0).normalized);
	}
		

//	private string ToString (Vector3 v) {
//		return "(" + v.x + ";" + v.y + ";" + v.z + ")";
//	}
}

