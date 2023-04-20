using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public void InitializeStats(List<(string, float)> stats, GameObject statPrefab)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            GameObject newObject = Instantiate(statPrefab, transform);
            newObject.gameObject.GetComponent<TMP_Text>().text = stats[i].Item1 + ": " + stats[i].Item2;
        }
    }
}
