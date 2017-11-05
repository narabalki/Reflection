using UnityEngine;
using System.Collections;

public class ChewAnim : MonoBehaviour
{

	Animator animalAnim;
	Animator panelAnim;
	Animator filterAnim;
	//UI_FilterGroup filterGrp;
	// Use this for initialization

	void Start ()
	{
		animalAnim = GetComponent<Animator> ();
		panelAnim = transform.parent.GetComponent<Animator> ();
		filterAnim = transform.parent.GetChild (0).GetComponent<Animator> ();
		//filterGrp = transform.parent.GetComponent<UI_FilterGroup> ();
	}

	#if false
	public void animate (string trigger)
	{
		if(!trigger.Equals("Chewing"))
			System.Threading.Thread.Sleep(250);
		if (filterGrp != null)
			filterGrp.animState = trigger;
		animalAnim.SetTrigger (trigger);
		if (trigger.Equals ("Down")) {
			filterAnim.SetTrigger ("Down");
			if (panelAnim != null && filterGrp != null && filterGrp.animState.Equals ("Down")) {
				if (trigger.Contains ("mosquito"))
					panelAnim.SetTrigger ("mosPanelDown");
				else
					panelAnim.SetTrigger ("Down");	
			}
		}
	}

	public void OnMonkeyExited (string trigger)
	{
		UI_Arena.instance.ShowLevelCompletionPop ();
	}

	public void animateDown (string trigger)
	{
		animalAnim.SetTrigger (trigger);
	}

	public void OnAnimationComplete() {
		if (filterGrp.animState.Equals ("Pop")) {
			transform.SetAsFirstSibling ();
		}
		else
			transform.SetAsLastSibling ();
	}
	#endif	
}
