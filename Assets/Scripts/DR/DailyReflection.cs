using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

public class DailyReflection : MonoBehaviour {
	public static string dr_url = "http://dr.sahajmarg.org/sm_view_dr/show_dr?lang=EN&display_dt=2002-01-01";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static IEnumerator FetchDR() {
		Dictionary<string, string> headerInfo = new Dictionary<string, string>();
		WWW download = new WWW (dr_url, null, headerInfo);
		yield return download;
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			// show the highscores
			Debug.Log(download.text);
			#if false
			XElement root = XElement.Parse (download.text);
			var nodes = root.Descendants("meta")
				.Select(y => y.Descendants()
					.Where(x => x.Attributes["property"].Value == "og:title"))
				.ToList();
			string title = string.Join (" ", nodes.ToArray ());
			Debug.Log(title);
			#endif
		}
	}
}
