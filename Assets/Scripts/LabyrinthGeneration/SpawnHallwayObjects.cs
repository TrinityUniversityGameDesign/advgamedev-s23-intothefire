using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHallwayObjects : MonoBehaviour
{

	public GameObject hallwayObject;
	
	[SerializeField]
	private List<Transform> spawnpoints = new List<Transform>();
	//A hallway's spawnpoints are set in the inspector while editing the prefab
	
	private void Start()
	{
		SpawnHallwayObject();
	}


	private void SpawnHallwayObject()
	{
		int numOfSpawnPoints = spawnpoints.Count;
		int randomIndex = Random.Range(0, numOfSpawnPoints+1);
		//This gives a 1 in (numOfSpawnPoints +1) chance that there won't be a hallway object in this hallway.
		//This is where we would put in any global % chance stuff for hallway object generation, such as outer hallways are safer, more centeral hallways are more dangerous, etc
		if (randomIndex != numOfSpawnPoints)
		{
			Instantiate(hallwayObject, spawnpoints[Random.Range(0, numOfSpawnPoints - 1)].position, gameObject.transform.rotation, this.transform);
		}
	}


}
