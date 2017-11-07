using UnityEngine;
using System.Collections;

public class GameStatusController : MonoBehaviour {
	public GameObject lostCanvas;
	public GameObject wonCanvas;
	public Light redLight;
	public bool IsGameLost {get; private set;}

	// Use this for initialization
	void Start () {
		IsGameLost = false;  
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetLost(){
		lostCanvas.SetActive(true);
		redLight.enabled = true;
        GameObject.Find("Player").GetComponent<ClickToMove>().Die();
	}

	public void SetWon() {
		wonCanvas.SetActive(true);
	}
}
