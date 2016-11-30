using UnityEngine;
using System.Collections;

public class PlayerMotion : MonoBehaviour {

	protected Animator animator;
	protected Rigidbody2D rb2d;
	protected SpriteRenderer sprt;

	// Use this for initialization
	protected virtual void Start () {
		animator = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		sprt = GetComponent<SpriteRenderer> ();
	}
	
	public void TurnLeft () {
		sprt.flipX = true;
		animator.SetInteger ("direction", (int)Helpers.Direction.Horizontal);
	}

	public void TurnRight () {
		sprt.flipX = false;
		animator.SetInteger ("direction", (int)Helpers.Direction.Horizontal);
	}

	public void TurnDown () {
		animator.SetInteger ("direction", (int)Helpers.Direction.Down);
	}

	public void TurnUp () {		
		sprt.flipX = false;
		animator.SetInteger ("direction", (int)Helpers.Direction.Up);
	}
		
	public void TurnTo (Vector3 target) {
		float diff_x = target.x - transform.position.x;
		float diff_y = target.y - transform.position.y;

		if (Mathf.Abs (diff_x) >= Mathf.Abs (diff_y)) {
			// turn horizontal
			if (diff_x < 0) {
				TurnLeft ();
			} else {
				TurnRight ();
			}
		} else {
			if (diff_y	 < 0) {
				TurnDown ();
			} else {
				TurnUp ();
			}
		}
	}

	public void StartWalkingAnimation () {
		animator.SetBool ("walk", true);
	}

	public void StopWalkingAnimation () {
		animator.SetBool ("walk", false);
	}

	public void StartAttackAnimation () {
		Invoke ("StartAttacking", 0.1f);
	}

	private void StartAttacking () {
		animator.SetTrigger ("attack");
	}

	public Helpers.Direction FacingDirection () {
		Helpers.Direction animationDirection = (Helpers.Direction)animator.GetInteger ("direction");

		if (animationDirection == Helpers.Direction.Up) {
			return Helpers.Direction.Up;
		} else if (animationDirection == Helpers.Direction.Down) {
			return Helpers.Direction.Down;
		} else {
			if (sprt.flipX) {
				return Helpers.Direction.Left;
			} else {
				return Helpers.Direction.Right;
			}			
		}
	}
}
