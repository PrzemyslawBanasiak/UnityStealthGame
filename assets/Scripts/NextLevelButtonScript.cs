using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NextLevelButtonScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	
	void TaskOnClick () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
