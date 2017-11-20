using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Home_Arena : MonoBehaviour {
	public static Home_Arena instance;
	public static string scene = ScenesEnum.HomeScene.ToString ();

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void OnDRButton() {
		SceneManager.LoadSceneAsync (DR_Arena.scene);
	}
}