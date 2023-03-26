using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpleefArenaBottomFloor : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.transform.tag == "Player")
		{
			collision.transform.position = new Vector3(collision.transform.position.x, 21, collision.transform.position.z);
		}
	}
}
