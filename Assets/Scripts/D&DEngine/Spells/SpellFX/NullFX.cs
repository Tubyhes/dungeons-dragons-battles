using UnityEngine;
using System.Collections;

public class NullFX : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	public Coroutine StartFX () {
		return StartCoroutine (End ());
	}

	public IEnumerator End () {
		yield return new WaitForSeconds (1.5f);
		Destroy (gameObject);
	}
}
