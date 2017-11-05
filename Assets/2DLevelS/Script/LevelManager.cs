using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

	public float speed = 20f;
	public StoryMode vStoryMode;
	public GameObject LineBTWChapter;
	public GameObject EmptyGameObject; //USED to link the same line to another chapter
	private Levels vCurrentSelected = null;

	public Color vRoadNotDone = new Color(0.2f, 0.2f, 0.2f);
	public Color vRoadDone = Color.white;
	public Color vRoadBtwChapter = Color.green;
	
	private bool ChangeScoreType = false;

	private Ray ray;
	private RaycastHit hit;

	private Vector3 Origin;
	private Vector3 Diference;
	private bool Drag=false;

	private GameObject LevelPanel;
	public bool showSettings = false;

	public static LevelManager instance;

	// Use this for initialization
	void Start () {

		LevelPanel = GameObject.Find ("SettingsPanel");
		DrawChapter ();

		LevelPanel.SetActive (showSettings);

		instance = this;
	}

	public void ToggleSettings() {
		showSettings = !showSettings;
		LevelPanel.SetActive (showSettings);
	}

	// Update is called once per frame
	void Update () {
		if (!showSettings) {
			if (Input.GetMouseButton (0)) {
				Diference = (Camera.main.ScreenToWorldPoint (Input.mousePosition)) - Camera.main.transform.position;
				if (Drag == false) {
					Drag = true;
					Origin = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				}
			} else {
				Drag = false;
			}

			if (Drag == true) {
				Camera.main.transform.position = Origin - Diference;
			}

			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit) && Input.GetMouseButtonDown (0) && vCurrentSelected == null) {  //can only get a new level if we have closed the level selector
				Levels vLevelSelected = GetLevelByObject (hit.collider.gameObject);

				//can only click on the LevelNext Sprite
				if (vLevelSelected.vObject.GetComponent<SpriteRenderer> ().sprite.name == "LevelNEXT" ||
				    vLevelSelected.vObject.GetComponent<SpriteRenderer> ().sprite.name == "LevelDONE") {
					vCurrentSelected = vLevelSelected;

					#if false
					ApplicationModel.currentLevel = vLevelSelected.Chapter;
					ApplicationModel.currentIndex = vLevelSelected.order;
					Debug.Log ("the current level is " + ApplicationModel.currentLevel);
					#endif
					SceneManager.LoadSceneAsync ("GamePlayArena");

				}
			}
		}
	} 

	public void CompleteLevel(string vScore)
	{
		vCurrentSelected.Completed = true;
		vCurrentSelected.score = vScore;
		vCurrentSelected = null;
		DrawChapter();
	}


	Levels GetLevelByObject(GameObject vLevelObject)
	{
		//initialize the variable
		Levels vLevelToReturn = null;

		foreach (Chapters CChapter in vStoryMode.vChapter)
		{
			foreach (Levels cLevels in CChapter.vLevels)
			{
				//get the right level
				if (cLevels.vObject == vLevelObject)
				{
					vLevelToReturn = cLevels;
					vLevelToReturn.Chapter = CChapter.order; //save the chapter in the level
				}
			}
		}

		//get the level to return
		return vLevelToReturn;
	}

	//get the next level position to make a line between them
	public Vector3[] GetNextLevelPosition(Levels CuLevel, Chapters CuChapter)
	{
		Vector3[] vNextPosition = new Vector3[0];

		//check if we are at the last level of the chapter to make a connection between chapters and is completed
		if ((CuLevel.order == vStoryMode.vChapter[CuChapter.order-1].vLevels.Count()) && (CuLevel.Completed))
		{
			vNextPosition = new Vector3[vStoryMode.vChapter[CuChapter.order-1].UnlockNextChapter.Count()];
			int i = 0;
			
			foreach(string vUnlockNextChapter in vStoryMode.vChapter[CuChapter.order-1].UnlockNextChapter)
			{
				//get the next position
				if (vUnlockNextChapter.Trim() != "")
				{
					vNextPosition[i] = vStoryMode.vChapter[int.Parse(vUnlockNextChapter)-1].vLevels[0].vObject.transform.position;
					vStoryMode.vChapter[int.Parse(vUnlockNextChapter)-1].vLevels[0].CanShow = true; //make sure we can see the other chapter when unlocked
					i++;
				}
			}
		}
		else
		{
			//check all the level object and make sure the current level icon is showed
			foreach (Chapters CChapter in vStoryMode.vChapter)
			{
				//only get the current chapters
				if (CChapter.order == CuChapter.order)
					foreach (Levels cLevels in CChapter.vLevels)
					{
					    //if we get the next levels, then we get the current position
						if (cLevels.order == CuLevel.order+1)
						{
							vNextPosition = new Vector3[1];
							vNextPosition[0] = cLevels.vObject.transform.position;
						}
					}
			}
		}
		
		//return the next position
		return vNextPosition;
	}

	public void ShowScore(Levels vLevels)
	{
		GameObject vScoreTab = (GameObject)Instantiate(Resources.Load("GUI/ScoreTab"));
		vScoreTab.transform.SetParent(vLevels.vObject.transform);
		vScoreTab.transform.localPosition = new Vector3 (-0.72f, 1.1f, 0f); 

		//show the Score above the level by the score type
		switch (vStoryMode.vScoreType)
		{
			//show the good number of stars
			case ScoreType.Stars :

				//disable the other
				vScoreTab.transform.Find("NumberType").gameObject.SetActive(false);
				vScoreTab.transform.Find("TimeType").gameObject.SetActive(false);

				//get full and empty star
				Sprite vSpriteFullS = Resources.Load<Sprite> ("GUI/Star");
				Sprite vSpriteEmptyS = Resources.Load<Sprite> ("GUI/StarEmpty");

				//empty them all by default
				vScoreTab.transform.Find("StarType").Find("Star1").GetComponent<SpriteRenderer>().sprite = vSpriteEmptyS;
				vScoreTab.transform.Find("StarType").Find("Star2").GetComponent<SpriteRenderer>().sprite = vSpriteEmptyS;
				vScoreTab.transform.Find("StarType").Find("Star3").GetComponent<SpriteRenderer>().sprite = vSpriteEmptyS;
				
				//by default, we put 0 
				if (vLevels.score == "")
					vLevels.score = "0";

				//when chaing score, we put value 1 by default
				if (ChangeScoreType)
					vLevels.score = "1";

				//get the full star 
				if (int.Parse(vLevels.score) > 0) vScoreTab.transform.Find("StarType").Find("Star1").GetComponent<SpriteRenderer>().sprite = vSpriteFullS;
				if (int.Parse(vLevels.score) > 1) vScoreTab.transform.Find("StarType").Find("Star2").GetComponent<SpriteRenderer>().sprite = vSpriteFullS;
				if (int.Parse(vLevels.score) > 2) vScoreTab.transform.Find("StarType").Find("Star3").GetComponent<SpriteRenderer>().sprite = vSpriteFullS;
				
			break;

			//show number
			case ScoreType.Numbers :
				//disable the other
				vScoreTab.transform.Find("StarType").gameObject.SetActive(false);
				vScoreTab.transform.Find("TimeType").gameObject.SetActive(false);

				//when chaing score, we put value 1 by default
				if (ChangeScoreType)
					vLevels.score = "1";

				//show the right score
				vScoreTab.transform.Find("NumberType").Find("NumberBack").Find("ScoreNumber").GetComponent<TextMesh>().text = vLevels.score.ToString();
				vScoreTab.transform.Find("NumberType").Find("NumberBack").Find("ScoreNumber").GetComponent<MeshRenderer>().sortingOrder = 1500;
			break;

			//show time
			case ScoreType.Times :
				//disable the other
				vScoreTab.transform.Find("NumberType").gameObject.SetActive(false);
				vScoreTab.transform.Find("StarType").gameObject.SetActive(false);

				//when chaing score, we put value 1 by default
				if (ChangeScoreType)
					vLevels.score = "1:00";

				//show the right score
				vScoreTab.transform.Find("TimeType").Find("NumberBack").Find("ScoreNumber").GetComponent<TextMesh>().text = vLevels.score.ToString();
				vScoreTab.transform.Find("TimeType").Find("NumberBack").Find("ScoreNumber").GetComponent<MeshRenderer>().sortingOrder = 1500;
			break;
		}

	}

	public void DrawChapter()
	{
		//before drawing the chapter, we will remove all the previous objects. ONLY usefull when we redraw.
		foreach (Chapters CChapter in vStoryMode.vChapter)
		{
			foreach (Levels cLevels in CChapter.vLevels)
			{
				//destroy child
				foreach (Transform child in cLevels.vObject.transform) {
					if (cLevels.vObject.transform != child.transform)
						GameObject.Destroy(child.gameObject);
				}
			}
		}

		//check all the level object and make sure the current level icon is showed
		foreach (Chapters CChapter in vStoryMode.vChapter)
		{
			//put a different icon for the first not completed levels by chapters
			bool firststage = true;

			//initialie the sprite to use
			Sprite vLevelToDo = Resources.Load<Sprite> ("Map/Level/LevelTODO");
			Sprite vLevelDone = Resources.Load<Sprite> ("Map/Level/LevelDONE");
			Sprite vLevelNext = Resources.Load<Sprite> ("Map/Level/LevelNEXT");

			bool ShowChapter = false;

			//get all the levels in this chapters
			foreach (Levels cLevels in CChapter.vLevels)
			{
				if (CChapter.order == MyPlayerPrefs.GetLevel()) {
					if (cLevels.order <= MyPlayerPrefs.GetChallengeIndex ())
						cLevels.Completed = true;
				} else if (CChapter.order < MyPlayerPrefs.GetLevel()) {
					cLevels.Completed = true;
				}

				cLevels.vObject.SetActive(true);
				SpriteRenderer vSpriteRenderer = cLevels.vObject.GetComponent<SpriteRenderer>();

				//if there is a level we can show, we show the entire chapter
				if (cLevels.CanShow)
					ShowChapter = true;

				//show the current level done and todo
				if (cLevels.Completed)
				{
					//ONLY show the score if completed
					//ShowScore(cLevels);
					vSpriteRenderer.sprite = vLevelDone;
					ShowChapter = true; //if at least 1 level is completed in the chapter, we show the rest. if not, we hide it
				}
				else
				{
					//ONLY show in RED the next level to do 
					if (firststage) {
						firststage = false;
						vSpriteRenderer.sprite = vLevelNext;
						if (ShowChapter) {
							Camera.main.transform.position = new Vector3(cLevels.vObject.transform.position.x,cLevels.vObject.transform.position.y,Camera.main.transform.position.z);
						}
					} else {
						vSpriteRenderer.sprite = vLevelToDo;
//						Camera.main.transform.LookAt (cLevels.vObject.transform);
					}
				}

				if (!ShowChapter && !cLevels.CanShow)
				{
					vSpriteRenderer.gameObject.SetActive(false);
				}

				
				//draw the line between the 2 points
				Vector3[] vNextPosition = GetNextLevelPosition(cLevels, CChapter);
				
				if (vNextPosition.Count() > 0)
				{
					int i = 0;
					foreach (Vector3 vUnlockNextChapterPos in vNextPosition)
					{
						if (vUnlockNextChapterPos != Vector3.zero)
						{
							//get the current line renderer
							LineRenderer vCurrentLine = GetComponent<LineRenderer>();
							if (CChapter.vLevels.Count() == cLevels.order)
								vCurrentLine = LineBTWChapter.GetComponent<LineRenderer>();
							
							if (i > 0)
							{
								//create a empty game object
								GameObject vEmptyGameObject =  (GameObject)Instantiate(EmptyGameObject, cLevels.vObject.transform.position,Quaternion.identity);
								vEmptyGameObject.AddComponent<LineRenderer>();
								LineRenderer vLineRenderer = vEmptyGameObject.GetComponent<LineRenderer>();
								vLineRenderer.material = vCurrentLine.material;
								vLineRenderer.SetWidth(.45f, .45f);
								
								//if current level is completed
								if (CChapter.vLevels.Count() == cLevels.order)
									vLineRenderer.SetColors(vRoadBtwChapter, vRoadBtwChapter);
								else if (cLevels.Completed)
									vLineRenderer.SetColors(vRoadDone, vRoadDone);
								else
									vLineRenderer.SetColors(vRoadNotDone, vRoadNotDone);
								
								vLineRenderer.SetPosition(0, cLevels.vObject.transform.position);
								vLineRenderer.SetPosition(1, vUnlockNextChapterPos);

							}
							else
							{
								if (cLevels.vObject.GetComponent<LineRenderer>() == null)
								{
									cLevels.vObject.AddComponent<LineRenderer>();
									cLevels.vLineRenderer = cLevels.vObject.GetComponent<LineRenderer>();
								}
								cLevels.vLineRenderer.material = vCurrentLine.material;
								cLevels.vLineRenderer.SetWidth(.45f, .45f);


								//if current level is completed
								if (CChapter.vLevels.Count() == cLevels.order)
									cLevels.vLineRenderer.SetColors(vRoadBtwChapter, vRoadBtwChapter);
								else if (cLevels.Completed)
									cLevels.vLineRenderer.SetColors(vRoadDone, vRoadDone);
								else
									cLevels.vLineRenderer.SetColors(vRoadNotDone, vRoadNotDone);
								
								cLevels.vLineRenderer.SetPosition(0, cLevels.vObject.transform.position);
								cLevels.vLineRenderer.SetPosition(1, vUnlockNextChapterPos);
							}
							i++;
						}
					}
				}
			}
		}
	}

	public enum ScoreType{Stars, Numbers, Times};

	[System.Serializable]
	public class StoryMode
	{
		public Chapters[] vChapter;
		public ScoreType vScoreType = ScoreType.Stars; //here you can define how you will calculate the score on a level
	}

	[System.Serializable]
	public class Chapters
	{
		public int order = 1;
		public string name = "";
		public Levels[] vLevels = new Levels[0];
		public string[] UnlockNextChapter = new string[0];
	}
	
	[System.Serializable]
	public class Levels
	{
		public int order = 0;
		public string name = "";
		public string description = "";
		public string score = "0";
		public GameObject vObject = null;
		public LineRenderer vLineRenderer = null;
		public bool Completed = false;
		public bool CanShow = false;
		public int Chapter = 0;
	}
}
