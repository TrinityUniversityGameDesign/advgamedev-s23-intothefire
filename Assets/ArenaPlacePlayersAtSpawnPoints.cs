using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPlacePlayersAtSpawnPoints : MonoBehaviour
{
    public List<Transform> spawns = new List<Transform>(4);

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            GameManager.Instance.players[i].transform.position = spawns[i].position;
        }
    }

}
