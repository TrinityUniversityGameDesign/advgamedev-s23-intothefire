using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class DungeonGeneration : MonoBehaviour
{
    //[Header("Labyrinth Size (MUST BE ODD)")]
    //[SerializeField]
    private int labyrinthSize = 5; //dungeon will have labyrinthSize^2 number of rooms
    //default value is 5 so there won't be any bugs with a 0 size dungeon
    //[SerializeField]
    private int distanceApart = 120;



    //prefab references
    [Header("Labyrinth Prefabs")]
    public List<GameObject> placeholderRoom;
    public GameObject placeholderCenterRoom;
    public GameObject sorryPurpleTeam;
    public GameObject intersectionHallway;
    public GameObject crossRoomHallway;
    public GameObject centerRoomHallwayNegative;
    public GameObject centerRoomHallwayPositive;

    [HideInInspector]
    public Transform GeometryHolder;

    [Header("Labyrinth Geometry Lists")]
    public List<GameObject> roomList = new List<GameObject>();
    public List<GameObject> crossRoomHallwayList = new List<GameObject>();
    public List<GameObject> intersectionHallwayList = new List<GameObject>();

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start() {
        GeometryHolder = transform.GetChild(0);

        //just in case you forget to specify labyrinthSize in the editor, generates a minimum of 25 rooms so there's no errors
        if(GameManager.Instance == null)
        {
            Debug.LogError("Game Manager Instance was not set, check to make sure you have a game manager in the scene");
            return;
        }
        labyrinthSize = (int)(GameManager.Instance?.LabyrinthSize);
        distanceApart = (int)(GameManager.Instance?.DistanceApart);

        //If LabyrinthSize in the Game Manager is even lets make it odd. 
        if(labyrinthSize % 2 == 0)
        {
            labyrinthSize += 1;
        }

        if (labyrinthSize <= 5) {
            labyrinthSize = 5;
        }

        //Call Labyrinth generation on first time startup rather than when the comonent starts. This might want to be bound to LabyrinthExplore, I am not sure, or a loading state in between
        GameManager.Instance?.StartupNewGameBegin.AddListener(GenerateLabyrinth);
        GameManager.Instance.ShowdownBegin.AddListener(RemoveLabyrinth);
        //GenerateLabyrinth();
    }

    public void RemoveLabyrinth()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void GenerateLabyrinth() {
        //Debug.Log("Generating Labyrinth");
        //spawn rooms
        GenerateRooms();

        //spawn cross room hallways and hallways connecting to center room 
        GenerateCrossRoomHallways();

        //spawn intersection hallways
        GenerateIntersectionHallways();
    }

    private void GenerateRooms()
    {
        for (int i = 0; i < labyrinthSize; i++)
        {
            for (int j = 0; j < labyrinthSize; j++)
            {

            //if we are at the location of the center room
            if (i == Mathf.CeilToInt(labyrinthSize / 2) && j == Mathf.CeilToInt(labyrinthSize / 2))
            {
                GameObject centerRoom = Instantiate(placeholderCenterRoom,
                new Vector3(i * distanceApart, 0, j * distanceApart),
                Quaternion.Euler(0, 0, 0));
                roomList.Add(centerRoom);
            }
            else
            {
                //spawn prefabs at set intervals
                int rand = Random.Range(0, placeholderRoom.Count);
                GameObject newRoom;
                
                    newRoom = Instantiate(placeholderRoom[rand],
                    new Vector3(i * distanceApart, 0, j * distanceApart),
                    Quaternion.Euler(0, 0, 0),
                    GeometryHolder.transform);
               

                //this is where we would tell the new thing to have a variant based on if it's on the edge of the labyrinth


                //add the most recently made object to the appropriate list
                roomList.Add(newRoom);
            }
            }
        }
    }


    private void GenerateIntersectionHallways()
    {
        for (int i = 0; i < labyrinthSize - 1; i++)
        {
            for (int j = 0; j < labyrinthSize - 1; j++)
            {

            //spawn prefabs at set intervals
            GameObject newThing = Instantiate(intersectionHallway,
                new Vector3(i * distanceApart + distanceApart / 2, 0, j * distanceApart + distanceApart / 2),
                Quaternion.Euler(0, 0, 0),
                GeometryHolder.transform);

            //this is where we would tell the new thing to have a variant based on if it's on the edge of the labyrinth


            //add the most recently made object to the appropriate list
            intersectionHallwayList.Add(newThing);


            }
        }
    }


    private void GenerateCrossRoomHallways()
    {
        GenerateHorizontalHallways();
        GenerateVerticalHallways();
    }
    private void GenerateHorizontalHallways()
    {
        for (int i = 0; i < labyrinthSize -1; i++)
        {
            for (int j = 0; j < labyrinthSize; j++)
            {

            GameObject hallwayToSpawn;
            //this is where we would tell the new thing to have a variant based on if it's on the edge of the labyrinth
        
            //if the hallway we need to place is attached to the center room, place a center room hallway
            if((i == (Mathf.FloorToInt((labyrinthSize - 2)/2f)) && j == Mathf.FloorToInt(labyrinthSize/2)))
			        {
                //Spawn - Center Hallway East
                hallwayToSpawn = centerRoomHallwayNegative;
            }
            else if((i == (Mathf.CeilToInt((labyrinthSize - 2)/2f))) && j == Mathf.FloorToInt(labyrinthSize / 2))
			        {
                //spawn + Center Hallway West
                hallwayToSpawn = centerRoomHallwayPositive;
            }
				
            else
			        {
                //spawn crossroom hallway
                hallwayToSpawn = crossRoomHallway;
            }
        
            GameObject newHallway = Instantiate(hallwayToSpawn,
                new Vector3(i * distanceApart + distanceApart/2, 0, j * distanceApart + 0),
                Quaternion.Euler(0, 0, 0),
                GeometryHolder.transform);

            //add the most recently made object to the appropriate list
            crossRoomHallwayList.Add(newHallway);
            }
        }
    }
    private void GenerateVerticalHallways()
    {
        for (int i = 0; i < labyrinthSize; i++)
        {
            for (int j = 0; j < labyrinthSize - 1; j++)
            {

            GameObject hallwayToSpawn;
            //this is where we would tell the new thing to have a variant based on if it's on the edge of the labyrinth

            //if the hallway we need to place is attached to the center room, place a center room hallway
            if ((j == (Mathf.FloorToInt((labyrinthSize - 2)/2f)) && i == Mathf.FloorToInt(labyrinthSize / 2)))
            {
                //Spawn - Center Hallway South
                hallwayToSpawn = centerRoomHallwayNegative;
            }
            else if ((j == (Mathf.CeilToInt((labyrinthSize - 2)/2f))) && i == Mathf.FloorToInt(labyrinthSize / 2))
            {
                //spawn + Center Hallway North
                hallwayToSpawn = centerRoomHallwayPositive;
            }
        
            else
            {
                //spawn crossroom hallway
                hallwayToSpawn = crossRoomHallway;
            }

            GameObject newHallway = Instantiate(hallwayToSpawn,
                new Vector3(i * distanceApart, 0, j * distanceApart + distanceApart/2),
                Quaternion.Euler(0, -90, 0),
                GeometryHolder.transform);

            //add the most recently made object to the appropriate list
            crossRoomHallwayList.Add(newHallway);
            }
        }
    }




    //General spawning function
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

}
