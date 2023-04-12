using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifegainItem : Item
{
    // Start is called before the first frame update
    
    
    public LifegainItem()
    {
        value = 0.02f;
        icon = Resources.Load("Textures/ItemIcons/milk") as Sprite;
        name = "Milk";
        description = "You have more milk now! don't drink it just look at it and heal  " + value as string + " more per second";
    }
    public LifegainItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/ItemIcons/milk") as Sprite;
        name = "Milk";
        description = "You have more milk now! don't drink it just look at it and heal  " + value as string + " more per second";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeLifegain(value);
        return true;
    }
}
