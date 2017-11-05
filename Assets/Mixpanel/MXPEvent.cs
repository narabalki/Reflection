using System;
using System.Collections.Generic;
using System.Diagnostics;

public class MXPEvent {

	public string eventName;
	public Dictionary<string , object> properties;
	public Stopwatch timer;

	public MXPEvent(string _eventName){
		this.eventName = _eventName;
		this.properties = new Dictionary<string,object>();
		this.timer = Stopwatch.StartNew ();
	}

	public void AddProperty(string pName,object pValue){
		this.properties [pName] = pValue;
	}

}
