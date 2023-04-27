using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpleefArenaBottomFloor : MonoBehaviour
{
	[SerializeField]
	SpleefSideEventController controller;



	private void OnTriggerEnter(Collider other)
	{
			if(other.transform.tag == "Player")
			{
				other.transform.position = new Vector3(other.transform.position.x, gameObject.transform.position.y + 81, other.transform.position.z);
				controller.removeVictor(other.gameObject);
			}
		
	}
}
