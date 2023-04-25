using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialRoomScript : MonoBehaviour
{
	// public enum RoomSize
	// { onextwo, twoxtwo };
	public EmptyRoom hostEmpty;

	public enum RoomState
	{ empty, closed, trialing, completed};

	//public RoomSize roomSize;
	protected RoomState currRoomState;
	[SerializeField]
	protected Transform[] doors;

	[SerializeField]
	protected GameObject startPad; // for startpad prefab
	[SerializeField]
	protected List<GameObject> trialGeometry = new List<GameObject>();
	[SerializeField]
	protected List<GameObject> enemyList = new List<GameObject>();
	private int roomLength;  //used to determine where trial geometry can be placed & enemies can be spawned

	protected int enemyCount;

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
		//Debug.Log(currRoomState);
		//Debug.Log("Collided with: " + other);
		if(other.transform.tag == "Player" && currRoomState == RoomState.empty)
		{
			//Debug.Log("Player found for this room");
			RoomClose();
			playerRef = other.gameObject;
		}
		//Debug.Log(doors);
	}

	//Player enters room
	protected void RoomClose() //called when player first enter's a room's trigger box
	{
		SetDoorPresence(true);
		currRoomState = RoomState.closed;
		//Debug.Log("Room Closed");

		//Spawn Startpad in appropriate room location
		if(hostEmpty){StartTrial();}
		else{PlaceStartPad();}
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

			MeshCollider boxCollider = childObject.GetComponent<MeshCollider>();
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

	public int GetEnemyCount(){return enemyCount;}
	
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
		if(hostEmpty){
			hostEmpty.depsawnRoom();
		}
	}
	
	public virtual void DespawnTrialGeometry()
	{
		//go through trial geometry list and set to Active(false)
		// foreach(GameObject thing in trialGeometry)
		// {
		// 	if(thing){
		// 		thing.SetActive(false);
		// 	}
		// }
	}
	private void GivePlayerLoot()
	{
		//use playerRef to give loot
	}
}
