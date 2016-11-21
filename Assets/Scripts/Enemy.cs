using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	protected Rigidbody2D rb2d;

	protected virtual void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}

}
