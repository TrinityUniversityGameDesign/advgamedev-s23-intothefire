using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum MicroEvents
{
    NoEvent,
    Meteor
}

public class MicroEventController : MonoBehaviour
{

    MicroEvents _currentEvent;

    public GameObject meteorControllerPrefab;
    GameObject spawnedMeteorController;



    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.MicroEventBegin.AddListener(StartNewMicroEvent);
        GameManager.Instance.MicroEventEnd.AddListener(EndCurrentMicroEvent);
    }

    void StartNewMicroEvent()
    {
        Debug.Log("Beginning MicroEvent from MicroEventController");

        // Get the number of options in the enum
        int numOptions = System.Enum.GetNames(typeof(MicroEvents)).Length;

        // Choose a random index between 1 (To avoid NoEvent) and numOptions-1
        int randomIndex = Random.Range(1, numOptions);

        // Get the corresponding option from the enum
        _currentEvent = (MicroEvents)randomIndex;

        Debug.Log("Chosen option: " + _currentEvent);

        switch (_currentEvent)
        {
            case MicroEvents.Meteor:
                spawnedMeteorController = Instantiate(meteorControllerPrefab, this.transform);
                GameManager.Instance.CurrentEvent = GameEvents.Meteor;
                break;
            default:
                break;
        }
    }

    void EndCurrentMicroEvent()
    {
        //Debug.Log("Ending MicroEvent from MicroEventController");
        switch (_currentEvent)
        {
            case MicroEvents.Meteor:
                Destroy(spawnedMeteorController);
                break;
            default:
                break;
        }

        _currentEvent = MicroEvents.NoEvent;
    }

}
