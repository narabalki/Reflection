using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class SmartTextureLocalization : MonoBehaviour
{
	Sprite originalSprite; 
	public string localizedTextureKey;
	bool gotOriginalSprite = false;

	void Start ()
	{
		SetLocalizedTexture ();	
	}

	void Awake ()
	{
		LanguageManager.Instance.OnChangeLanguage+=OnLanguageChange;
		SetLocalizedTexture ();
	}

	void OnLanguageChange (LanguageManager l)
	{
		if (this != null) {
			SetLocalizedTexture ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnEnable()
	{
		SetLocalizedTexture ();	
	}

	public void SetLocalizedTexture ()
	{
		GetOriginalSprite ();
		Image temp = this.GetComponent<Image> ();
		temp.sprite = localizedTextureKey.GetLocalizedTexture (temp.sprite);
	}

	void GetOriginalSprite ()
	{
		if (!gotOriginalSprite) {
			Sprite currentSprite = this.GetComponent<Sprite> ();
			if (originalSprite == null) {
				originalSprite = currentSprite;
			}
			gotOriginalSprite = true;
		}
	} 


	//WIP - In a Language Manager perhaps? 
//	void UpdateTextures(LanguageManager l)
//	{
//		UI_Button[] buttons = FindObjectsOfType<UI_Button> ();
//		foreach (UI_Button btn in buttons) {
//			btn.SetLocalizedTexture ();
//		}
//
//		UI_Filter[] filters = FindObjectsOfType<UI_Filter> ();
//		foreach (UI_Filter fil in filters) {
//			fil.SetLocalizedTexture ();
//		}
//			
//	 
//		foreach (UI_Phrase phrase in UI_Model.instance.uiPhrasesInArena) {
//			foreach (UI_Word word in phrase.uiWordList) {
//				word.SetLocalizedText();
//			}
//			foreach (UI_PlaceHolder pl in phrase.uiPlaceHolderList) {
//				foreach (UI_Relation rel in pl.uiRelationsIn) {
//					rel.SetLocalizedText();
//				}
//				foreach (UI_Relation rel in pl.uiRelationsOut) {
//					rel.SetLocalizedText();
//				}
//				if (pl.uiWord != null) {
//					pl.uiWord.SetLocalizedTexture ();
//				}
//			}
//		}
//		SmartTextureLocalization[] instances = GameObject.FindObjectsOfType<SmartTextureLocalization> ();
//		foreach (SmartTextureLocalization instance in instances) {
//			instance.SetLocalizedTexture ();
//		}
//
//		SmartTextLocalization[] inst = GameObject.FindObjectsOfType<SmartTextLocalization> ();
//		foreach (SmartTextLocalization instance in inst) {
//			instance.SetLocalizedText ();
//		}
//
//		if (UI_WordPalette.instance.questionVessel.mWord != null) {
//			UI_WordPalette.instance.questionVessel.mWord.picPath = "QuestionWordImage".LocalizeString ();
//		}
//
//	}

	// Use this for initialization
}

