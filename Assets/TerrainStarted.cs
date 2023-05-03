using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainStarted : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.StartupNewGameBegin.AddListener(TurnOnTerrain);
    }

    void TurnOnTerrain()
    {
        gameObject.GetComponent<Terrain>().enabled = true;
    }
    
}
