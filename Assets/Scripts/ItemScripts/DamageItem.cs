using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageItem : Item
{
    // Start is called before the first frame update
    
    void start()
    {
        value = 10;
        icon = Resources.Load("Prefabs/Textures/grud") as Sprite;
        name = "spikeyBall";
        description = "putting this spikey ball on your weapon should make it hurt some more. It does +" + value as string + "more damage now";
    }
    

    // Update is called once per frame
   public new bool ItemPickup()
    {
        player.GetComponent<JacksonPlayerMovement>().ChangeDamage(value);
        return true;
    }
}
