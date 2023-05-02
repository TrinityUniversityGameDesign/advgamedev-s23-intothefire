using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSideEventController : SideEventController_General
{
    public override List<int> ComputeVictor()
    {
    List<int> winner = new List<int>();
    winner.Add(GetComponentInChildren<MiniBossAi_Old>().GetWinner());
    return winner;
  }
}
