using UnityEngine;
using System.Collections;
using SmartLocalization;
using UnityEngine.UI;

public class SmartTextLocalization : MonoBehaviour
{
	public string localizedTextKey;
	bool gotOriginalText = false;

	void OnEnable()
	{
		SetLocalizedText ();	
	}

	public void SetLocalizedText()
	{
		GetOriginalText ();
		Text currentText = this.GetComponent<Text> ();
		string localizedText = localizedTextKey.Localize();

		if (localizedText != null ) {
			currentText.text = localizedText;
		} else {
			currentText.text = localizedTextKey;
		}
	}
	// Use this for initialization
	void Awake ()
	{
		LanguageManager.Instance.OnChangeLanguage+=OnLanguageChange;
		SetLocalizedText ();
	}

	void OnLanguageChange (LanguageManager l)
	{
		SetLocalizedText();
	}

	void GetOriginalText ()
	{
		if (!gotOriginalText) {
			localizedTextKey =  this.gameObject.GetComponent<Text>().text;
			gotOriginalText = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

