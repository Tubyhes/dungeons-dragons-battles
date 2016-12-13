using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClassButton : MonoBehaviour {

	public Image portrait;
	public MyButton button;

	public void SetAvailable (bool available) {
		if (available) {
			portrait.color = Color.white;
			button.interactable = true;
		} else {
			portrait.color = new Color (1f, 1f, 1f, 0.3f);
			button.interactable = false;
		}
	}
		
}
