using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRTimeListController : MonoBehaviour {
	public GameObject ContentPanel;
	public GameObject DRTimeListItemPrefab;

	private static List<string> Years = new List<string> () {
		"2002", "2003", "2004", "2005",
		"2006", "2007", "2008", "2009", 
		"2010", "2011", "2012", "2013",
		"2014", "2015", "2016", "2017"
	};
	private static List<string> Months = new List<string> () {
		"Jan", "Feb", "Mar", "Apr",
		"May", "June", "July", "Aug",
		"Sep", "Oct", "Nov", "Dec"
	};
	private static List<string> Dates = new List<string> () {
		"1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
		"11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
		"21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"
	};

	private void InitScrollView(List<string> periods) {
		foreach(string period in periods){
			GameObject newYear = Instantiate(DRTimeListItemPrefab) as GameObject;
			DRTimeListItemController controller = newYear.GetComponent<DRTimeListItemController> ();
			controller.Period.text = period;
			newYear.transform.parent = ContentPanel.transform;
			newYear.transform.localScale = Vector3.one;
		}
	}

	// Use this for initialization
	void Start () {
		if (name.Equals ("Year Scroll View"))
			InitScrollView (Years);
		else if (name.Equals ("Month Scroll View"))
			InitScrollView (Months);
		else
			InitScrollView (Dates);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
