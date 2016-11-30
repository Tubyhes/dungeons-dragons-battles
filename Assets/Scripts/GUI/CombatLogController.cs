using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatLogController : MonoBehaviour {

	public Text combatLog;
	public Text anouncement;
	public ScrollRect scrollRect;
	public float brightTime = 3f;
	public float fadeTime = 3f;

	public void Log (string log) {
		// make sure the text becomes brightly colored again
		CancelInvoke ("StartFading");
		StopCoroutine ("FadeOutText");
		SetAlpha (combatLog, 1f);

		// append the text and scroll down
		combatLog.text += log + '\n';
		Canvas.ForceUpdateCanvases ();
		scrollRect.verticalNormalizedPosition = 0.5f;
		Canvas.ForceUpdateCanvases ();

		// make sure the text will fade out again
		Invoke ("StartFading", brightTime);
	}

	public void Clear () {
		CancelInvoke ("StartFading");
		StopCoroutine ("FadeOutText");
		SetAlpha (combatLog, 0f);

		combatLog.text = "";
	}

	public void SetAnouncement (string a) {
		anouncement.text = a;
		SetAlpha (anouncement, 1f);

		Invoke ("HideAnouncement", brightTime);
	}

	private void StartFading () {
		StartCoroutine ("FadeOutText");
	}

	private IEnumerator FadeOutText () {
		while (combatLog.color.a > 0.0001f) {
			SetAlpha (combatLog, combatLog.color.a - Time.deltaTime / fadeTime);
			yield return null;
		}

		SetAlpha (combatLog, 0f);
	}

	private void HideAnouncement () {
		SetAlpha (anouncement, 0f);
	}

	private void SetAlpha (Text text, float alpha) {
		Color color = text.color;
		color.a = alpha;
		text.color = color;
	}
}
