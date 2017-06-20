using UnityEngine;
using System.Collections;

public class GuardMove : MonoBehaviour {
	public GameObject path;
	public float SightAngle = 75f;
    public float detectionTime = 1.5f;
	private Transform[] pathPoints;
	private UnityEngine.AI.NavMeshAgent agent;
	private int targetIndex = 0;
    private GameObject Player;
    private Vector3 lastPlayerPosition;
    private float lastPlayerEnterSightTime;
    private float startLookingAroundTime = 0f;
    public float shortSightDistance = 0.2f;
    public float shortSightAngle = 210f;
	public float shotRange = 3f;
    private bool wasPlayerInSight = false;
    private Animator anim;

	/* State Info */
	private string currentState = "Patrol";
	private bool playerLeftSight = false;
	private bool playerInSight = false;
	private bool isMoving = false;
	private bool playerInShotRange = false;

	/* Delayed Transition */
	private string currentTransitionTo;
	private float currentTransitionTime;
	private bool delayedTransitionUpdated = false;

	public bool ddd = false;


    void Start()
    {
        pathPoints = new Transform[path.transform.childCount];
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		for(int i =0; i < path.transform.childCount; ++i)
			pathPoints[i] = path.transform.GetChild(i);
		agent.destination = pathPoints [0].position;
        Player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        if (Player == null)
            Debug.Log("Player Not Found");
	}
	
	// Update is called once per frame
	void Update () {
		delayedTransitionUpdated = false;
		if (currentState == "Patrol") {
			agent.isStopped = false;
			if (playerInSight) {
				currentState = "FollowPlayer";
			} else if (agent.remainingDistance < agent.radius) {
				targetIndex = (targetIndex + 1) % pathPoints.Length;
				agent.destination = pathPoints [targetIndex].position;
				GetComponent<Transform> ().LookAt (agent.steeringTarget + transform.forward);	
			}
		}

		else if (currentState == "FollowPlayer") {
			agent.isStopped = false;
			if (!playerInSight)
				DelayedTransition ("GotoPlayerPosition", 1f);
			else if (playerInShotRange) 
				currentState = "Aim";
			agent.destination = lastPlayerPosition;
		}

		else if (currentState == "Aim") {
			agent.isStopped = true;
			if (!playerInSight)
				currentState = "GotoPlayerPosition";
			else
				DelayedTransition ("Shot", detectionTime);
		}

		else if (currentState == "GotoPlayerPosition") {
			agent.isStopped = false;
			agent.destination = lastPlayerPosition;
			if (playerInSight) {
				currentState = "FollowPlayer";
			} else if (agent.remainingDistance < agent.radius) {
				currentState = "Look Around";
			}
		}

		else if (currentState == "Look Around") {
			if (playerInSight)
				currentState = "FollowPlayer";
			else
				DelayedTransition ("Patrol", 4.8f);
		}

		else if (currentState == "Shot") {
			agent.isStopped = true;
			currentState = "Done";
			GameObject.Find ("GameController").GetComponent<GameStatusController>().SetLost ();
		}

		if (!delayedTransitionUpdated)
			currentTransitionTo = "";
	}

	void DelayedTransition (string to, float ttime) {
		delayedTransitionUpdated = true;
		if (currentTransitionTo == to) {
			if (currentTransitionTime < Time.time)
				currentState = to;
		} else {
			currentTransitionTo = to;
			currentTransitionTime = Time.time + ttime;
		}
	}
    
	void OnTriggerStay(Collider other) {
        if (other.gameObject != Player)
            return;
        handlePlayerIsSight(other);
        if (!playerInSight && wasPlayerInSight)
            handlePlayerLeftSight();
    }

    private void handlePlayerIsSight(Collider other) {
        wasPlayerInSight = playerInSight;
        playerInSight = false;

        Vector3 direction = other.transform.position - transform.position;
		float angle = Vector3.Angle(direction, transform.forward);
        float distance = Vector3.Distance(other.transform.position, transform.position);

        if(angle > SightAngle/2 && !(distance < shortSightDistance && angle < shortSightAngle/2)) 
            return;
        
        RaycastHit hit;
        if(!Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, 50.0f))
            return;

        if(hit.collider.gameObject != Player)
            return;
        
        lastPlayerPosition = other.transform.position;
        playerInSight = true;

        if (!wasPlayerInSight) {
			anim.SetBool ("PlayerInSight", true);
        }

		if (distance < shotRange)
			playerInShotRange = true;
    }

    private void handlePlayerLeftSight()
    {
		anim.SetBool ("PlayerInSight", false);
    }

    private float truncate(float min, float max, float val) {
        if(val > max) 
            return max;
        if(val < min)
            return min;
        return val;
    }
}
