using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : Item
{
    // Start is called before the first frame update
    
    
    public SpeedItem()
    {
        value = 10;
        icon = Resources.Load("Textures/ItemIcons/foot") as Sprite;
        name = "Feet";
        description = "you put these feet on your feet and now you have more feet per feet. you move " + value as string + "faster";
    }
    public SpeedItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/ItemIcons/foot") as Sprite;
        name = "Feet";
        description = "you put these feet on your feet and now you have more feet per feet. you move " + value as string + "faster";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeSpeed(value);
        return true;
    }
}
