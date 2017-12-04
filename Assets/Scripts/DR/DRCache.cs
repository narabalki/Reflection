using System;
using MiniJSON;
using System.IO;
using UnityEngine;
using System.Collections;	
using System.Collections.Generic;

public class DRCache : MonoBehaviour
{
	public static List<string> DRLanguages = new List<string> { "en" };
	public static DRCache instance;
	public Dictionary<string, Dictionary<string, DailyReflection>> drMap; 
	public Dictionary<string, Dictionary<string, string>> drDisplayDateMap;

	public void InitDRMap () {
		drMap = new Dictionary<string, Dictionary<string, DailyReflection>> (); 
		drDisplayDateMap = new Dictionary<string, Dictionary<string, string>> ();
	}

	private void AddEntriesToDRMap(string lang, Dictionary<string, DailyReflection> _drMap) {

		foreach (string date in _drMap.Keys)
			if (!drMap [lang].ContainsKey (date))
				drMap [lang] [date] = _drMap [date];
	}

	private void AddEntriesToDRDisplayDateMap(string lang, Dictionary<string, string> _drDisplayDateMap) {

		foreach (string date in _drDisplayDateMap.Keys)
			if (!drDisplayDateMap [lang].ContainsKey (date))
				drDisplayDateMap [lang] [date] = _drDisplayDateMap [date];
	}

	public void OnReceivedDRs (DRFetchContext ctxt) {
		if (!drMap.ContainsKey (ctxt.lang))
			drMap [ctxt.lang] = ctxt.drMap;
		else
			AddEntriesToDRMap (ctxt.lang, ctxt.drMap);
		if (!drDisplayDateMap.ContainsKey (ctxt.lang))
			drDisplayDateMap [ctxt.lang] = ctxt.drDisplayDateMap;
		else
			AddEntriesToDRDisplayDateMap (ctxt.lang, ctxt.drDisplayDateMap);
	}

	public void FetchInitialDRs() {

		foreach (string lang in DRLanguages) {
			DRFetcher drFetcher = new DRFetcher (this, lang, OnReceivedDRs);
			drFetcher.BeginFetching ();
		}
	}

	void MakeSingleton () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (this);
		}
	}

	void Awake () {
		MakeSingleton ();
	}

	void Start () {
		InitDRMap ();
		FetchInitialDRs ();
	}

	private DailyReflection GetDROfDisplayDate(string lang, string date) {
		
		DailyReflection dr = null;
		if (drMap != null && drMap.ContainsKey (lang) && drMap [lang].ContainsKey (date))
			dr = drMap [lang] [date];
		return dr;
	}

	public DailyReflection GetDROfDate(string lang, string fetchDate) {
		
		DailyReflection dr = null;
		if (drDisplayDateMap != null && drDisplayDateMap.ContainsKey (lang) && drDisplayDateMap [lang].ContainsKey (fetchDate))
			dr = GetDROfDisplayDate (lang, drDisplayDateMap [lang] [fetchDate]);
		return dr;
	}

	public DailyReflection GetLatestDR(string lang) {
		
		return GetDROfDate (lang, DateTime.Today.ToString ("yyyy-MM-dd"));
	}

	public void OnReceivedDROfDate (DRFetchContext ctxt) {
		OnReceivedDRs (ctxt);
		if (ctxt.ctxt != null) {
			Action<DailyReflection> callback = (Action<DailyReflection>)ctxt.ctxt;
			DailyReflection dr = null;
			if (ctxt.drMap.Keys.Count > 0) {
				string date = (new List<string> (ctxt.drMap.Keys)) [0];
				dr = ctxt.drMap [date];
			} 
			if (callback != null)
				callback (dr);
		}
	}

	public void FetchDROfDate(string lang, string fetchDate, Action<DailyReflection> callback) {
		DailyReflection dr = GetDROfDate (fetchDate, lang);
		if (dr != null) {
			if (callback != null)
				callback (dr);
		} else {
			DRFetcher drFetcher = new DRFetcher (this, lang, OnReceivedDROfDate);
			drFetcher.InitContext ((object)callback);
			drFetcher.FetchFromServer (fetchDate);
		}
	}
}

