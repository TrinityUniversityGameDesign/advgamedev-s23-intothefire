using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	public TrialRoomScript hostRoom;

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.transform.tag == "Player")
		{
			hostRoom.DecrementEnemyCount();

			Destroy(gameObject);
			Debug.Log("Enemy died.");
		}
	}
}
