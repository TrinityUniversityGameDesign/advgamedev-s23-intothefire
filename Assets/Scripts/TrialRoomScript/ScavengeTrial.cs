using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeTrial : TrialRoomScript
{
    [SerializeField]
    private List<GameObject> scavengeEntities = new List<GameObject>();

    [SerializeField]
    private GameObject boxPrefab;
    [SerializeField]
    private GameObject scavengeEnemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
		SetDoorPresence(false);
		currRoomState = RoomState.empty;
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                thing.SetActive(false);
            }
        }
        foreach (GameObject thing in scavengeEntities)
        {
            if(thing){
                thing.GetComponent<ScavengeEntity>().hostRoom = this;
                thing.SetActive(false);
            }
            
        }

        scavengeEntities[Random.Range(0, scavengeEntities.Count - 1)].GetComponent<ScavengeEntity>().hasTreasure = true;
    }

    public override void PlaceStartPad(){
        GameObject startPadReference = startPad;
		startPadReference.GetComponent<StartPadScript>().hostRoom = this;
        startPadReference = Instantiate(startPad, transform.position, new Quaternion(0, 0, 0, 0), this.transform);
        trialGeometry.Add(startPadReference);
        //Debug.Log("override");
    }

    public override void StartTrial(){
        currRoomState = RoomState.trialing;
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                thing.SetActive(true);
            }
        }
        foreach(GameObject thing in scavengeEntities){
            thing.SetActive(true);
        }
    }

    public override void DespawnTrialGeometry(){
        SetDoorPresence(false);
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                SpecialDisable disable = thing.GetComponent<SpecialDisable>();
                if(disable){
                    disable.disable();
                }
                else{
                    thing.SetActive(false);
                }
            }
        }
        foreach (GameObject thing in enemyList)
        {
            if(thing){
                thing.SetActive(false);
            }
        }
        foreach (GameObject thing in scavengeEntities)
        {
            if(thing){
                thing.SetActive(false);
            }
        }
    }

    new public void OnTriggerEnter(Collider other)
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

}
