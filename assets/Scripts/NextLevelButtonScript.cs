using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NextLevelButtonScript : MonoBehaviour {
	public SceneAsset NextLevel;
	// Use this for initialization
	void Start () {
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	
	void TaskOnClick () {
		SceneManager.LoadScene(NextLevel.name);
	}
}
