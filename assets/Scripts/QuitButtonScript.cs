using UnityEngine;
using UnityEngine.UI;
public class QuitButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	
	void TaskOnClick () {
		Application.Quit();
	}
}
