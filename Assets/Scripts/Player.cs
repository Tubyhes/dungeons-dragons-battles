using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float range = 0.5f;
	public LayerMask blockingLayer;

	protected bool isAttacking;
	protected Animator animator;
	protected Rigidbody2D rb2d;
	protected BoxCollider2D box2d;
	protected SpriteRenderer sprt;

	protected int maxAttacks;
	protected int maxMoves;
	protected int maxHitPoints;

	// Use this for initialization
	protected virtual void Start () {
		animator = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		box2d = GetComponent<BoxCollider2D> ();
		sprt = GetComponent<SpriteRenderer> ();

		maxAttacks = 1;
		maxMoves = 4;
		maxHitPoints = 8;
	}
	
	protected void TurnLeft () {
		sprt.flipX = true;
		animator.SetInteger ("direction", (int)Helpers.Direction.Horizontal);
	}

	protected void TurnRight () {
		sprt.flipX = false;
		animator.SetInteger ("direction", (int)Helpers.Direction.Horizontal);
	}

	protected void TurnDown () {
		animator.SetInteger ("direction", (int)Helpers.Direction.Down);
	}

	protected void TurnUp () {		
		Vector3 newScale = transform.localScale;
		newScale.x = 1;
		transform.localScale = newScale;	
		animator.SetInteger ("direction", (int)Helpers.Direction.Up);
	}

	protected RaycastHit2D FacingObject () {
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

	protected Helpers.Direction FacingDirection () {
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
	 
	protected void StartAttackAnimation () {
		isAttacking = true;
		Invoke ("EndAttackAnimation", 0.5f);
		animator.SetTrigger ("attack");
	}

	protected void StartInteracting (NPC other) {
		animator.SetBool ("walk", false);
		other.Interact (Helpers.OppositeDirection(FacingDirection()));
	}

	protected void EndAttackAnimation () {
		isAttacking = false;
	}


}
