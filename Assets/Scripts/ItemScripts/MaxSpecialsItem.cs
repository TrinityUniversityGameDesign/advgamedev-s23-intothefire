using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpecialsItem : Item
{
    // Start is called before the first frame update
    
    
    public MaxSpecialsItem()
    {
        value = 1;
        icon = Resources.Load("Textures/ItemIcons/dragon") as Sprite;
        name = "Dragon Sphere";
        description = "You find a non tv show specific sphere which lets you use your special " + value as string + "more times";
    }
    public MaxSpecialsItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/ItemIcons/dragon") as Sprite;
        name = "Dragon Sphere";
        description = "You find a non tv show specific sphere which lets you use your special " + value as string + "more times";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeMaxSpecials(value);
        return true;
    }
}
