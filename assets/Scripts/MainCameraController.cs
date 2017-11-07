using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour
{
	public float smooth = 1.5f;			// The relative speed at which the camera will catch up.
	
	
	private Transform player;			// Reference to the player's transform.
	private Vector3 relCameraPos;		// The relative position of the camera from the player.
	private float relCameraPosMag;		// The distance of the camera from the player.
	private Vector3 newPos;				// The position the camera is trying to reach.

	
	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag("Player").transform;
		relCameraPos = transform.position - player.position;
		relCameraPosMag = relCameraPos.magnitude - 0.5f;
	}
	
	
	void FixedUpdate ()
	{
		Vector3 standardPos = player.position + relCameraPos; //relative position of the camera from the player
        
        Vector3 abovePos = player.position + Vector3.up * relCameraPosMag;
		
		// An array of 5 points to check if the camera can see the player.
		Vector3[] checkPoints = new Vector3[5];
		
		// The first is the standard position of the camera.
		checkPoints[0] = standardPos;		
		checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
		checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
		checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
		
		// The last is the abovePos.
		checkPoints[4] = abovePos;
		
		// Run through the check points...
		for(int i = 0; i < checkPoints.Length; i++)
		{
			if(ViewingPosCheck(checkPoints[i]))
				break;
		}
		
		transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
		SmoothLookAt();
	}
	
	
	bool ViewingPosCheck (Vector3 checkPos)
	{
		RaycastHit hit;
		
		if(Physics.Raycast(checkPos, player.position - checkPos, out hit, relCameraPosMag))
			if(hit.transform != player)
				return false;
		
		newPos = checkPos;
		return true;
	}
	
	
	void SmoothLookAt ()
	{
		Vector3 relPlayerPosition = player.position - transform.position;
		Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
	}
}
