using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

	public Vector3 destination;

	public void SetDestination(Vector3 dest)
	{
		destination = dest;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<Transform>().position = destination;
		}
	}


}
