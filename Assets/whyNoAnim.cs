using UnityEngine;
using System.Collections;

public class whyNoAnim : MonoBehaviour {

	Animator anim;
	bool animating = true;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void onClick(string trigger) {
		Debug.Log("Sprite Clicked");

		anim.SetTrigger ("Down");
		if (animating)
			anim.SetTrigger (trigger);
		
		animating = !animating;
	}

}
