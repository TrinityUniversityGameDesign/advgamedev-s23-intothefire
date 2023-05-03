using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebindSetup : MonoBehaviour
{
    string findPlayer;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        findPlayer = transform.parent.parent.parent.parent.name;
        if (findPlayer.Contains("0"))
        {
            player = GameObject.Find("Player0");
        }
        else if (findPlayer.Contains("1"))
        {
            player = GameObject.Find("Player1");
        }
        else if (findPlayer.Contains("2"))
        {
            player = GameObject.Find("Player2");
        }
        else if (findPlayer.Contains("3"))
        {
            player = GameObject.Find("Player3");
        }
        foreach (Transform child in transform.GetChild(0))
        {
            if(child.gameObject.GetComponent<RebindScript>() != null)
            {
                child.gameObject.GetComponent<RebindScript>().AssignPlayer(player);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
