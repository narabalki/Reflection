using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DRListItem : MonoBehaviour
{
	public GameObject itemObject;
	public GameObject panel1;
	public Text Title1;
	public Text Date1;
	public Text Message1;

	public DRListItem () {
	}

	public void InitListItem(DailyReflection dr1) {
		itemObject.SetActive (false);
		if (dr1 != null) {
			Title1.text = dr1.title;
			Date1.text = dr1.date;
			Message1.text = dr1.message;
			panel1.SetActive (true);
		} else {
			panel1.SetActive (false);
		}
		itemObject.SetActive (true);
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
}

