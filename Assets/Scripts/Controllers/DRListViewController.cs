using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DRListViewController : MonoBehaviour {

	public static string scene = ScenesEnum.DRListViewScene.ToString ();
	public GameObject drListObj;
	public Image datesImage;
	public Image authorsImage;
	public Image topicsImage;
	public Image favoriteImage;
	public Text datesText;
	public Text authorsText;
	public Text topicsText;
	public Text favoriteText;
	public InputField searchInputField;
	private bool allowEnter;

	private Color GetInactiveImageColor() {
		return new Color32 (225, 40, 113, 255);
	}

	private Color GetActiveImageColor() {
		return new Color32 (193, 9, 81, 255);
	}

	private Color GetInactiveTextColor() {
		return new Color32 (200, 201, 202, 255);
	}

	private Color GetActiveTextColor() {
		return new Color32 (200, 201, 202, 255);
	}

	private void MakeActiveColors(Image img=null, Text text=null) {
		datesImage.color = authorsImage.color = topicsImage.color = favoriteImage.color = GetInactiveImageColor ();
		datesText.color = authorsText.color = topicsText.color = favoriteText.color = GetInactiveTextColor();
		datesText.fontStyle = authorsText.fontStyle = topicsText.fontStyle = favoriteText.fontStyle = FontStyle.Normal;
		if (img != null)
			img.color = GetActiveImageColor ();
		if (text != null) {
			text.color = GetActiveTextColor ();
			text.fontStyle = FontStyle.Normal;
		}
	}

	public void OnDatesButton() {
		List<DailyReflection> drs = DRCache.FetchSomeDRs ("en");
		DRList drList = drListObj.GetComponent<DRList> ();
		drList.InitScrollView (drs);
		MakeActiveColors (datesImage, datesText);
	}

	public void OnAuthorsButton() {
		List<DailyReflection> drs = DRCache.FetchSomeDRs ("en");
		DRList drList = drListObj.GetComponent<DRList> ();
		drList.InitScrollView (drs);
		MakeActiveColors (authorsImage, authorsText);
	}

	public void OnTopicsButton() {
		List<DailyReflection> drs = DRCache.FetchSomeDRs ("en");
		DRList drList = drListObj.GetComponent<DRList> ();
		drList.InitScrollView (drs);
		MakeActiveColors (topicsImage, topicsText);
	}

	public void OnFavoriteButton() {
		List<DailyReflection> drs = DRCache.FetchSomeDRs ("en");
		DRList drList = drListObj.GetComponent<DRList> ();
		drList.InitScrollView (drs);
		MakeActiveColors (favoriteImage, favoriteText);
	}

	public void OnSearchButton() {
		List<DailyReflection> drs = DRCache.FetchSomeDRs ("en", 20);
		DRList drList = drListObj.GetComponent<DRList> ();
		drList.InitScrollView (drs);
		MakeActiveColors ();
	}

	void Awake () {
		OnDatesButton ();
	}

	// Use this for initialization
	void Start () {

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

	public void OnBackButton() {
		SceneManager.LoadSceneAsync (Home_Arena.scene);
	}
}
