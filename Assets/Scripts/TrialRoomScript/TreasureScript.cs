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
		}
	}
}
