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
		SceneManager.LoadSceneAsync ("DRScene");
		StartCoroutine(DailyReflection.FetchDR ());
	}

	public void OnBackButton()
	{
		SceneManager.LoadSceneAsync ("HomeScene");
	}
}