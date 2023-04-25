using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    /// <summary>
    ///  Instantiate the list of stats in the inventory view
    /// </summary>
    /// <param name="stats">list of stats from player</param>
    /// <param name="statPrefab">prefab for the stat object</param>
    public void InitializeStats(List<(string, float)> stats, GameObject statPrefab)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            GameObject newObject = Instantiate(statPrefab, transform);
            newObject.gameObject.GetComponent<TMP_Text>().text = stats[i].Item1 + ": " + stats[i].Item2;
        }
    }
    
    public void InitializeStats(PlayerStats stats, GameObject statPrefab)
    {
        int space = 0;
        foreach (var (k, v) in stats.Stats)
        {
            GameObject newObject = Instantiate(statPrefab, transform);
            newObject.gameObject.GetComponent<TMP_Text>().text = k + ": " + v.TotalValue() + " + " + v.BonusValue();
        }
    }
}
