using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialRoomScript : MonoBehaviour
{
	public enum RoomSize
	{ onextwo, twoxtwo };
	public enum TrialType
	{ combat, plat, empty, hunt};

	public enum RoomState
	{ empty, closed, trialing, completed};

	public TrialType trialType;
	public RoomSize roomSize;
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

	[SerializeField]
	private GameObject boxPrefab;
	[SerializeField]
	private GameObject scavengeEnemyPrefab;

	private int enemyCount;

	protected GameObject playerRef;

	private void Start()
	{
		//room initial setup
		//doors = transform.GetChild(0).transform.GetComponentsInChildren<Transform>();
		SetDoorPresence(false);
		currRoomState = RoomState.empty;


		//if this room is an empty room, don't turn it into a combat or platforming room
		if(trialType != TrialType.empty)
		{
			//determine room type
			int rand = Random.Range(1, 3);
			if (rand == 1)
			{
				trialType = TrialType.combat;
			}
			else if(rand == 2)
			{
				trialType = TrialType.plat;
			}
			else
			{
				trialType = TrialType.hunt;
			}
		}
	}

	//____________________________________________________________________________________________________________

	public void OnTriggerEnter(Collider other)
	{
		Debug.Log(currRoomState);
		Debug.Log(trialType);
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
		
		GameObject startPadReference = startPad;
		if (trialType == TrialType.combat)
		{
			//place in center of the room
			startPadReference = Instantiate(startPad, transform.position, new Quaternion(0, 0, 0, 0), this.transform);
			startPadReference.GetComponent<StartPadScript>().hostRoom = this;
		}
		if (trialType == TrialType.plat)
		{
			//place at the end of the room
			//Instantiate(startPad prefab, new Vector3(LOCATION), new Quaternion(0, 0, 0, 0), this.transform);

			Vector3 startPadPos = this.transform.position + new Vector3(0, 0, -10);
			startPadReference = Instantiate(startPad, startPadPos, new Quaternion(0, 0, 0, 0), this.transform);//make this at one end of the room////////////////////////////////
			startPadReference.GetComponent<StartPadScript>().hostRoom = this;
		}
		if (trialType == TrialType.hunt)
		{
			startPadReference = Instantiate(startPad, transform.position, new Quaternion(0, 0, 0, 0), this.transform);
			startPadReference.GetComponent<StartPadScript>().hostRoom = this;
		}
		//transform.Find("StartPad").gameObject.GetComponent<StartPadScript>().hostRoom = this;//transform.GetChild<StartPad>("StartPad") or something
		//GameObject startPadReference = transform.GetChild(4).gameObject;
		// GameObject startPadReference = startPad;
		// startPadReference.GetComponent<StartPadScript>().hostRoom = this;
		trialGeometry.Add(startPadReference);
		
	}

	//____________________________________________________________________________________________________________

	//Generate Trial Geometry and Start Trial
	public virtual void StartTrial()//called by the startpad when player enters the startpad's trigger area
	{
		if (currRoomState == RoomState.closed) {
			if (trialType == TrialType.combat)
			{
				GenerateCombatInside();
				SpawnEnemies();
			}
			if (trialType == TrialType.plat)
			{
				GeneratePlatInside();
			}
			if (trialType == TrialType.hunt)
			{
				GenerateHuntInside();
			}
			currRoomState = RoomState.trialing;
			Debug.Log("Trial Started");
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
	
	private void GenerateCombatInside()//generate a combat room
	{
		//******************all locations for room geometry must be places in relation to room transform**************

		//use room type and dungeonDepth to create a room layout-- generating encounters happens when currRoomState is set to Trialing
		//add everything to List TrialGeometry
	}
	private void SpawnEnemies()
	{
		//spawn a bunch of enemies and add them to List<GameObject>EnemyList
		//trial is over when EnemyList.Count == 0;

		int numOfEnemies = Random.Range(3, 5);
		Debug.Log("Number of enemies for this room: " + numOfEnemies);
		for (int i = 0; i < numOfEnemies; i++)
		{
			Vector3 spawnLocation = new Vector3(transform.position.x + Random.Range(-15, 15), -3f, transform.position.z + Random.Range(-15, 15));
			GameObject enemy = Instantiate(enemyPrefab, spawnLocation, new Quaternion(0, 0, 0, 0), this.transform);
			enemy.GetComponent<EnemyScript>().hostRoom = this;
			enemyCount++;
		}
	}

	public void DecrementEnemyCount()
	{
		enemyCount--;
	}

	private void GenerateHuntInside(){
		//Debug.Log("hunt started");
		SpawnBoxes();
		//SpawnScavengeEnemies();
	}

	private void SpawnBoxes(){
		List<GameObject> boxes = new List<GameObject>();
		int numOfBoxes = 3;
		for(int i = 0; i < numOfBoxes; i++){
			Vector3 spawnLocation = new Vector3(transform.position.x + Random.Range(-20, 20), 2f, transform.position.z + Random.Range(-20, 20));
			GameObject box = Instantiate(boxPrefab, spawnLocation, new Quaternion(0, 0, 0, 0), this.transform);
			box.GetComponent<Box>().hostRoom = this;
			boxes.Add(box);
			trialGeometry.Add(box);
		}
		boxes[Random.Range(0, boxes.Count - 1)].GetComponent<Box>().hasTreasure = true;
		//Debug.Log(boxes.Count);
	}

	private void SpawnScavengeEnemies(){
		List<GameObject> scavengeEnemies = new List<GameObject>();
		int numOfEnemies = 5;
		for(int i = 0; i < numOfEnemies; i++){
			Vector3 spawnLocation = new Vector3(transform.position.x + Random.Range(-20, 20), 2f, transform.position.z + Random.Range(-20, 20));
			GameObject enemy = Instantiate(scavengeEnemyPrefab, spawnLocation, new Quaternion(0, 0, 0, 0), this.transform);
			enemy.GetComponent<ScavengeEnemy>().hostRoom = this;
			scavengeEnemies.Add(enemy);
		}
		scavengeEnemies[Random.Range(0, scavengeEnemies.Count - 1)].GetComponent<ScavengeEnemy>().hasTreasure = true;
	}


	public void Update()//is there a way to only call update once a combat room is started?
	{
		//checks to see if all enemies are defeated for a combat room
		if(trialType == TrialType.combat && currRoomState == RoomState.trialing && enemyCount == 0)
		{
			TrialCompleted();
		}
	}

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
