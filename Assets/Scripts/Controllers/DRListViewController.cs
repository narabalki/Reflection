using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRListViewController : MonoBehaviour {

	public static string scene = ScenesEnum.DRListViewScene.ToString ();
	public GameObject drListObj;
	public DRList drList;

	void Awake () {

		drList = drListObj.GetComponent<DRList> ();
		List<DailyReflection> drs;
		if (DRCache.instance != null) {
			drs = DRCache.instance.FetchSomeDRs ("en");
		} else {
			drs = new List<DailyReflection> ();
		}
		drList.InitScrollView (drs);
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
