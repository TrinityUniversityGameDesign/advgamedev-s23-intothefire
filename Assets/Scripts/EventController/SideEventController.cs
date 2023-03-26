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
