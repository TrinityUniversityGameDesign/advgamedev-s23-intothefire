using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialRoomScript : MonoBehaviour
{
	// public enum RoomSize
	// { onextwo, twoxtwo };

	public enum RoomState
	{ empty, closed, trialing, completed};

	//public RoomSize roomSize;
	protected RoomState currRoomState;
	[SerializeField]
	protected Transform[] doors;

	[SerializeField]
	private GameObject enemyPrefab;

	[SerializeField]
	protected GameObject startPad; // for startpad prefab
	[SerializeField]
	protected GameObject endPad;
	[SerializeField]
	protected List<GameObject> trialGeometry = new List<GameObject>();
	[SerializeField]
	protected List<GameObject> enemyList = new List<GameObject>();
	private int roomLength;  //used to determine where trial geometry can be placed & enemies can be spawned

	private int enemyCount;

	protected GameObject playerRef;

	private void Start()
	{
		//room initial setup
		//doors = transform.GetChild(0).transform.GetComponentsInChildren<Transform>();
		SetDoorPresence(false);
		currRoomState = RoomState.empty;


		//if this room is an empty room, don't turn it into a combat or platforming room
	}

	//____________________________________________________________________________________________________________

	public void OnTriggerEnter(Collider other)
	{
		Debug.Log(currRoomState);
		//Debug.Log("Collided with: " + other);
		if(other.transform.tag == "Player" && currRoomState == RoomState.empty)
		{
			Debug.Log("Player found for this room");
			RoomClose();
			playerRef = other.gameObject;
		}
		Debug.Log(doors);
	}

	//Player enters room
	protected void RoomClose() //called when player first enter's a room's trigger box
	{
		SetDoorPresence(true);
		currRoomState = RoomState.closed;
		Debug.Log("Room Closed");

		//Spawn Startpad in appropriate room location
		PlaceStartPad();
	}
	protected void SetDoorPresence(bool doorsEnabled)
	{
		//Debug.Log("Doors are present- " + doorsEnabled);

		foreach (Transform childObject in doors)
		{
			MeshRenderer meshRenderer = childObject.GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				meshRenderer.enabled = doorsEnabled;
			}

			BoxCollider boxCollider = childObject.GetComponent<BoxCollider>();
			if (boxCollider != null)
			{
				boxCollider.enabled = doorsEnabled;
			}
		}
	}
	public virtual void PlaceStartPad()
	{
		
		GameObject startPadReference = Instantiate(startPad, transform.position, new Quaternion(0, 0, 0, 0), this.transform);
		startPadReference.GetComponent<StartPadScript>().hostRoom = this;
		trialGeometry.Add(startPadReference);
		
	}

	//____________________________________________________________________________________________________________

	//Generate Trial Geometry and Start Trial
	public virtual void StartTrial()//called by the startpad when player enters the startpad's trigger area
	{
		if (currRoomState == RoomState.closed) {
			currRoomState = RoomState.trialing;
			Debug.Log("StartTrial Should be overridden in a child class");
		}
	}

	private void GeneratePlatInside()//generate a platforming room
	{
		//******************all locations for room geometry must be places in relation to room transform**************
		//use room type and dungeonDepth to create a room layout with an appropriate difficulty
		//add everything to List TrialGeometry

		//^^Is what I would do if i had time ayoooo

		//Create an endPad at the transform of the room w/ -10 in the z
		Vector3 endPadPos = this.transform.position + new Vector3(0, 0, 10);
		GameObject endPadReference = Instantiate(endPad, endPadPos, new Quaternion(0, 0, 0, 0), this.transform);
		//GameObject endPadReference = transform.GetChild(5).gameObject;
		//Set the host room of the endPad to this room
		endPadReference.GetComponent<EndPadScript>().hostRoom = this;
		//Add the endPad to our trial geometry so we can delete it once a room is cleared
		trialGeometry.Add(endPadReference);

	}

	public int GetEnemyCount(){return enemyCount;}

	public void SetEnemyCount(int val){enemyCount = val;}

	public void DecrementEnemyCount(){enemyCount--;}

	public void IncrementEnemyCount(){enemyCount++;}

//____________________________________________________________________________________________________________

	//completed trial
	public void TrialCompleted()
	{
		DespawnTrialGeometry();
		GivePlayerLoot();
		currRoomState = RoomState.completed;
		Debug.Log("Room Completed");
		SetDoorPresence(false);
	}
	
	public virtual void DespawnTrialGeometry()
	{
		//go through trial geometry list and set to Active(false)
		foreach(GameObject thing in trialGeometry)
		{
			if(thing){
				thing.SetActive(false);
			}
		}
	}
	private void GivePlayerLoot()
	{
		//use playerRef to give loot
	}


}
