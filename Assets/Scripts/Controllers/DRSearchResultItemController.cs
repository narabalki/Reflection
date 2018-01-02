using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DRSearchResultItemController : MonoBehaviour
{
	public GameObject itemObject;
	public GameObject panel1;
	public GameObject panel2;
	public Text Title1;
	public Text Date1;
	public Text Message1;
	public Text Title2;
	public Text Date2;
	public Text Message2;

	public DRSearchResultItemController () {
	}

	public void InitResultPair(DailyReflection dr1, DailyReflection dr2) {
		itemObject.SetActive (false);
		if (dr1 != null) {
			Title1.text = dr1.title;
			Date1.text = dr1.date;
			Message1.text = dr1.message;
			panel1.SetActive (true);
		} else {
			panel1.SetActive (false);
		}
		if (dr2 != null) {
			Title2.text = dr2.title;
			Date2.text = dr2.date;
			Message2.text = dr2.message;
			panel2.SetActive (true);
		} else {
			panel2.SetActive (false);
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

