using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritItem : Item
{
    // Start is called before the first frame update
    
    
    public CritItem()
    {
        value = .05f;
        icon = Resources.Load("Textures/ItemIcons/crit-strike-banner") as Sprite;
        name = "League Crit Effect";
        description = "Huh I didn't know that would transfer between games. Gain " + value as string + " crit chance";
    }
    public CritItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/ItemIcons/crit-strike-banner") as Sprite;
        name = "League Crit Effect";
        description = "Huh I didn't know that would transfer between games. Gain " + value as string + " crit chance";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeCrit(value);
        return true;
    }
}
