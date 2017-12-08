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
	public List<string> tags;

	public DailyReflection(string lang, string fetchDate, string _title, string _message, string _footnote, string _author, List<string> _tags) {
		
		language = lang;
		date = fetchDate;
		title = _title;
		message = _message;
		footnote = _footnote;
		author = _author.Trim ();
		tags = _tags;
	}

	public void DebugLogDR() {
		
		Debug.Log (language);
		Debug.Log (title);
		Debug.Log (date);
		Debug.Log (message);
		Debug.Log (footnote);
	}
}
