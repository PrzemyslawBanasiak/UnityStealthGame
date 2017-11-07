using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
    int score = 0;
    int max;
    Text text;
    GameStatusController statusController;

	// Use this for initialization
	void Start () {
        text = GameObject.Find("Score").GetComponent<Text>();
        max = GameObject.Find("Gems").GetComponent<Transform>().childCount;
        statusController = this.GetComponent<GameStatusController>();
        text.text = FormatString();
    }

    public void AddScore(int add = 1) {
        score += add;
        text.text = FormatString();
        if(score == max) {
            statusController.SetWon();
        }
    }

    string FormatString() {
        return score + " / " + max;
    }
}
