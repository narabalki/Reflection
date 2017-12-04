using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Xml.Linq;



public class DailyReflection
{
	public string title;
	public string message;
	public string date;
	public string footnote;
	public string author;
	public string language;


	public DailyReflection(string lang, string fetchDate, string _title, string _message, string _footnote, string _author) {
		
		language = lang;
		date = fetchDate;
		title = _title;
		message = _message;
		footnote = _footnote;
		author = _author;
	}



	public void DebugLogDR() {
		
		Debug.Log (language);
		Debug.Log (title);
		Debug.Log (date);
		Debug.Log (message);
		Debug.Log (footnote);
	}
}
