using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;

public class DRTimeListController : MonoBehaviour {
	public GameObject ContentPanel;
	public GameObject DRTimeListItemPrefab;
	public VerticalScrollSnap scrollRect;
	public List<string> durations = null;
	public string currentDuration = "";
	public int currentPage = 0;

	private int GetPageOfDuration(string duration) {
		int page = 0;
		for (int i = 0; i < durations.Count; i++) {
			if (durations [i].Equals (duration)) {
				page = i;
				break;
			}
		}
		return page;
	}

	public void InitScrollView(List<string> periods, string currentPeriod) {
		scrollRect = GetComponent<VerticalScrollSnap>();
		periods.Reverse ();
		durations = periods;
		foreach(string period in periods){
			GameObject newYear = Instantiate(DRTimeListItemPrefab) as GameObject;
			DRTimeListItemController controller = newYear.GetComponent<DRTimeListItemController> ();
			controller.Period.text = period;
			newYear.transform.parent = ContentPanel.transform;
			newYear.transform.localScale = Vector3.one;
		}
		scrollRect.GoToScreen (GetPageOfDuration (currentPeriod));
		scrollRect.OnSelectionPageChangedEvent.AddListener (OnTimeChanged);
	}

	// Use this for initialization
	void Start () {
	}

	public void OnTimeChanged(int page) {
		Debug.Log ("OnTimeChanged: " + name + ", page:" + page + ", object:" + durations [page]);
		currentPage = page;
	}

	public string GetCurrentDuration() {
		if (durations == null)
			return "";
		return durations [currentPage];
	}

	// Update is called once per frame
	void Update () {
		
	}
}
