using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatLogController : MonoBehaviour {

	public Text combatLog;
	public ScrollRect scrollRect;
	public float brightTime = 3f;
	public float fadeTime = 2f;

	public void Log (string log) {
		// make sure the text becomes brightly colored again
		CancelInvoke ("StartFading");
		StopCoroutine ("FadeOutText");
		SetAlpha (1f);

		// append the text and scroll down
		combatLog.text += log;
		Canvas.ForceUpdateCanvases ();
		scrollRect.verticalNormalizedPosition = 0.5f;
		Canvas.ForceUpdateCanvases ();

		// make sure the text will fade out again
		Invoke ("StartFading", brightTime);
	}

	private void StartFading () {
		StartCoroutine ("FadeOutText");
	}

	private IEnumerator FadeOutText () {
		while (combatLog.color.a > 0.0001f) {
			SetAlpha (combatLog.color.a - Time.deltaTime / fadeTime);
			yield return null;
		}

		SetAlpha (0f);
	}

	private void SetAlpha (float alpha) {
		Color color = combatLog.color;
		color.a = alpha;
		combatLog.color = color;
	}
}
