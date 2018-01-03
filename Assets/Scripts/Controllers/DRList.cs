using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;

public class DRList : MonoBehaviour
{
	public GameObject ContentPanel;
	public GameObject DRListItemPrefab;
	public ScrollRect scrollRect;
	public List<DailyReflection> drList = null;

	public void InitScrollView(List<DailyReflection> _drList) {

		scrollRect = GetComponent<ScrollRect>();
		drList = _drList;
		foreach (DailyReflection dr in drList) {
			GameObject drObj = Instantiate(DRListItemPrefab) as GameObject;
			DRListItem controller = drObj.GetComponent<DRListItem> ();
			controller.InitListItem (dr);
			drObj.transform.parent = ContentPanel.transform;
			drObj.transform.localScale = Vector3.one;
		}
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
}

