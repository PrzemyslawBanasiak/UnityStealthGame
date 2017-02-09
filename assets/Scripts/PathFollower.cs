using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour {
	//System.Collections.ArrayList
	public GameObject path;
	private Transform[] pathPoints;
	private NavMeshAgent agent;
	private int targetIndex = 0;

	void Start() {
		pathPoints = new Transform[path.transform.childCount];
		agent = GetComponent<NavMeshAgent>();
		for(int i =0; i < path.transform.childCount; ++i)
			pathPoints[i] = path.transform.GetChild(i);
		agent.destination = pathPoints [0].position;
	}
	
	// Update is called once per frame
	void Update () {
		if (agent.remainingDistance < agent.radius) {
			targetIndex = (targetIndex + 1) % pathPoints.Length;
			agent.destination = pathPoints [targetIndex].position;
			GetComponent<Transform>().LookAt(agent.steeringTarget + transform.forward);
		}

		Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
		Debug.DrawRay(transform.position, forward, Color.green);
	}
}
