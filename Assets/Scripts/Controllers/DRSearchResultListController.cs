using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;

public class DRSearchResultListController : MonoBehaviour
{
	public GameObject ContentPanel;
	public GameObject DRSearchResultPairPrefab;
	public ScrollRect scrollRect;
	public List<DailyReflection> searchResults = null;

	public void InitScrollView(List<DailyReflection> _searchResults, string currentPeriod) {
		scrollRect = GetComponent<ScrollRect>();
		searchResults = _searchResults;
		for (int i = 0; i < searchResults.Count; i+= 2) {
			List<DailyReflection> resultPair = new List<DailyReflection> ();
			resultPair.Add (searchResults [i]);
			if (i + 1 < searchResults.Count)
				resultPair.Add (searchResults [i + 1]);
			GameObject resultPairObj = Instantiate(DRSearchResultPairPrefab) as GameObject;
			DRSearchResultPairController controller = resultPairObj.GetComponent<DRSearchResultPairController> ();
			controller.resultPair = resultPair;
			resultPairObj.transform.parent = ContentPanel.transform;
			resultPairObj.transform.localScale = Vector3.one;
		}
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
}

