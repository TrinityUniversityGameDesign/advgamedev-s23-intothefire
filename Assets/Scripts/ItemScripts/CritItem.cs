using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritItem : Item
{
    // Start is called before the first frame update
    
    
    public CritItem()
    {
        value = .05f;

        Texture2D texture = Resources.Load("Textures/ItemIcons/crit-strike-banner") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "League Crit Effect";
        description = "Huh I didn't know that would transfer between games. Gain " + value as string + " crit chance";
    }
    public CritItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/crit-strike-banner") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
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
