using UnityEngine;
using System.Collections;

public class CombatActionController : MonoBehaviour {

	public SpriteRenderer icon;
	public SpriteRenderer border;
	public Helpers.CombatAction action;

	public void SetAvailable (bool available) {
		if (available) {
			icon.color = new Color (1f, 1f, 1f, 1f);
			border.color = new Color (0f, 0f, 0f, 1f);
		} else {
			icon.color = new Color (1f, 1f, 1f, 0.5f);
			border.color = new Color (1f, 0f, 0f, 0.5f);
		}
	}

	public void SetHighlight (bool highlight) {
		if (highlight) {
			border.color = new Color (0f, 1f, 1f, 1f);
		} else {
			border.color = new Color (0f, 0f, 0f, 1f);
		}
	}
		
}
