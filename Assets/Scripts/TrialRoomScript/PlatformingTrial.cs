using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingTrial : TrialRoomScript
{
    [SerializeField]
	protected GameObject endPad;
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currRoomState == RoomState.trialing){
            List<Transform> copy = new List<Transform>(playerList);
            if(playerList.Count > 0){
                foreach(Transform thing in playerList)
                {
                    if(thing && !thing.GetComponent<Collider>().bounds.Intersects(transform.GetComponent<Collider>().bounds)){
                        copy.Remove(thing);
                    }
                }
                playerList = copy;
            }

            SetDoorPresence(!(playerList.Count == 0));
        }
        
    }

    new public void OnTriggerEnter(Collider other)
	{
		//Debug.Log(currRoomState);
		//Debug.Log("Collided with: " + other);
		if(other.transform.tag == "Player")
		{
            if(currRoomState == RoomState.empty){
                //Debug.Log("Player found for this room");
                RoomClose();
                //playerRef = other.gameObject;
                StartTrial();
            }

            if(!playerList.Contains(other.transform)){
                playerList.Add(other.transform);
            }
			
		}
		//Debug.Log(doors);
	}

    public override void StartTrial(){
        currRoomState = RoomState.trialing;
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                thing.SetActive(true);
            }
        }
    }

    public override void DespawnTrialGeometry(){
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                SpecialDisable disable = GetComponent<SpecialDisable>();
                if(disable){
                    disable.disable();
                }
                else{
                    thing.SetActive(false);
                }
            }
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
}
