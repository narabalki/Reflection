using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DRSearchResultPairController : MonoBehaviour
{
	public GameObject resultPairObj;
	public GameObject DRSearchResultItem1Prefab;
	//public GameObject DRSearchResultItem2Prefab;
	public List<DailyReflection> resultPair = null;

	public DRSearchResultPairController () {
	}

	public void InitResultPair(List<DailyReflection> drPair) {

		int index = 0;
		resultPair = drPair;
		foreach (DailyReflection dr in resultPair) {
			GameObject resultObj = null;
			//if (index % 2 == 0)
				resultObj = Instantiate (DRSearchResultItem1Prefab) as GameObject;
			//else
			//	resultObj = Instantiate (DRSearchResultItem2Prefab) as GameObject;
			DRSearchResultItemController controller = resultObj.GetComponent<DRSearchResultItemController> ();
			resultObj.transform.parent = resultPairObj.transform;
			resultObj.transform.localScale = Vector3.one;
			//controller.InitResultPair (dr);
		}
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
}

