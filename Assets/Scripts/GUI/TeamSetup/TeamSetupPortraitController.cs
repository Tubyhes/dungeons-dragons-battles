using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeamSetupPortraitController : MonoBehaviour {

	public Image portrait;
	public MyButton button;

	public void SetPlusButtonMode () {
		portrait.gameObject.SetActive (false);
		button.gameObject.SetActive (true);
	}

	public void SetPortraitMode (Sprite sprite) {
		portrait.gameObject.SetActive (true);
		portrait.sprite = sprite;
		button.gameObject.SetActive (false);
	}

	public void SetEmptyMode () {
		portrait.gameObject.SetActive (false);
		button.gameObject.SetActive (false);
	}
} 
