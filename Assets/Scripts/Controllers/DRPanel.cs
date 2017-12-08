using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DRPanel : MonoBehaviour {
	public GameObject drPanelObject;
	public Text title;
	public Text date;
	public Text message;
	public Text footnote;

	public DRTagsPanel tagsPanel = null;

	public DailyReflection currentDR;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDRPanel (DailyReflection dr) {
		currentDR = dr;
		drPanelObject.SetActive (false);
		title.text = currentDR.title;
		date.text = currentDR.date;
		message.text = currentDR.message;
		footnote.text = currentDR.footnote;
		tagsPanel.UpdateDRTagsPanel (dr);
		drPanelObject.SetActive (true);
	}

	public void InitDRPanel (DRTagsPanel _tagsPanel) {
		tagsPanel = _tagsPanel;
		if (DRCache.instance != null) {
			UpdateDRPanel (DRCache.instance.GetLatestDR ("en"));
		}
	}
}
