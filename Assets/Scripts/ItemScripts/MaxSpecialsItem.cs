using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxJumpsItem : Item
{
    // Start is called before the first frame update
    
    
    public MaxJumpsItem()
    {
        value = 1;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Invisible Floor";
        description = "you find another invisible floor so you can jump " + value as string + "more times";
    }
    public MaxJumpsItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Invisible Floor";
        description = "you find another invisible floor so you can jump " + value as string + "more times";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeMaxJumps(value);
        return true;
    }
}
