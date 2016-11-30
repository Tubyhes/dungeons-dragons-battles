using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource audioSource;
	public AudioClip menuOpen;
	public AudioClip menuItemSwitch;
	public AudioClip menuCancel;
	public AudioClip menuItemSelect;
	public AudioClip attack;
	public AudioClip magicMissile;
	public AudioClip heal;
	public AudioClip[] walk;
	public AudioClip characterDeath;

	// Use this for initialization
	void Start () {
	
	}
	
	public void PlayMenuOpen () {
		PlayClip (menuOpen);
	}

	public void PlayMenuItemSwitch () {
		PlayClip (menuItemSwitch);
	}

	public void PlayMenuCancel () {
		PlayClip (menuCancel);
	}

	public void PlayMenuItemSelect () {
		PlayClip (menuItemSwitch);
	}

	public void PlayAttack () {
		PlayClip (attack);
	}

	public void PlayMagicMissile () {
		PlayClip (magicMissile);
	}

	public void PlayHeal () {
		PlayClip (heal);
	}

	public void PlayWalk () {
		PlayClip (walk [(int)Random.Range (0, walk.Length)]);
	}

	public void PlayCharacterDeath () {
		PlayClip (characterDeath);
	}

	private void PlayClip (AudioClip clip) {
		audioSource.clip = clip;
		audioSource.Play ();
	}
}
