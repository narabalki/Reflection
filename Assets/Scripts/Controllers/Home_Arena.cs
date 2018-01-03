using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Home_Arena : MonoBehaviour {
	public static Home_Arena instance;
	public static string scene = ScenesEnum.HomeScene.ToString ();
	public InputField searchInputField;
	private bool allowEnter;

	// Use this for initialization
	void Start () {
		
		ExtensionMethods.InitDataPaths ();
	}

	public void OnSearchButton() {
		
		SceneManager.LoadSceneAsync (DRListViewController.scene);
	}

	// Update is called once per frame
	void Update () {
		
		if (allowEnter && (searchInputField.text.Length > 0) && (Input.GetKey (KeyCode.Return) || Input.GetKey (KeyCode.KeypadEnter))) {
			OnSearchButton ();
			allowEnter = false;
		} else {
			allowEnter = searchInputField.isFocused;
		}
	}

	public void OnDRButton() {
		
		SceneManager.LoadSceneAsync (DRListViewController.scene);
	}
}