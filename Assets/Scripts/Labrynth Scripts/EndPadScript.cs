using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPadScript : MonoBehaviour
{
	public TrialRoomScript hostRoom;

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Player")
		{
			Debug.Log("Player Trigger Detected");
			hostRoom.TrialCompleted();

			if(other.gameObject.name == "Player0"){
                GameObject manager = GameObject.Find("Global_GameManager");
                if(manager){
                    manager.GetComponent<GameManager>().AwardRandomItem(0);
                }
            }
            else if(other.gameObject.name == "Player1"){
                GameObject manager = GameObject.Find("Global_GameManager");
                if(manager){
                    manager.GetComponent<GameManager>().AwardRandomItem(1);
                }
            }
            else if(other.gameObject.name == "Player2"){
                GameObject manager = GameObject.Find("Global_GameManager");
                if(manager){
                    manager.GetComponent<GameManager>().AwardRandomItem(2);
                }
            }
            else if(other.gameObject.name == "Player3"){
                GameObject manager = GameObject.Find("Global_GameManager");
                if(manager){
                    manager.GetComponent<GameManager>().AwardRandomItem(3);
                }
            }
		}
	}
}
