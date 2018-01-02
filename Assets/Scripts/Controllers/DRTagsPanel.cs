using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class DRTagsPanel : MonoBehaviour {
	public GameObject drTagsPanelObject;
	public GameObject tag1Object;
	public GameObject tag2Object;
	public GameObject tag3Object;
	public GameObject authorTagObject;
	public Text Tag1;
	public Text Tag2;
	public Text Tag3;
	public Text AuthorTag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDRTagsPanel (DailyReflection dr) {
		
		drTagsPanelObject.SetActive (false);
		tag1Object.SetActive (false);
		tag2Object.SetActive (false);
		tag3Object.SetActive (false);
		authorTagObject.SetActive (false);
		if (dr.tags.Count > 0) {
			Tag1.text = dr.tags [0];
			tag1Object.SetActive (true);
		}
		if (dr.tags.Count > 1) {
			Tag2.text = dr.tags [1];
			tag2Object.SetActive (true);
		}
		if (dr.tags.Count > 2) {
			Tag3.text = dr.tags [2];
			tag3Object.SetActive (true);
		}
		if (!string.IsNullOrEmpty (dr.author)) {
			AuthorTag.text = dr.author;
			authorTagObject.SetActive (true);
		}
		drTagsPanelObject.SetActive (true);
	}
}
