using UnityEngine;
using System.Collections;

public class ClickableOpenDoors : IClickable {
	private GameObject player;
	public Transform animationPosition;
	private bool isSet = false;
	public float offset = 0.01f;

	public void Start() {
		player = GameObject.FindWithTag("Player");
		if(animationPosition == null)
			animationPosition = GetComponent<Transform>();
	}

	public override void OnClick() {
		player.GetComponent<NavMeshAgent>().destination = animationPosition.position;
	}

	public override void Reset() {
		isSet = false;
	}

	void Update() {
		if(!isSet)
			return;

		if(Vector3.Distance(animationPosition.position, player.transform.position) < offset) {
			player.transform.rotation = animationPosition.rotation;
		}
	}
}
