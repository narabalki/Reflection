using System;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

public static class DRWebXMLFetcher
{
	#if false
	private static string dr_url = "http://dr.sahajmarg.org/sm_view_dr/show_dr?lang=EN&display_dt=2002-01-01";
	private static string title_tag = "<td width='48%' height='32' align='left' valign='middle'><p><span style='font-size: 20pt; color: #496072; font-weight: bold; font-family:arial, sans-serif'>";
	private static string title_end_tag = "</span>";
	private static string date_tag = "<span style='text-decoration: none; color: #272727; font-size: 14px; color: #46586C; font-weight: bold; font-family:arial, sans-serif'>";
	private static string date_end_tag = "</span>";
	private static string body_tag = "<td colspan='2' align='left' valign='top' style='font:Arial, Helvetica, sans-serif; font-size:16pt; padding-top:5px;color: #000;'>";
	private static string body_end_tag = "</td>";
	private static string footnote_tag = "<em><i>";
	private static string footnote_end_tag = "</i>";
	#else
	private static string title_tag = "<title>";
	private static string title_end_tag = "</title>";
	private static string date_tag = "<display-dt type=\"date\">";
	private static string date_end_tag = "</display-dt>";
	private static string body_tag = "<content>";
	private static string body_end_tag = "</content>";
	private static string footnote_tag = "<footnote>";
	private static string footnote_end_tag = "</footnote>";
	private static string author_tag = "<author>";
	private static string author_end_tag = "</author>";
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

	private static DailyReflection ExtractDR(string lang, string fetchDate, string html) {

		string date, title, message, footnote, author;

		date = fetchDate;
		title = message = footnote = author = "";

		title = GetBetween (html, title_tag, title_end_tag).Trim ();
		message = GetBetween (html, body_tag, body_end_tag).Trim ();
		date = GetBetween (html, date_tag, date_end_tag).Trim ();
		footnote = GetBetween (html, footnote_tag, footnote_end_tag).Trim ();
		author = GetBetween (html, author_tag, author_end_tag).Trim ();

		return new DailyReflection (lang, fetchDate, title, message, footnote, author, new List<string> ());
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

	public static IEnumerator FetchDRFromServer(string fetchDate, string lang, Action<DRFetchContext> callback) {

		WWW download = StartDownload (fetchDate, lang);
		yield return download;

		DRFetchContext context = new DRFetchContext (lang);

		if (!string.IsNullOrEmpty (download.error)) {
			Debug.Log ("Error downloading DR for fetch date: " + fetchDate + " with error: " + download.error);
		} else {
			DailyReflection dr = ExtractDR (lang, fetchDate, download.text);
			if (dr != null) {
				context.drMap [dr.date] = dr;
				context.drDisplayDateMap [fetchDate] = dr.date;
				dr.DebugLogDR ();
			}
		}

		if (callback != null)
			callback (context);
	}
}

