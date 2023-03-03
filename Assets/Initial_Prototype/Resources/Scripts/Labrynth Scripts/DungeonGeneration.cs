using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
  [SerializeField]
  private int dungeonSize = 4; //dungeon will have dungeonSize^2 number of rooms
  //default value is 4 so there won't be any bugs with a 0 size dungeon


  //reference to room prefabs
  public GameObject emptyRoom;
  public GameObject room1x2;
  public GameObject room2x2;
  


  public List<GameObject> trialRooms = new List<GameObject>();
  public List<GameObject> teleporterRooms = new List<GameObject>();



  // Start is called before the first frame update
  void Start()
  {
    //just in case you forget to specify dungeonSize in the editor, generates a minimum of 16 rooms so there's no errors
    if(dungeonSize <= 4)
		{
      dungeonSize = 4;
		}
    GenerateDungeon();
  }

  public void GenerateDungeon()
	{

    //create trial rooms
    GenerateTrialRooms();
  }

  private void GenerateTrialRooms()
	{
    for (int i = 0; i < dungeonSize; i++)
    {
      for (int j = 0; j < dungeonSize; j++)
      {
        if ((i == 0 && j == 0) || (i == dungeonSize-1 && j == 0) || (i == 0 && j == dungeonSize-1) || (i == dungeonSize-1 && j == dungeonSize-1))//set corner rooms to empty rooms
        {
          Instantiate(emptyRoom, new Vector3(i * 100f, 0f, j * 100f), new Quaternion(0, 0, 0, 0), this.transform);
        }
        else 
        {
          int roomType = Random.Range(1, 3);

          //randomly rotates a room 90 degrees about the y axis
          //0.071 is the rotation needed in the x and z parameters for a 90 degree rotation in a quaternion
          int rotateOrNot = Random.Range(1, 3);
          float rotation = 0.7071f * (rotateOrNot % 2);
  
          if (roomType == 1)
          {
            Instantiate(room1x2, new Vector3(i * 100f, 0f, j * 100f), new Quaternion(0, rotation, 0, rotation), this.transform);
          }
          if (roomType == 2)
          {
            Instantiate(room2x2, new Vector3(i * 100f, 0f, j * 100f), new Quaternion(0, 0, 0, 0), this.transform);
          }
        }
        trialRooms.Add(transform.GetChild(i * dungeonSize + j).gameObject);
      }
    }
  }


}
