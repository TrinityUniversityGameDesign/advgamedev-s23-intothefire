using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSideEventController : SideEventController_General
{
    public override int ComputeVictor()
    {
        return GetComponentInChildren<MiniBossAi>().GetWinner();
    }
}
