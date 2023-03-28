using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    // Start is called before the first frame update
    public float value = 0f;
    public float time = 0f;
    public float timer = 0f;
    public GameObject player;
    public Sprite icon;
    public string name = "";
    public string description = "";
    public Item()
    {

    }
    public Item(GameObject selectedPlayer)
    {
        player = selectedPlayer;
    }

    public Item(GameObject selectedPlayer, float itemValue)
    {
        player = selectedPlayer;
        value = itemValue;
    }
    public Item(GameObject selectedPlayer,float itemValue, float iterationTime)
    {
        player = selectedPlayer;
        value = itemValue;
        time = iterationTime;
    }

    public Item(float itemValue)
    {
        value = itemValue;
    }
    
    public Item(float itemValue, float iterationTime)
    {
        value = itemValue;
        time = iterationTime;
    }

    // Update is called once per frame
    public bool UpdateItem()
    {
        timer++;
        if(timer > time)
        {
            timer = 0f;
            ItemCooldown();
        }
        return false;
    }

    public bool ItemCooldown()
    {
        return false;
    }
    public bool ItemPickup()
    {
        return false;
    }
    public bool ItemJump()
    {
        return false;
    }
    public bool ItemMove()
    {
        return false;
    }
    public bool ItemSpecial()
    {
        return false;
    }
    public bool ItemLight()
    {
        return false;
    }
    public bool ItemHeavy()
    {
        return false;
    }

    public void AssignPlayer(GameObject play)
    {
        player = play;
    }
}
