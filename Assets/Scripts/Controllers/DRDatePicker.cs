using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DRDatePicker : MonoBehaviour {
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
	public DRPanel drPanel = null;
	public DRTimeListController drYearController = null;
	public DRTimeListController drMonthController = null;
	public DRTimeListController drDateController = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Initialize(DRPanel _drPanel) {
		
		drPanel = _drPanel;
		drYearController.InitScrollView (Years, DateTime.Today.Year.ToString ());
		drMonthController.InitScrollView (Months, "Nov");
		drDateController.InitScrollView (Dates, DateTime.Today.Day.ToString ());
	}

	private string GetCurrentYear() {
		return drYearController.GetCurrentDuration ();
	}

	private string GetCurrentMonth() {
		return drMonthController.GetCurrentDuration ();
	}

	private string GetCurrentDate() {
		return drDateController.GetCurrentDuration ();
	}

	private string GetCurrentDateTimeString() {
		string datestring = drYearController.GetCurrentDuration ();
		if (!string.IsNullOrEmpty (datestring)) {
			datestring += "-" + (drMonthController.durations.Count - drMonthController.currentPage).ToString ("00");
			datestring += "-" + drDateController.GetCurrentDuration ().PadLeft (2, '0');
		}
		return datestring;
	}

	public void OnFetchButton() {
		if (DRCache.instance != null)
			DRCache.instance.FetchDROfDate ("en", GetCurrentDateTimeString (), drPanel.UpdateDRPanel);
	}
}
