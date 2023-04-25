using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum SideEvents
{
  NoEvent,
  Spleef,
  Miniboss
}

public class SideEventController : MonoBehaviour
{

    SideEvents _currentEvent;

    public GameObject spleefControllerPrefab;
    GameObject spawnedSpleefController;

    public GameObject miniBossControllerPrefab;
    GameObject spawnedMinibossController;

    SideEventController_General svc;

    public List<Vector3> preSideEventPlayerPositions = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SideEventBegin.AddListener(StartNewSideEvent);
        GameManager.Instance.SideEventEnd.AddListener(EndCurrentSideEvent);
    }

    void StartNewSideEvent()
    {
        //Debug.Log("Beginning SideEvent from SideEventController");
        

        // Get the number of options in the enum
        int numOptions = System.Enum.GetNames(typeof(SideEvents)).Length;
    
        // Choose a random index between 1 (To avoid NoEvent) and numOptions-1
        int randomIndex = Random.Range(1, numOptions);
    
        // Get the corresponding option from the enum
        _currentEvent = (SideEvents)randomIndex;
    

        //save copies of player position components before players are sent to side event
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            preSideEventPlayerPositions.Add(GameManager.Instance.players[i].transform.position);
        }

        GameManager.Instance.EnablePlayerInvincibility.Invoke();

        switch (_currentEvent)
        {
            case SideEvents.Spleef:
                spawnedSpleefController = Instantiate(spleefControllerPrefab, this.transform);
                break;
            case SideEvents.Miniboss:
                spawnedMinibossController = Instantiate(miniBossControllerPrefab, transform);
                break;
            default:
                break;
        }
        svc = GetComponentInChildren<SideEventController_General>();

        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            GameManager.Instance.players[i].transform.position = svc.spawnPoints[i].position;
        }

        Debug.Log(3);
    }

    void EndCurrentSideEvent()
    {
        int victor = svc.ComputeVictor();
        AwardPlayer(victor);

        //Debug.Log("Ending SideEvent from SideEventController");

        //teleport players back to where they were before side event
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            GameManager.Instance.players[i].transform.position = preSideEventPlayerPositions[i];
        }

        GameManager.Instance.DisablePlayerInvincibility.Invoke();

        switch (_currentEvent)
        {
            case SideEvents.Spleef:
                Destroy(spawnedSpleefController);
                break;
            case SideEvents.Miniboss:
                Destroy(spawnedMinibossController);
                break;
            default:
                break;
        }
        _currentEvent = SideEvents.NoEvent;

    }

    void AwardPlayer(int victor)
    {
        GameManager.Instance.AwardRandomItem(victor);
    }
}
