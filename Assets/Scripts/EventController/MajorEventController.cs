using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorEventController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.MajorEventBegin.AddListener(StartNewMajorEvent);
        GameManager.Instance.MajorEventEnd.AddListener(EndCurrentMajorEvent);
    }

    void StartNewMajorEvent()
    {
        Debug.Log("Beginning MajorEvent from MajorEventController");
    }

    void EndCurrentMajorEvent()
    {
        Debug.Log("Ending MajorEvent from MajorEventController");
    }
}
