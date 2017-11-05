using UnityEngine;
using System.Collections;

public class AnimateTiledTexture : MonoBehaviour
{
	[SerializeField] bool TEST = false;

	[SerializeField] bool isAffectedByPause = true;
	[SerializeField] bool isPaused = false;
	[SerializeField] bool animate = false;
	[SerializeField] bool animateBackward = false;

	[SerializeField] Renderer myRend;

	[SerializeField] int columns = 2;
	[SerializeField] int rows = 2;
	[SerializeField] float framesPerSecond = 10f;

	[SerializeField] bool loop = true;
	[SerializeField] int numberOfCycles = 1;

	//the current frame to display
	public int totalFrames;
	[SerializeField] int currentFrame = 0;
	[SerializeField] int cycles = 0;
	
	[SerializeField] float frameDelay = 0; //(1f / framesPerSecond);
	[SerializeField] float animateTimer = 0;
	
	void Start()
	{
		//SetUp ();
		//AnimateTexture ();
	}

	void SetUp ()
	{
		//myRend.material = new Material (myRend.material);
		frameDelay = (1f / framesPerSecond);

		//set the tile size of the texture (in UV units), based on the rows and columns
		Vector2 size = new Vector2 (1f / columns, 1f / rows);
		myRend.material.SetTextureScale ("_MainTex", size);

		totalFrames = rows * columns;
	}

	public int GetTotalFrames () {
		int tot_frms = rows * columns;
		//Debug.Log ("TOTAL FRAMES :" + tot_frms);
		return tot_frms;
	}

	public void AnimateForward () {
		SetUp ();
		animate = true;
		animateBackward = false;
		currentFrame = 0;
	}

	public void AnimateBackward () {

		SetUp ();
		animate = true;
		animateBackward = true;
		currentFrame = totalFrames - 1;
	}

	public void StopAnimating (bool restoreToFirstFrame = false) {
		animate = false;

		if (restoreToFirstFrame) {
			if (animateBackward)
				currentFrame = totalFrames - 1;
			else
				currentFrame = 0;

			ChangeFrame (currentFrame);
		}
	}
	
	Vector2 FindOffset (int frame)
	{
		Vector2 offSet = new Vector2 (	( (float)frame / columns - (frame / columns) )				, 		//x index
		                              ( (1f - 1f / (float)rows) - ( (frame / columns) / (float)rows ) )	);	//y index
		return offSet;
	}

	public void ChangeFrame (int frame)
	{
		//Debug.Log (name + ", Frame :" + frame);
		//split into x and y indexes
		Vector2 offset = FindOffset (frame);
		myRend.material.SetTextureOffset ("_MainTex", offset);
	}

	void AnimateSpriteForward ()
	{
		if (!isAffectedByPause || !isPaused)
		{
			animateTimer += Time.deltaTime;
			
			if(animateTimer >= frameDelay)
			{
				//move to the next index
				currentFrame++;
				if (currentFrame >= rows * columns) { currentFrame = 0; }
				
				ChangeFrame (currentFrame);
				
				animateTimer = 0;
			}
		}
		
	}

	void AnimateSpriteBackward ()
	{
		if (!isAffectedByPause || !isPaused)
		{
			animateTimer += Time.deltaTime;
			
			if(animateTimer >= frameDelay)
			{
				//move to the next index
				currentFrame--;
				if (currentFrame <= 0) { currentFrame = (rows * columns) - 1; }

				ChangeFrame (currentFrame);

				animateTimer = 0;
			}
		}
		
	}
	
	void Update ()
	{
		//frameDelay = (1f / framesPerSecond);

//		if (TEST) {
//			TEST = false;
//			AnimateForward ();
//		}

		if (animate) {
			if (!animateBackward) {
				AnimateSpriteForward ();
			}
			else {
				AnimateSpriteBackward ();
			}

			if (!loop) {
				UpdateCycles ();
			}
		}
	}

	void UpdateCycles ()
	{
		if (!animateBackward) {
			if (currentFrame >= totalFrames - 1) {
				cycles++;
				
				if (cycles >= numberOfCycles) { 
					StopAnimating ();
					cycles = 0;
				}
			}
		}
		else {
			if (currentFrame <= 0) {
				cycles++;
				
				if(cycles >= numberOfCycles) { 
					StopAnimating ();
					cycles = 0;
				}
			}
		}
	}

//	public void AnimateTexture ()
//	{
//		StartCoroutine(UpdateTiling());
//		
//		//set the tile size of the texture (in UV units), based on the rows and columns
//		Vector2 size = new Vector2(1f / columns, 1f / rows);
//		renderer.sharedMaterial.SetTextureScale("_MainTex", size);
//	}
//	
//	IEnumerator UpdateTiling()
//	{
//		while (!isPaused)
//		{
//			//move to the next index
//			currentFrame++;
//			if (currentFrame >= rows * columns) { currentFrame = 0; }
//			
//			//split into x and y indexes
//			Vector2 offset = FindOffset ();
//			
//			myRend.sharedMaterial.SetTextureOffset("_MainTex", offset);
//			
//			yield return new WaitForSeconds(1f / framesPerSecond);
//		}
//		
//	}
//	
}