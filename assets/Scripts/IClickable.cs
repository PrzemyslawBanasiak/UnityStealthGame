using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IClickable : MonoBehaviour {
	private HighlightsFX Highlights;
	
	virtual public void OnClick() {
		Debug.Log("OnClick not defined");
	}

	virtual public void Reset() {
		Debug.Log("Reset not defined");
	}

	void Awake() {
		Highlights = GameObject.Find("Main Camera").GetComponent<HighlightsFX>();
	}

	void OnMouseEnter() {
		Highlights.objectRenderer = this.GetComponent<Renderer>();
    }

	void OnMouseExit() {
		Highlights.objectRenderer = null;
	}
}
