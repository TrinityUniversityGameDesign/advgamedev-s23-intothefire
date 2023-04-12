using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum SideEvents
{
  NoEvent,
  Spleef
}

public class SideEventController : MonoBehaviour
{

  SideEvents _currentEvent;

  public GameObject spleefControllerPrefab;
  GameObject spawnedSpleefController;
  public List<Vector3> preSideEventPlayerPositions = new List<Vector3>();


  // Start is called before the first frame update
  void Start()
    {
        GameManager.Instance.SideEventBegin.AddListener(StartNewSideEvent);
        GameManager.Instance.SideEventEnd.AddListener(EndCurrentSideEvent);
    }

    void StartNewSideEvent()
    {
        Debug.Log("Beginning SideEvent from SideEventController");

        // Get the number of options in the enum
        int numOptions = System.Enum.GetNames(typeof(SideEvents)).Length;
    
        // Choose a random index between 1 (To avoid NoEvent) and numOptions-1
        int randomIndex = Random.Range(1, numOptions);
    
        // Get the corresponding option from the enum
        _currentEvent = (SideEvents)randomIndex;
    
        Debug.Log("Chosen option: " + _currentEvent);

        //save copies of player position components before players are sent to side event
        for (int i = 0; i < 4; i++)
        {
          if (i+1 <= GameManager.Instance.players.Count)
          {
            preSideEventPlayerPositions.Add(GameManager.Instance.players[i].transform.position);
          }
        }



    switch (_currentEvent)
        {
          case SideEvents.Spleef:
            spawnedSpleefController = Instantiate(spleefControllerPrefab, this.transform);
            break;
          default:
            break;
        }
  }

    void EndCurrentSideEvent()
    {
        Debug.Log("Ending SideEvent from SideEventController");

        //teleport players back to where they were before side event
        for (int i = 0; i < 4; i++)
        {
          if (i + 1 <= GameManager.Instance.players.Count)
          {
            GameManager.Instance.players[i].transform.position = preSideEventPlayerPositions[i];
          }
        }

        switch (_currentEvent)
        {
          case SideEvents.Spleef:
            Destroy(spawnedSpleefController);
            break;
          default:
            break;
        }
    
        _currentEvent = SideEvents.NoEvent;

  }
}
