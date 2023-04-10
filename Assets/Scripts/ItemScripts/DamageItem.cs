using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageItem : Item
{
    // Start is called before the first frame update
    
    
    public DamageItem()
    {
        value = 5;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "spikeyBall";
        description = "putting this spikey ball on your weapon should make it hurt some more. It does +" + value as string + "more damage now";
    }
    public DamageItem(float val)
    {
        value = val;
        icon = Resources.Load("Prefabs/Textures/grud") as Sprite;
        name = "spikeyBall";
        description = "putting this spikey ball on your weapon should make it hurt some more. It does +" + value as string + "more damage now";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeDamage(value);
        return true;
    }
}
