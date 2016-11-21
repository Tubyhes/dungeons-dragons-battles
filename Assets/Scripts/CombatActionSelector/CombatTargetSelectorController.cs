using UnityEngine;
using System.Collections;

public class CombatTargetSelectorController : MonoBehaviour {

	public CombatActionSelectorController casc;

	private int range;
	private int aoe;
	private bool targetRequired;
	private CombatManager combatManager;
	private bool ignoreFirstClick = true;

	// Use this for initialization
	void Start () {
		combatManager = FindObjectOfType (typeof(CombatManager)) as CombatManager;
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.activeSelf) {
			return;
		}

		if (ignoreFirstClick) {
			ignoreFirstClick = false;
			return;
		}

		// listen to cancel button
		if (Input.GetButtonDown ("Fire2")) {
			TargetSelectorCancelled ();
			return;
		}

		// listen to select action
		if (Input.GetButtonDown ("Fire1")) {
			TargetSelected ();
			return;
		}

		// listen to horizontal / vertical input
		Vector2 dir = AxisButtonDownGetter.GetInput ();
		if (dir.Equals (Vector2.zero)) {
			return;
		} else {
			Move (dir);
			return;
		}

	}
		
	public void Enable (int range, int aoe, bool targetRequired) {
		this.range = range;
		this.aoe = aoe;
		this.targetRequired = targetRequired;
		ignoreFirstClick = true;
		gameObject.SetActive (true);
	}

	private void DisableTargetSelector () {
		transform.localPosition = Vector3.zero;
		gameObject.SetActive (false);
	}

	private void TargetSelectorCancelled () {
		casc.TargetSelectorCancelled ();
		DisableTargetSelector ();
	}

	private void Move (Vector2 dir) {
		Vector3 currentPosition = transform.localPosition;
		Vector3 targetPosition = new Vector3 (currentPosition.x + dir.x, currentPosition.y + dir.y, 0);

		// dont move beyond the range of this effect
		if (Mathf.Abs (targetPosition.x) > (float)range || Mathf.Abs (targetPosition.y) > (float)range) {
			return;
		}

		transform.localPosition = targetPosition;
	}

	private void TargetSelected () {
		CharacterSheet enemy = combatManager.GetEnemyAtGridPosition (transform.position);
		if (enemy == null) {
			Debug.Log ("No enemy to attack here!");
			return;
		}

		casc.TargetSelected (transform.position);
		DisableTargetSelector ();
	}
}
