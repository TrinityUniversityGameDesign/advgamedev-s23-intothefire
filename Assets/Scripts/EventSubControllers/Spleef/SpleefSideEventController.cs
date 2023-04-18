using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpleefSideEventController : SideEventController_General
{
  [SerializeField]
  public int spleefLength = 10;
  [SerializeField]
  private GameObject spleefPlatform;

  //public Transform P1SpawnPoint;
  //public Transform P2SpawnPoint;
  //public Transform P3SpawnPoint;
  //public Transform P4SpawnPoint;

  [SerializeField]
  private GameObject platformHolder;

  float ourX;
  float ourY;
  float ourZ;


  // Start is called before the first frame update
  void Start()
  {
    ourX = gameObject.transform.position.x;
    ourY = gameObject.transform.position.y;
    ourZ = gameObject.transform.position.z;

    //List<Transform> spawnPoints = new List<Transform>();
    //spawnPoints.Add(P1SpawnPoint);
    //spawnPoints.Add(P2SpawnPoint);
    //spawnPoints.Add(P3SpawnPoint);
    //spawnPoints.Add(P4SpawnPoint);

        //Teleporting players is handled by the event controller. 
  //  for (int i = 0; i < 4; i++)
		//{
  //    if(i + 1 <= GameManager.Instance.players.Count)
		//	{
  //      Debug.Log("Before: " + GameManager.Instance.players[i].transform.position);
  //      GameManager.Instance.players[i].transform.position = spawnPoints[i].position;
  //      Debug.Log("After: " + GameManager.Instance.players[i].transform.position);

  //      //Error being caused by Player+Camera Object transform.position being updated, but the actual player part is being teleported wierdly in relation to it's parent object
		//	}
		//}


    SpawnPlatforms();
  }

  
  
  private void SpawnPlatforms()
	{
    for(int i = -spleefLength; i < spleefLength; i++)
		{
      for (int j = -spleefLength; j < spleefLength; j++)
      {
        Instantiate(spleefPlatform, new Vector3(ourX + i * 5 + 2.5f, ourY -3, ourZ + j * 5 + 2.5f), Quaternion.Euler(0, 0, 0), platformHolder.transform);
        Instantiate(spleefPlatform, new Vector3(ourX + i * 5 + 2.5f, ourY - 23, ourZ + j * 5 + 2.5f), Quaternion.Euler(0, 0, 0), platformHolder.transform);
        Instantiate(spleefPlatform, new Vector3(ourX + i * 5 + 2.5f, ourY - 43, ourZ + j * 5 + 2.5f), Quaternion.Euler(0, 0, 0), platformHolder.transform);
      }
    }
	}


    public override int ComputeVictor()
    {
        //TODO @Drew
        return base.ComputeVictor();
    }

}
