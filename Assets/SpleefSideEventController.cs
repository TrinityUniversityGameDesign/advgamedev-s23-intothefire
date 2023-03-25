using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpleefSideEventController : MonoBehaviour
{
  [SerializeField]
  public int spleefLength = 10;
  [SerializeField]
  GameObject spleefPlatform;


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
        Instantiate(spleefPlatform, new Vector3(i * 5, -3, j * 5), Quaternion.Euler(0, 0, 0));
      }
    }
	}

}
