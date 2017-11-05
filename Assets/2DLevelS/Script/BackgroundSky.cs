using UnityEngine;
using System.Collections;

public class BackgroundSky : MonoBehaviour {

	//enum
	public enum Direction{East, West};

	//public variable
	public float Speed = 1f;
	private GameObject Background;
	public Direction vDirection = Direction.East;

	//private gameobject
	private GameObject vStart;

	// Use this for initialization
	void Start () {

		//get the right background object
		Background = transform.parent.gameObject;
		vStart = Background.transform.Find("Start").gameObject;

		if (vDirection == Direction.West)
			Speed = Speed * - 1;
	}

	void RespawnObject(float vSpawnXPosition)
	{
		transform.position = new Vector3 (vSpawnXPosition, transform.position.y, transform.position.z);
	}

	public bool IsIn()
	{
		float vPosition = transform.position.x;
		float vStartBackground = vStart.transform.position.x;
		float vEndBackground = Background.transform.position.x + Background.gameObject.GetComponent<Renderer>().bounds.extents.x;

		//Debug.Log("vposition="+vPosition+", vstart="+vStartBackground+", vEndBackground" + vEndBackground);

		//check if the object is out of the background
		if (vPosition > vEndBackground)
		{
			RespawnObject(vStartBackground + 10);
		    return false;
		}
		else if (vPosition < vStartBackground)
		{
			RespawnObject(vEndBackground - 10);
			return false;
		}
		else 
			return true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsIn())
			GetComponent<Rigidbody2D>().velocity = new Vector3 (Speed, 0f);	
	}
}
