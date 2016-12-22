using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource audioSource;

	// gui sounds
	public AudioClip menuOpen;
	public AudioClip menuItemSwitch;
	public AudioClip menuCancel;
	public AudioClip menuItemSelect;

	// combat sounds
	public AudioClip attack;
	public AudioClip[] walk;
	public AudioClip characterDeath;

	// spell sounds
	public AudioClip magicMissile;
	public AudioClip fire;
	public AudioClip heal;
	public AudioClip bless;
	public AudioClip shield;
	public AudioClip fear;

	// Use this for initialization
	void Start () {
	
	}


	// GUI sounds
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

	// Combat sounds
	public void PlayAttack () {
		PlayClip (attack);
	}

	public void PlayWalk () {
		PlayClip (walk [(int)Random.Range (0, walk.Length)]);
	}

	public void PlayCharacterDeath () {
		PlayClip (characterDeath);
	}

	// Spell sounds
	public void PlayMagicMissile () {
		PlayClip (magicMissile);
	}

	public void PlayFire () {
		PlayClip (fire);
	}

	public void PlayShield () {
		PlayClip (shield);
	}

	public void PlayBless () {
		PlayClip (bless);
	}

	public void PlayHeal () {
		PlayClip (heal);
	}

	public void PlayFear () {
		PlayClip (fear);
	}



	private void PlayClip (AudioClip clip) {
		audioSource.clip = clip;
		audioSource.Play ();
	}
}
