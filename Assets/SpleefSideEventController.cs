using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpleefSideEventController : MonoBehaviour
{
  [SerializeField]
  public int spleefLength = 10;
  [SerializeField]
  private GameObject spleefPlatform;

  public Transform P1SpawnPoint;
  public Transform P2SpawnPoint;
  public Transform P3SpawnPoint;
  public Transform P4SpawnPoint;

  [SerializeField]
  private GameObject platformHolder;


  // Start is called before the first frame update
  void Start()
  {
    SpawnPlatforms();
  }

  
  
  private void SpawnPlatforms()
	{
    for(int i = -spleefLength; i < spleefLength; i++)
		{
      for (int j = -spleefLength; j < spleefLength; j++)
      {
        Instantiate(spleefPlatform, new Vector3(i * 5 + 2.5f, -3, j * 5 + 2.5f), Quaternion.Euler(0, 0, 0), platformHolder.transform);
        Instantiate(spleefPlatform, new Vector3(i * 5 + 2.5f, -23, j * 5 + 2.5f), Quaternion.Euler(0, 0, 0), platformHolder.transform);
        Instantiate(spleefPlatform, new Vector3(i * 5 + 2.5f, -43, j * 5 + 2.5f), Quaternion.Euler(0, 0, 0), platformHolder.transform);
      }
    }
	}




}
