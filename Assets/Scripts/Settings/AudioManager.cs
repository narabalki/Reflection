using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	void MakeSingleton ()
	{
		DontDestroyOnLoad (this.gameObject);
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}

	void Awake () {
		MakeSingleton ();
	}

	public AudioSource myAudioSource;

	public AudioClip audio_challengeWon;
	public AudioClip audio_WordDrag;

	public AudioClip audio_Button;

	public AudioClip audio_FilterButton;
	public AudioClip audio_Swoosh;

	public AudioClip audio_WordDrop;

	public AudioClip audio_ReplaceWord;

	public AudioClip audio_MainCategory;
	public AudioClip audio_SubCategory;

	public AudioClip audio_ToggleButton;

	public AudioClip audio_ChallengeWon;
	public AudioClip audio_ChallengeLost;

	public AudioClip audio_DeletePhrase;

	public void PlayAudio_Button () {
		PlayAudio (audio_Button);
	}

	public void PlayAudio_FilterButton () {
		PlayAudio (audio_FilterButton);
	}

	public void PlayAudio_Swoosh () {
		PlayAudio (audio_Swoosh);
	}

	public void PlayAudio_WordDrag () {
		PlayAudio (audio_WordDrag);
	}

	public void PlayAudio_WordDrop () {
		PlayAudio (audio_WordDrop);
	}

	public void PlayAudio_RelpaceWord () {
		PlayAudio (audio_ReplaceWord);
	}

	public void PlayAudio_MainCategory () {
		PlayAudio (audio_MainCategory);
	}

	public void PlayAudio_SubCategory () {
		PlayAudio (audio_SubCategory);
	}

	public void PlayAudio_ToggleButton () {
		PlayAudio (audio_ToggleButton);
	}

	public void PlayAudio_ChallengeWon () {
		PlayAudio (audio_ChallengeWon);
	}

	public void PlayAudio_ChallengeLost () {
		PlayAudio (audio_ChallengeLost);
	}

	public void PlayAudio_DeletePhrase () {
		PlayAudio (audio_DeletePhrase);
	}

	void PlayAudio (AudioClip audio) {
		if (Settings.instance.CanPlaySFX ()) {
			myAudioSource.PlayOneShot (audio);
		}
	}
}