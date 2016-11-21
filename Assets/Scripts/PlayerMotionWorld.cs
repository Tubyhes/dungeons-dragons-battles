using UnityEngine;
using System.Collections;

public class PlayerMotionWorld : PlayerMotion {

	public LayerMask blockingLayer;
	public float range = 0.5f;
	public float moveSpeed = 5f;

	protected BoxCollider2D box2d;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		box2d = GetComponent<BoxCollider2D> ();
	}
	
	public RaycastHit2D FacingObject () {
		Helpers.Direction facingDir = FacingDirection ();
		Vector2 target = transform.position;

		if (facingDir == Helpers.Direction.Up) {
			target.y += range;	
		} else if (facingDir == Helpers.Direction.Down) {
			target.y -= range;
		} else if (facingDir == Helpers.Direction.Left) {
			target.x -= range;
		} else {
			target.x += range;
		}

		box2d.enabled = false;
		RaycastHit2D hitObject = Physics2D.Linecast (transform.position, target, blockingLayer);
		box2d.enabled = true;

		return hitObject;
	}

	public void StartInteracting (NPC other) {
		animator.SetBool ("walk", false);
		other.Interact (Helpers.OppositeDirection(FacingDirection()));
	}

	public void MoveTo (Vector3 target) {
		float magnitude = moveSpeed * Time.deltaTime;
		Vector3 delta = target * magnitude;
		rb2d.MovePosition (transform.position + delta);
	}
}
