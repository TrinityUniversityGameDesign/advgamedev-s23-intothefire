using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : MonoBehaviour
{
    public TrialRoomScript hostRoom;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Player")
		{
            Destroy(gameObject);
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
