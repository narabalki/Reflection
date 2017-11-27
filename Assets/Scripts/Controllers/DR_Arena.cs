using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DR_Arena : MonoBehaviour {
	public static DR_Arena instance;
	public static string scene = ScenesEnum.DRScene.ToString ();
	public DRPanel drPanel;
	public DRDatePicker datePicker;

	void Awake () {
		drPanel.InitDRPanel ();
		datePicker.Initialize (drPanel);
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	public void OnBackButton() {
		SceneManager.LoadSceneAsync (Home_Arena.scene);
	}
}
