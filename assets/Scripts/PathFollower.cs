using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour {
	public GameObject path;
	private Transform[] pathPoints;
	private UnityEngine.AI.NavMeshAgent agent;
	private int targetIndex = 0;

	void Start() {
		pathPoints = new Transform[path.transform.childCount];
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		for(int i =0; i < path.transform.childCount; ++i)
			pathPoints[i] = path.transform.GetChild(i);
		agent.destination = pathPoints [0].position;
	}
	
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
