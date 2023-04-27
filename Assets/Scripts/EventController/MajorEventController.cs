using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum MajorEvents
{
    NoEvent,
    Minotaur
}

public class MajorEventController : MonoBehaviour
{

    MajorEvents _currentEvent;

    public GameObject roamingBossPrefab;
    GameObject roamingBoss;
    [SerializeField] Transform startPoint;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.MajorEventBegin.AddListener(StartNewMajorEvent);
        GameManager.Instance.MajorEventEnd.AddListener(EndCurrentMajorEvent);

        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;
        Debug.Log("Found Destination: " + startPoint.position);
    }

    void StartNewMajorEvent()
    {
        Debug.Log("Beginning MajorEvent from MajorEventController");
        //Debug.Log("Beginning MicroEvent from MicroEventController");

        // Get the number of options in the enum
        int numOptions = System.Enum.GetNames(typeof(MajorEvents)).Length;

        // Choose a random index between 1 (To avoid NoEvent) and numOptions-1
        int randomIndex = Random.Range(1, numOptions);

        // Get the corresponding option from the enum
        _currentEvent = (MajorEvents)randomIndex;

        Debug.Log("Chosen option: " + _currentEvent);

        switch (_currentEvent)
        {
            case MajorEvents.Minotaur:
                roamingBoss = Instantiate(roamingBossPrefab, startPoint.position, Quaternion.identity, this.transform);
                Debug.Log("Minotaur location: " + roamingBoss.transform.position);
                break;
            default:
                break;
        }
    }

    void EndCurrentMajorEvent()
    {
        //Debug.Log("Ending MajorEvent from MajorEventController");
    }
}
