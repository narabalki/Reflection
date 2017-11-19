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
	public string author;
	public string language;
	public string html;
	public string error;

	#if false
	private static string dr_url = "http://dr.sahajmarg.org/sm_view_dr/show_dr?lang=EN&display_dt=2002-01-01";
	private static string title_tag = "<td width='48%' height='32' align='left' valign='middle'><p><span style='font-size: 20pt; color: #496072; font-weight: bold; font-family:arial, sans-serif'>";
	private static string title_end_tag = "</span>";
	private static string date_tag = "<span style='text-decoration: none; color: #272727; font-size: 14px; color: #46586C; font-weight: bold; font-family:arial, sans-serif'>";
	private static string date_end_tag = "</span>";
	private static string body_tag = "<td colspan='2' align='left' valign='top' style='font:Arial, Helvetica, sans-serif; font-size:16pt; padding-top:5px;color: #000;'>";
	private static string body_end_tag = "</td>";
	private static string author_tag = "<em><i>";
	private static string author_end_tag = "</i>";
	#else
	private static string title_tag = "<title>";
	private static string title_end_tag = "</title>";
	private static string date_tag = "<display-dt type=\"date\">";
	private static string date_end_tag = "</display-dt>";
	private static string body_tag = "<content>";
	private static string body_end_tag = "</content>";
	private static string author_tag = "<footnote>";
	private static string author_end_tag = "</footnote>";
	private static string dr_web_service = "http://dr.sahajmarg.org/sm_view_dr/show_xml";
	#endif

	private static string GetBetween(string strSource, string strStart, string strEnd) {
		
		int start, end;
		string substring = "";
		if (strSource.Contains (strStart) && strSource.Contains (strEnd)) {
			start = strSource.IndexOf (strStart, 0) + strStart.Length;
			end = strSource.IndexOf (strEnd, start);
			substring = strSource.Substring (start, end - start);
		}
		return substring;
	}

	public DailyReflection(string lang, string fetchDate, string _html, string _error) {
		
		language = lang;
		date = fetchDate;
		html = _html;
		title = message = author = error = "";
		if (!string.IsNullOrEmpty (_error)) {
			Debug.Log ("Error downloading DR for fetch date: " + fetchDate + " with error: " + _error);
			error = _error;
		} else {
			title = GetBetween (html, title_tag, title_end_tag).Trim ();
			message = GetBetween (html, body_tag, body_end_tag).Trim ();
			date = GetBetween (html, date_tag, date_end_tag).Trim ();
			author = GetBetween (html, author_tag, author_end_tag).Trim ();
		}
	}

	private void DebugLogDR() {
		
		Debug.Log (language);
		Debug.Log (title);
		Debug.Log (date);
		Debug.Log (message);
		Debug.Log (author);
		Debug.Log (error);
	}

	private static string GetDRURL(string fetchDate, string lang) {
		
		string url = dr_web_service;
		string delim = "?";
		if (!fetchDate.Equals ("")) {
			url = url + delim + "display_dt=" + fetchDate;
			delim = "&";
		}
		url = url + delim + "lang=" + lang;
		return url;
	}

	private static WWW StartDownload(string fetchDate, string lang) {
		
		Dictionary<string, string> headerInfo = new Dictionary<string, string> ();
		WWW download = new WWW (GetDRURL (fetchDate, lang), null, headerInfo);
		return download;
	}

	public static IEnumerator FetchDR(string fetchDate="", string lang="EN", Action<DailyReflection> callback=null) {
		
		WWW download = StartDownload (fetchDate, lang);
		yield return download;
		DailyReflection dr = new DailyReflection (lang, fetchDate, download.text, download.error);
		dr.DebugLogDR ();
		if (callback != null)
			callback (dr);
	}
}
