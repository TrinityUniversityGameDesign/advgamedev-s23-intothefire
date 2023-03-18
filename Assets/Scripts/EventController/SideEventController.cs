using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideEventController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SideEventBegin.AddListener(StartNewSideEvent);
        GameManager.Instance.SideEventEnd.AddListener(EndCurrentSideEvent);
    }

    void StartNewSideEvent()
    {
        Debug.Log("Beginning SideEvent from SideEventController");
    }

    void EndCurrentSideEvent()
    {
        Debug.Log("Ending SideEvent from SideEventController");
    }
}
