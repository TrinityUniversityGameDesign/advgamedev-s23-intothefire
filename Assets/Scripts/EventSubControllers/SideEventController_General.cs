using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideEventController_General : MonoBehaviour
{
    public List<Transform> spawnPoints;

    public int victor = -1;

    public virtual List<int> ComputeVictor() { return new List<int>(-2); }
}
