using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PortraitController : MonoBehaviour {

	public GameObject portraitHolder;
	public Image portraitImage;
	public GameObject movesLeftBar;
	public GameObject attacksLeftBar;
	public GameObject hitPointsLeftBar;
	public Text movesLeftText;
	public Text attacksLeftText;
	public Text hitPointsLeftText;

	public void Enable () {
		portraitImage.gameObject.SetActive (true);
	}

	public void SetMovesLeft (int movesLeft, int maxMoves) {

		Vector3 scale = movesLeftBar.transform.localScale;
		Vector3 position = movesLeftBar.transform.position;

		scale.x = Mathf.Max((float)movesLeft / (float)maxMoves, 0f);
		movesLeftBar.transform.localScale = scale;
		movesLeftBar.transform.position = position;

		movesLeftText.text = "Moves: " + movesLeft + "/" + maxMoves;
	}

	public void SetAttacksLeft (int attacksLeft, int maxAttacks) {

		Vector3 scale = attacksLeftBar.transform.localScale;
		Vector3 position = attacksLeftBar.transform.position;

		scale.x = Mathf.Max((float)attacksLeft / (float)maxAttacks, 0f);
		attacksLeftBar.transform.localScale = scale;
		attacksLeftBar.transform.position = position;

		attacksLeftText.text = "Attacks: " + attacksLeft + "/" + maxAttacks;
	}

	public void SetHitpointsLeft (int hpLeft, int maxHitPoints) {

		Vector3 scale = hitPointsLeftBar.transform.localScale;
		Vector3 position = hitPointsLeftBar.transform.position;

		scale.x = Mathf.Max((float)hpLeft / (float)maxHitPoints, 0f);
		hitPointsLeftBar.transform.localScale = scale;
		hitPointsLeftBar.transform.position = position;

		hitPointsLeftText.text = "HP: " + hpLeft + "/" + maxHitPoints;
	}

	public void SetImage (Sprite image) {
		portraitImage.sprite = image;
	}

	public void SetHighlighted (bool highlighted) {
		if (highlighted) {
			portraitHolder.GetComponent<Image> ().color = new Color (125f, 0f, 0f);
		} else {
			portraitHolder.GetComponent<Image> ().color = new Color (0f, 0f, 0f);
		}
	}
}
