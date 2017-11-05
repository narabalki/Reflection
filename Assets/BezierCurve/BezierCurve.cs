using UnityEngine;
using System.Collections;

public class BezierCurve : MonoBehaviour 
{
	public static BezierCurve instance;
	void Awake() { MakeMeSingleton(); }
	void MakeMeSingleton () {
		GameObject.DontDestroyOnLoad(this);
		if(instance == null) { instance = this; }
		else { GameObject.Destroy(this.gameObject); }
	}
	
	public int divisions 	= 30;
	public float height 	= 30;
	
	public ArrayList GetBezierPoints (Vector3 startPosition, Vector3 endPosition, bool getPositiveHeight, int num_of_points = 0)
	{
		ArrayList bezierPoints = new ArrayList ();

		if (num_of_points == 0) {
			num_of_points = divisions;
		}

		Vector3 midPosition = FindPerpendicular(startPosition ,endPosition, getPositiveHeight);
		
		for (int t = 0; t <= num_of_points; t++)
		{
			Vector3 bezierPoint = CalculateBezierPoint(((float)t/num_of_points) , startPosition , midPosition , endPosition);
			//bezierPoint.z = startPosition.z;
			bezierPoints.Add (bezierPoint);
		}
		
		//PrintPoints (startPosition, endPosition, bezierPoints);
		
		return bezierPoints;
	}

	static void PrintPoints (Vector3 startPosition, Vector3 endPosition, ArrayList bezierPoints)
	{
		string x = "===== Bezier's Points ======\n";
		x += "Start Pos : (" + startPosition.x + ", " + startPosition.y + ", " + startPosition.z + ") \n";
		x += "End Pos : (" + endPosition.x + ", " + endPosition.y + ", " + endPosition.z + ") \n";
		x += "====== Points ======";
		foreach (Vector3 bp in bezierPoints) {
			x += "\n" + "(" + bp.x + ", " + bp.y + ", " + bp.z + ")";
		}
		Debug.Log (x);
	}

	private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
		float u = 1 - t;
		float tSquare = t * t;
		float uSquare = u * u;
		
		//bezierPoint = (( (1-t) * (1-t) ) * p0) + (2 * (1-t) * t * p1) + ((t * t) * p2);
		Vector3 bezierPoint = (uSquare * p0) + (2 * u * t * p1) + (tSquare * p2);
		
		return bezierPoint;
    }
    
	public Vector3 FindPerpendicular (Vector3 startPoint , Vector3 endPoint, bool getPositiveHeight)
	{
		/*
			dx = x1-x2
			dy = y1-y2
			dist = sqrt(dx*dx + dy*dy)
			dx /= dist
			dy /= dist
			x3 = x1 + (N/2)*dy
			y3 = y1 - (N/2)*dx
			x4 = x1 - (N/2)*dy
			y4 = y1 + (N/2)*dx
		*/
		
		Vector3 tempVector = new Vector3(0,0,0);
		Vector3 thirdPoint = new Vector3(0,0,0);
		Vector3 forthPoint = new Vector3(0,0,0);
		
		Vector3 midPoint = FindMidPoint(startPoint ,endPoint);
		
		tempVector.x = endPoint.x-startPoint.x;
		tempVector.y = endPoint.y-startPoint.y;
		
		float dist = Mathf.Sqrt(tempVector.x * tempVector.x + tempVector.y * tempVector.y);
		
		tempVector.x /= dist;
		tempVector.y /= dist;
		
		thirdPoint.x = midPoint.x - height * tempVector.y;
		thirdPoint.y = midPoint.y + height * tempVector.x;
		
		forthPoint.x = midPoint.x + height * tempVector.y;
		forthPoint.y = midPoint.y - height * tempVector.x;
		
		Vector3 pointToReturn = new Vector3(0,0,0);
		
		if(thirdPoint.y > forthPoint.y) 
		{
			if (getPositiveHeight) 	{ pointToReturn = thirdPoint; }
			else 					{ pointToReturn = forthPoint; }
		}
		else
		{
			if (getPositiveHeight) 	{ pointToReturn = forthPoint; }
			else 					{ pointToReturn = thirdPoint; }
		}
		
		//Debug.Log ("FindPerpendicular >>>>\n startPoint:" + startPoint + "\n endPoint:" + endPoint + "\n midPoint:" + midPoint + "\n thirdPoint:" + pointToReturn);
		
		return pointToReturn;
	}
	
	Vector2 FindMidPoint(Vector2 startPoint , Vector2 endPoint)
	{
		//Midpoint of a line = x1+x2/2, y1+y2/2
		Vector2 midPoint = new Vector2(0,0);
		
		midPoint.x = (startPoint.x + endPoint.x)/2;
		midPoint.y = (startPoint.y + endPoint.y)/2;
		
		return midPoint;
	}
}
