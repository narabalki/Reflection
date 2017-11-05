using System;
using System.Collections;
using System.Collections.Generic;

public class MXPEventManager {

	static MXPEventManager instance;

	public Dictionary<string,MXPEvent> events;

	private MXPEventManager(){
		events = new Dictionary<string,MXPEvent >();
	}

	public static MXPEventManager GetInstance(){
		if (instance == null)
			instance = new MXPEventManager ();
		return instance;
	}

	public MXPEvent AddEvent(string name,Dictionary<string,object> _properties = null ){
		MXPEvent evnt;
		if (events.ContainsKey (name))
			evnt = events [name];
		else
			evnt = new MXPEvent (name);
		if(_properties!=null)
			foreach(string property in _properties.Keys)
				evnt.AddProperty(property,_properties[property]);
		events [name] = evnt;
		return evnt;
	}

	public void SendEvent(string name,Dictionary<string,object> _properties = null ){
		MXPEvent evnt;
		if (events.ContainsKey (name)) {
			evnt = events [name];
			events.Remove(name);
		}else
			evnt = new MXPEvent (name);
		evnt.timer.Stop ();
		if(_properties!=null)
			foreach(string property in _properties.Keys)
				evnt.AddProperty(property,_properties[property]);
		TimeSpan span = evnt.timer.Elapsed;
		evnt.AddProperty ("Total time taken", span.Minutes*60 + span.Seconds);
		Mixpanel.instance.SendEvent (evnt.eventName, evnt.properties);
	}

	public void SendEvent(string tempName,string name,Dictionary<string,object> _properties = null){
		MXPEvent evnt;
		if (events.ContainsKey (tempName)) {
			evnt = events [tempName];
			events.Remove(tempName);
		}else
			evnt = new MXPEvent (name);
		evnt.eventName = name;
		evnt.timer.Stop ();
		if(_properties!=null)
			foreach(string property in _properties.Keys)
				evnt.AddProperty(property,_properties[property]);
		TimeSpan span = evnt.timer.Elapsed;
		evnt.AddProperty ("Total time taken", span.Minutes*60 + span.Seconds);
		Mixpanel.instance.SendEvent (evnt.eventName, evnt.properties);
	}

	public void RemoveEvent(string name){
		events.Remove (name);
	}

	public void ClearAllEvents(){
		events.Clear ();
	}

}
