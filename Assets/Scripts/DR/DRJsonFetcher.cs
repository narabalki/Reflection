using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DRJsonFetcher
{
	private static string DRJSONBaseFolderPath = "/drs/";
	private static string drCacheFileName = "dr_cache.json";
	private static string drDisplayDateMapFileName = "dr_display_date_map.json";

	private static string TryGetValue(Dictionary<string, object> dictObj, string key) {

		string value = "";
		if (dictObj.ContainsKey (key))
			value = (string)dictObj [key];
		return value;
	}

	private static DailyReflection ExtractDR(object dictObj) {

		Dictionary<string, object> dict = (Dictionary<string, object>)dictObj;
		string date, title, message, footnote, author, lang;

		author = TryGetValue (dict, "author");
		lang = TryGetValue (dict, "language").ToLower ();
		footnote = TryGetValue (dict, "footnote");
		message = TryGetValue (dict, "content");
		date = TryGetValue (dict, "display-dt");
		title = TryGetValue (dict, "title");

		return new DailyReflection (lang, date, title, message, footnote, author);
	}


	private static string DRJSONFolderPath(string lang) {
		return DRJSONBaseFolderPath + lang.ToLower () + "/";
	}

	private static string DRJSONRelativePath(string lang, string jsonFile) {
		
		return DRJSONFolderPath(lang) + jsonFile;
	}

	private static void CopyDRJSONFiles(string lang) {

		try {
			string drJSONBaseFolder = ExtensionMethods.AppWritableDirectory () + DRJSONBaseFolderPath;
			if (!Directory.Exists(drJSONBaseFolder))
				Directory.CreateDirectory(drJSONBaseFolder);
			string drJSONFolder = ExtensionMethods.AppWritableDirectory () + DRJSONFolderPath(lang);
			if (!Directory.Exists(drJSONFolder))
				Directory.CreateDirectory(drJSONFolder);
		} catch (IOException ex) {
			Debug.Log (ex.Message);
		}
		
		string relPath = DRJSONRelativePath (lang, drCacheFileName);
		string drCachePath = ExtensionMethods.AppWritableDirectory () + relPath;
		if (!File.Exists (drCachePath)) {
			File.Copy (Application.streamingAssetsPath + relPath, drCachePath);
		}

		relPath = DRJSONRelativePath (lang, drDisplayDateMapFileName);
		string drDisplayDateMapPath = ExtensionMethods.AppWritableDirectory () + relPath;
		if (!File.Exists (drDisplayDateMapPath)) {
			File.Copy (Application.streamingAssetsPath + relPath, drDisplayDateMapPath);
		}
	}

	private static Dictionary<string, DailyReflection> FetchDRMapFromJson(string lang) {

		Dictionary<string, DailyReflection> drMap = new Dictionary<string, DailyReflection> ();
		string drCachePath = ExtensionMethods.AppWritableDirectory () + DRJSONRelativePath (lang, drCacheFileName);

		JsonHelper drJson = new JsonHelper ();
		Dictionary<string, object> allDRObjects = drJson.ReadAndDeserializeJson (drCachePath, false);

		foreach (string date in new List<string>(allDRObjects.Keys)) {
			DailyReflection dr = ExtractDR (allDRObjects [date]);
			drMap [date] = dr;
		}

		return drMap;
	}

	private static Dictionary<string, string> FetchDRDisplayDateMap(string lang) {

		Dictionary<string, string> drDisplayDateMap = new Dictionary<string, string> ();
		string drDisplayDateMapPath = ExtensionMethods.AppWritableDirectory () + DRJSONRelativePath (lang, drDisplayDateMapFileName);

		JsonHelper drJson = new JsonHelper ();
		Dictionary<string, object> allDRObjects = drJson.ReadAndDeserializeJson (drDisplayDateMapPath, false);

		foreach (string date in new List<string>(allDRObjects.Keys)) {
			drDisplayDateMap [date] = (string)(allDRObjects [date]);
		}

		return drDisplayDateMap;
	}

	public static IEnumerator FetchDRsFromJson(string lang, Action<DRFetchContext> callback) {

		CopyDRJSONFiles (lang);
		yield return null;

		DRFetchContext context = new DRFetchContext (lang);
		context.ctxt = null;
		context.drMap = FetchDRMapFromJson (lang);
		context.drDisplayDateMap = FetchDRDisplayDateMap (lang);

		if (callback != null)
			callback (context);
	}
}

