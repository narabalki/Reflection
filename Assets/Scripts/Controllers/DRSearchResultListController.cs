using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;

public class DRSearchResultListController : MonoBehaviour
{
	public GameObject ContentPanel;
	public GameObject DRSearchResultPrefab;
	public ScrollRect scrollRect;
	public List<DailyReflection> searchResults = null;

	public void InitScrollView(List<DailyReflection> _searchResults) {
		
		scrollRect = GetComponent<ScrollRect>();
		searchResults = _searchResults;
		for (int i = 0; i < searchResults.Count; i+= 2) {
			DailyReflection dr1 = searchResults [i];
			DailyReflection dr2 = null;
			if (i + 1 < searchResults.Count)
				dr2 = searchResults [i + 1];
			GameObject resultPairObj = Instantiate(DRSearchResultPrefab) as GameObject;
			DRSearchResultItemController controller = resultPairObj.GetComponent<DRSearchResultItemController> ();
			controller.InitResultPair (dr1, dr2);
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

