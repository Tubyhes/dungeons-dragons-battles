using UnityEngine;
using System.Collections;

public class ShieldFX : MonoBehaviour {

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public Coroutine StartFXBlue () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.color = new Color (0f, 0f, 1f, 0f);
		return StartCoroutine (ShieldAnimation ());
	}

	public Coroutine StartFXGreen () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.color = new Color (0f, 1f, 0f, 0f);
		return StartCoroutine (ShieldAnimation ());
	}

	private IEnumerator ShieldAnimation () {
		while (spriteRenderer.color.a < 0.99f) {
			SetAlpha (spriteRenderer.color.a + Time.deltaTime / 1.5f);
			yield return null;
		}

		Destroy (gameObject);
	}

	private void SetAlpha (float alpha) {
		Color color = spriteRenderer.color;
		color.a = alpha;
		spriteRenderer.color = color;
	}
}
