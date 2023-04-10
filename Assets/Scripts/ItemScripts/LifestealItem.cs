using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifestealItem : Item
{
    // Start is called before the first frame update
    
    
    public LifestealItem()
    {
        value = 10;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Sponge";
        description = "You put sponges all over your weapon to soak up the opponents life???? heal  " + value as string + " percent on hit";
    }
    public LifestealItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Sponge";
        description = "You put sponges all over your weapon to soak up the opponents life???? heal  " + value as string + " percent on hit";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeLifesteal(value);
        return true;
    }
}
