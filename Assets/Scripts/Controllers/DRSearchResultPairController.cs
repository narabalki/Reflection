using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DRSearchResultPairController : MonoBehaviour
{
	public GameObject DRSearchResultItemPrefab;
	public List<DailyReflection> resultPair = null;

	public DRSearchResultPairController () {
	}

	public void InitResultPair(List<DailyReflection> drPair) {
		
		resultPair = drPair;
		foreach (DailyReflection dr in resultPair) {
			GameObject resultObj = Instantiate (DRSearchResultItemPrefab) as GameObject;
			DRSearchResultItemController controller = resultObj.GetComponent<DRSearchResultItemController> ();
		}
	}
}

