using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MicroEventController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.MicroEventBegin.AddListener(StartNewMicroEvent);
        GameManager.Instance.MicroEventEnd.AddListener(EndCurrentMicroEvent);
    }

    void StartNewMicroEvent()
    {
        Debug.Log("Beginning MicroEvent from MicroEventController");
    }

    void EndCurrentMicroEvent()
    {
        Debug.Log("Ending MicroEvent from MicroEventController");
    }

}
