using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItem : Item
{
    // Start is called before the first frame update
    
    
    public ArmorItem()
    {
        value = 2;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Can of Beans";
        description = "Have you ever tried to dent a can of beans? you take " + value as string + "less damage";
    }
    public ArmorItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Can of Beans";
        description = "Have you ever tried to dent a can of beans? you take " + value as string + "less damage";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeArmor(value);
        return true;
    }
}
