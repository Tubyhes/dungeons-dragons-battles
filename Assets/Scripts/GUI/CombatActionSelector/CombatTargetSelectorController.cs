using UnityEngine;
using System.Collections;

public class CombatTargetSelectorController : MonoBehaviour {

	public CombatActionSelectorController casc;

	private int range;
	private int aoe;
	private bool targetRequired;
	private CombatManager combatManager;
	private SoundManager soundManager;
	private bool ignoreFirstClick = true;

	void Awake () {
		gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		combatManager = FindObjectOfType (typeof(CombatManager)) as CombatManager;
		soundManager = FindObjectOfType (typeof(SoundManager)) as SoundManager;
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
		if (Input.GetButtonDown (Helpers.Fire2(casc.combatant.team))) {
			soundManager.PlayMenuCancel ();
			TargetSelectorCancelled ();
			return;
		}

		// listen to select action
		if (Input.GetButtonDown (Helpers.Fire1(casc.combatant.team))) {
			TargetSelected ();
			return;
		}

		// listen to horizontal / vertical input
		Vector2 dir = AxisButtonDownGetter.GetInput (casc.combatant.team);
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
		if (Mathf.Abs (targetPosition.x) + Mathf.Abs (targetPosition.y) > (float)range) {
			return;
		}

		soundManager.PlayMenuItemSelect ();
		transform.localPosition = targetPosition;
	}

	private void TargetSelected () {
		if (targetRequired) {
			CharacterSheet c = combatManager.GetCombatantAtGridPosition (transform.position);
			if (c == null) {
				Debug.Log ("No character to target here!");
				return;
			}
		}

		soundManager.PlayMenuItemSelect ();
		casc.TargetSelected (transform.position);
		DisableTargetSelector ();
	}
}
