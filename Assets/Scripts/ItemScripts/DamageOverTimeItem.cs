using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeItem : Item
{
    // Start is called before the first frame update
    
    
    public DamageOverTimeItem()
    {
        value = 2;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Garlic";
        description = "You put garlic all over your weapon and it stings for " + value as string + "damage over 10 seconds";
    }
    public DamageOverTimeItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Garlic";
        description = "You put garlic all over your weapon and it stings for " + value as string + "damage over 10 seconds";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeDamageOverTime(value);
        return true;
    }
}
