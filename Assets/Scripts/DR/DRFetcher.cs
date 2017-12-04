using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DRFetchContext
{
	public string lang;
	public Dictionary<string, DailyReflection> drMap;
	public Dictionary<string, string> drDisplayDateMap;
	public System.Object ctxt;

	public DRFetchContext (string _lang) {
		lang = _lang;
		drMap = new Dictionary<string, DailyReflection> ();
		drDisplayDateMap = new Dictionary<string, string> ();
		ctxt = null;
	}
}

public class DRFetcher
{
	public string language;
	public Action<DRFetchContext> callback;
	public DRFetchContext context;
	public DRCache cacheHandle;
	private int recentDays = 1;

	public DRFetcher (DRCache _cacheHandle, string _lang, Action<DRFetchContext> _callback) {

		cacheHandle = _cacheHandle;
		language = _lang;
		callback = _callback;
		context = null;
	}

	public void InitContext(System.Object ctxt) {
		
		context = new DRFetchContext (language);
		context.ctxt = ctxt;
	}

	public void OnDRFetched(DRFetchContext ctxt) {

		DRFetchContext retContext = ctxt;

		if (context != null) {
			foreach (string date in ctxt.drMap.Keys) {
				if (!context.drMap.ContainsKey (date))
					context.drMap [date] = ctxt.drMap [date];
			}

			foreach (string date in ctxt.drDisplayDateMap.Keys) {
				if (!context.drDisplayDateMap.ContainsKey (date))
					context.drDisplayDateMap [date] = ctxt.drDisplayDateMap [date];
			}

			retContext = context;
		}

		if (callback != null)
			callback (retContext);
	}

	public void FetchFromServer(string fetchDate) {
		
		cacheHandle.StartCoroutine (DRWebXMLFetcher.FetchDRFromServer (fetchDate, language, OnDRFetched));
	}

	private void CacheRecentDRs () {
		for (int i = 0; i < recentDays; i++) {
			string today = DateTime.Today.ToString ("yyyy-MM-dd");
			if (!context.drDisplayDateMap.ContainsKey (today)) {
				FetchFromServer (today);
			}
		}
	}

	public void OnDRsFetchedFromJSON (DRFetchContext ctxt) {
		
		context = ctxt;
		CacheRecentDRs ();
	}

	public void BeginFetching() {
		
		cacheHandle.StartCoroutine (DRJsonFetcher.FetchDRsFromJson (language, OnDRsFetchedFromJSON));
	}
}

