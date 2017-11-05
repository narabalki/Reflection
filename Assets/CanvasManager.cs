using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class CanvasManager : MonoBehaviour {
    private GameObject introCanvas, secondCanvas;
 
    void Awake() {
        introCanvas = GameObject.Find("Canvas");
		secondCanvas = GameObject.Find("SecondCanvas");
    }
 
    void Start()
    {
        introCanvas.SetActive(true);
		secondCanvas.SetActive(false);
    }
}