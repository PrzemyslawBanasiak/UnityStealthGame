using UnityEngine;
using System.Collections;

public class PlayerEnterGem : MonoBehaviour {
    private ScoreController controller;
    public GameObject collectEffect;

    void Start() {
        controller = GameObject.Find("GameController").GetComponent<ScoreController>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != "Player")
            return;
        controller.AddScore();
        if(collectEffect != null)
            Instantiate(collectEffect, this.GetComponent<Transform>().position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
