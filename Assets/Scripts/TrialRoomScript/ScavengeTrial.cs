using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeTrial : TrialRoomScript
{
    [SerializeField]
    private List<GameObject> scavengeEntities = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
		SetDoorPresence(false);
		currRoomState = RoomState.empty;

        foreach (GameObject thing in scavengeEntities)
        {
            thing.GetComponent<ScavengeEntity>().hostRoom = this;
            thing.SetActive(false);
        }

        scavengeEntities[Random.Range(0, scavengeEntities.Count - 1)].GetComponent<ScavengeEntity>().hasTreasure = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlaceStartPad(){
        GameObject startPadReference = startPad;
		startPadReference.GetComponent<StartPadScript>().hostRoom = this;
        startPadReference = Instantiate(startPad, transform.position, new Quaternion(0, 0, 0, 0), this.transform);
        trialGeometry.Add(startPadReference);
        //Debug.Log("override");
    }

    public override void StartTrial(){
        foreach(GameObject thing in scavengeEntities){
            thing.SetActive(true);
        }
    }

    public override void DespawnTrialGeometry(){
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                thing.SetActive(false);
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

    public void OnTriggerEnter(Collider other)
	{
		//Debug.Log(currRoomState);
		//Debug.Log(trialType);
		//Debug.Log("Collided with: " + other);
		if(other.transform.tag == "Player" && currRoomState == RoomState.empty)
		{
			//Debug.Log("Player found for this room");
			RoomClose();
			playerRef = other.gameObject;
		}
		//Debug.Log(doors);
	}

}
