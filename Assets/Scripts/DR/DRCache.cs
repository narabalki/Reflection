using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DRCache : MonoBehaviour
{
	public static List<string> DRLanguages = new List<string> { "EN" };
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

	public void OnReceivedDR (DailyReflection dr) {
		if (String.IsNullOrEmpty (dr.error)) {
			drMap [dr.language] [dr.date] = dr;
			drList [dr.language].Add (dr);
		}
	}

	private void CacheRecentDRs () {
		foreach (string lang in DRLanguages) {
			for (int i = 0; i < recentDays; i++) {
				StartCoroutine(DailyReflection.FetchDR (DateTime.Today.ToString ("yyyy-MM-dd"), lang, OnReceivedDR));
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

	public DailyReflection GetLatestDR(string lang="EN") {
		DailyReflection dr = null;
		if (drList.ContainsKey (lang)) {
			if (drList [lang].Count > 0)
				dr = drList [lang] [0];
		}
		return dr;
	}

}

