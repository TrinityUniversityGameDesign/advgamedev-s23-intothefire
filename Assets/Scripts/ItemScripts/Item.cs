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

    // Update is called once per frame
    public void UpdateItem()
    {
        timer++;
        if(timer > time)
        {
            timer = 0f;
            ItemCooldown();
        }
    }

    public void ItemCooldown()
    {

    }
    public void ItemPickup()
    {

    }
    public void ItemJump()
    {

    }
    public void ItemMove()
    {

    }
    public void ItemSpecial()
    {

    }
    public void ItemLight()
    {

    }
    public void ItemHeavy()
    {

    }

    public void AssignPlayer(GameObject play)
    {
        player = play;
    }
}
