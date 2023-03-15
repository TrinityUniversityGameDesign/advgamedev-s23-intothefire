using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
  [SerializeField]
  private int dungeonSize = 4; //dungeon will have dungeonSize^2 number of rooms
  //default value is 4 so there won't be any bugs with a 0 size dungeon


  //prefab references
  public GameObject placeholderRoom;
  public GameObject crossRoomHallway;
  public GameObject intersectionHallway;


  


  public List<GameObject> roomList = new List<GameObject>();
  public List<GameObject> crossRoomHallwayList = new List<GameObject>();
  public List<GameObject> intersectionHallwayList = new List<GameObject>();




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

    //spawn rooms
    //GenerateGeometry(placeholderRoom, new Vector3(0,0,0), Quaternion.Euler(0, 0, 0), 120, dungeonSize, dungeonSize, roomList);
    GenerateRooms(120f);

    //spawn cross room hallways
      //horizontal cross room hallways
      GenerateGeometry(crossRoomHallway, new Vector3(60, 0, 0), Quaternion.Euler(0, 0, 0), 120, dungeonSize - 1, dungeonSize, crossRoomHallwayList);

      //vertical cross room hallways
      GenerateGeometry(crossRoomHallway, new Vector3(0, 0, 60), Quaternion.Euler(0, 90, 0), 120, dungeonSize, dungeonSize - 1, crossRoomHallwayList);

    //spawn intersection hallways
    GenerateGeometry(intersectionHallway, new Vector3(60, 0, 60), Quaternion.Euler(0, 0, 0), 120, dungeonSize - 1, dungeonSize - 1, intersectionHallwayList);



  }


  /// <summary>
  /// <para>Takes the object you want to generate </para>
  /// <para>The location of the first generated object</para>
  /// <para>The quaternion rotation for your objects</para>
  /// <para>The distance between objects (this is the same distance in the X and Z)</para>
  /// <para>The number you want to create in the X axis</para>
  /// <para>The number you want to create in the Z axis</para>
  /// <para>And the list you want to add the geometry to for easier reference</para>
  /// </summary>
  private void GenerateGeometry(GameObject prefab, Vector3 startingLocation, Quaternion rotation, int distanceApart, int numToMakeX, int numToMakeZ, List<GameObject> ownerList)
	{
    for (int i = 0; i < numToMakeX; i++)
    {
      for (int j = 0; j < numToMakeZ; j++)
      {

        //spawn prefabs at set intervals
        GameObject newThing = Instantiate(prefab, new Vector3(i * distanceApart + startingLocation.x, 0, j * distanceApart + startingLocation.z), rotation); //If we want a room manager/ hallway manager, this is where we would add objects to it 

        //this is where we would tell the new thing to have a variant based on if it's on the edge of the labyrinth
        

        //add the most recently made object to the appropriate list
        ownerList.Add(newThing);
      }
    }
  }


  private void GenerateRooms(float distanceApart)
  {
    for (int i = 0; i < dungeonSize; i++)
    {
      for (int j = 0; j < dungeonSize; j++)
      {

        //spawn prefabs at set intervals
        GameObject newRoom = Instantiate(placeholderRoom, new Vector3(i * distanceApart, 0, j * distanceApart), Quaternion.Euler(0,0,0)); //If we want a room manager/ hallway manager, this is where we would add objects to it 

        //this is where we would tell the new thing to have a variant based on if it's on the edge of the labyrinth


        //add the most recently made object to the appropriate list
        roomList.Add(newRoom);
      }
    }
  }



}
