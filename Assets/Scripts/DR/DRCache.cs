using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DRCache : MonoBehaviour
{
	public static List<string> DRLanguages = new List<string> { "en" };
	public static DRCache instance;
	public Dictionary<string, List<DailyReflection>> drList;
	public Dictionary<string, Dictionary<string, DailyReflection>> drMap; 
	private int recentDays = 1;

	public void InitDRMap () {
		drMap = new Dictionary<string, Dictionary<string, DailyReflection>> ();
		drList = new Dictionary<string, List<DailyReflection>> ();
		foreach (string lang in DRLanguages) {
			drMap [lang] = new Dictionary<string, DailyReflection> ();
			drList [lang] = new List<DailyReflection> ();
		}
	}

	public void CacheDROfDate(string date, string lang, Action<DRFetchContext> callback, System.Object context=null) {
		DRFetchContext ctxt = new DRFetchContext ();
		ctxt.context = context;
		StartCoroutine (DailyReflection.FetchDR (date, lang, callback, ctxt));
	}

	public void OnReceivedDR (DRFetchContext ctxt) {
		DailyReflection dr = ctxt.dr;
		if (String.IsNullOrEmpty (dr.error)) {
			drMap [dr.language] [dr.date] = dr;
			drList [dr.language].Add (dr);
		}
	}

	private void CacheRecentDRs () {
		foreach (string lang in DRLanguages) {
			for (int i = 0; i < recentDays; i++) {
				CacheDROfDate (DateTime.Today.ToString ("yyyy-MM-dd"), lang, OnReceivedDR);
			}
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
		CacheRecentDRs ();
	}

	public DailyReflection GetLatestDR(string lang="e") {
		DailyReflection dr = null;
		if (drList.ContainsKey (lang)) {
			if (drList [lang].Count > 0)
				dr = drList [lang] [0];
		}
		return dr;
	}

	public DailyReflection GetDROfDate(string fetchDate = "", string lang="en") {
		DailyReflection dr = null;
		if (fetchDate == "")
			fetchDate = DateTime.Today.ToString ("yyyy-MM-dd");
		if (drList.ContainsKey (lang)) {
			if (drList [lang].Count > 0)
				dr = drList [lang] [0];
		}
		return dr;
	}

	public void OnReceivedDROfDate (DRFetchContext ctxt) {
		OnReceivedDR (ctxt);
		if (ctxt.context != null) {
			Action<DailyReflection> callback = (Action<DailyReflection>)ctxt.context;
			callback (ctxt.dr);
		}
	}

	public void FetchDROfDate(string fetchDate="", string lang="en", Action<DailyReflection> callback=null) {
		DailyReflection dr = GetDROfDate (fetchDate, lang);
		if (dr == null) {
			if (callback != null)
				callback (dr);
		} else {
			CacheDROfDate (fetchDate, lang, OnReceivedDROfDate, (object)callback);
		}
	}
}

