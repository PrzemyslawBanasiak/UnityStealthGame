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
    private bool playerInSight = false;
    private Vector3 lastPlayerPosition;
    private float lastPlayerEnterSightTime;
    private float startLookingAroundTime = 0f;
    public float shortSightDistance = 0.2f;
    public float shortSightAngle = 210f;
    private bool wasPlayerInSight = false;
    private bool isFollowing = false;
    private bool isLookingAround = false;
    private Animator anim;


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
		if (agent.remainingDistance < agent.radius) {
            if (!isFollowing) {
                targetIndex = (targetIndex + 1) % pathPoints.Length;
            } else {
                if (!isLookingAround)
                {
                    anim.SetTrigger("look_around");
                    agent.isStopped = true;
                    isLookingAround = true;
                    startLookingAroundTime = Time.time;
                } 
                else if ((startLookingAroundTime + 1f) < Time.time)
                {
                    startLookingAroundTime = 0f;
                    isFollowing = false;
                    agent.isStopped = false;
                    isLookingAround = false;
                    agent.destination = pathPoints[targetIndex].position;
                    GetComponent<Transform>().LookAt(agent.steeringTarget + transform.forward);
                }
            }
			agent.destination = pathPoints [targetIndex].position;
			GetComponent<Transform>().LookAt(agent.steeringTarget + transform.forward);
		}

		Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
		Debug.DrawRay(transform.position, forward, Color.green);
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

        if (!wasPlayerInSight)
        {
            agent.isStopped = true;
            lastPlayerEnterSightTime = Time.time;
        }

        if((lastPlayerEnterSightTime + detectionTime) < Time.time) {
            anim.SetTrigger("shot");
            GameObject.Find("GameController").GetComponent<GameStatusController>().SetLost();
        }
    }

    private void handlePlayerLeftSight()
    {
        agent.destination = lastPlayerPosition;
        agent.isStopped = false;
        isFollowing = true;
    }

    private float truncate(float min, float max, float val) {
        if(val > max) 
            return max;
        if(val < min)
            return min;
        return val;
    }
}
