using UnityEngine;
using System.Collections;

public class PlayerEnterGem : MonoBehaviour {
    private GameObject Player;
    private ScoreController controller;
    public GameObject collectEffect;

    void Start() {
        Player = GameObject.FindWithTag("Player");
        controller = GameObject.Find("GameController").GetComponent<ScoreController>();
        if (Player == null)
            Debug.Log("Player Not Found");
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject != Player)
            return;
        controller.addScore();
        if(collectEffect!=null)
            Instantiate(collectEffect, this.GetComponent<Transform>().position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
