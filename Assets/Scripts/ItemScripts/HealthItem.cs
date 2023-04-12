using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    // Start is called before the first frame update
    
    
    public HealthItem()
    {
        value = 10;
        icon = Resources.Load("Textures/ItemIcons/torso") as Sprite;
        name = "Torso";
        description = "you get more torso meaning more blood and places to take damage so you can take more. gain " + value as string + "more max health";
    }
    public HealthItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/ItemIcons/torso") as Sprite;
        name = "Torso";
        description = "you get more torso meaning more blood and places to take damage so you can take more. gain " + value as string + "more max health";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeHealth(value);
        return true;
    }
}
