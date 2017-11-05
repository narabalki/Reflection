using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class UI_Arena : MonoBehaviour
{
	public static UI_Arena instance;

	public SentenceBox sentenceBox;

	//public Animator birdAnimator;
	//public Animator monkeyAnimator;

	void Awake ()
	{
		instance = this;
	}


	void Start ()
	{
	}

	public void OnDRButton()
	{
		Application.LoadLevel(1);
	}

	public void OnBackButton()
	{
		Application.LoadLevel(0);
	}
}