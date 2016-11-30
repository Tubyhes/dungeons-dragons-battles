using UnityEngine;
using System.Collections;

public class PlayerMotionCombat : PlayerMotion {

	public float moveSpeed = 5f;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	public IEnumerator MoveTo (Vector3 target) 	{
		animator.SetBool ("walk", true);

		while ((transform.position - target).sqrMagnitude > 0.00001f) {
			Vector3 newPosition = Vector3.MoveTowards (rb2d.position, target, Time.deltaTime * moveSpeed);
			rb2d.MovePosition (newPosition);
			yield return null;
		}

		rb2d.MovePosition (target);

		animator.SetBool ("walk", false);
	}

	public void Die () {
		animator.SetBool ("dead", true);
	}

	public void Live () {
		TurnRight ();
		animator.SetBool ("dead", false);
	}
}
