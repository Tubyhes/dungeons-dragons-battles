using UnityEngine;
using System.Collections;

public class MagicMissileFX : MonoBehaviour {

	public float moveSpeed;

	public Coroutine SetTarget (Vector3 target) {
		Vector3 v = target - transform.position;
		float angle = Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		transform.rotation = q;

		return StartCoroutine (MoveToTarget (target));
	}

	public IEnumerator MoveToTarget (Vector3 target) {
		Rigidbody2D rb2d = GetComponent<Rigidbody2D> ();

		while ((transform.position - target).sqrMagnitude > 0.00001f) {
			Vector3 newPosition = Vector3.MoveTowards (rb2d.position, target, Time.deltaTime * moveSpeed);
			rb2d.MovePosition (newPosition);
			yield return null;
		}

		Destroy (gameObject);
	}

}
