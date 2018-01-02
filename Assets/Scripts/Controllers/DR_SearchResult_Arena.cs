using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DR_SearchResult_Arena : MonoBehaviour {

	public static string scene = ScenesEnum.DRSearchResultsScene.ToString ();
	public GameObject searchResults;
	public DRSearchResultListController searchResultList;

	void Awake () {

		searchResultList = searchResults.GetComponent<DRSearchResultListController> ();
		List<DailyReflection> drlist;
		if (DRCache.instance != null) {
			drlist = DRCache.instance.FetchSomeDRs ("en");
		} else {
			drlist = new List<DailyReflection> ();
		}
		searchResultList.InitScrollView (drlist);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
