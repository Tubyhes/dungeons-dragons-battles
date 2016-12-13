using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	void Awake () {
		GameManager.Instance ();
		Invoke ("LoadFirstLevel", 2f);
	}

	private void LoadFirstLevel() {
		SceneManager.LoadSceneAsync ("TeamSetup", LoadSceneMode.Single);
	}
}
