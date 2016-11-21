using UnityEngine;
using System.Collections;

public class PlayerMotionCombat : PlayerMotion {

	public float moveSpeed = 50f;
	public bool isMoving;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		isMoving = false;
	}

	public IEnumerator MoveTo (Vector3 target) 	{
		isMoving = true;
		animator.SetBool ("walk", true);

		while ((transform.position - target).sqrMagnitude > 0.00001f) {
			Vector3 newPosition = Vector3.MoveTowards (rb2d.position, target, Time.deltaTime * moveSpeed);
			rb2d.MovePosition (newPosition);
			yield return null;
		}

		animator.SetBool ("walk", false);
		isMoving = false;
	}
}
