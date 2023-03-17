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
		}
	}
}
