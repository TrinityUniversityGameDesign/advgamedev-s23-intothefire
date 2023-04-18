using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifegainItem : Item
{
    // Start is called before the first frame update
    
    
    public LifegainItem()
    {
        value = 0.02f;
        
        Texture2D texture = Resources.Load("Textures/ItemIcons/milk") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Milk";
        description = "You have more milk now! don't drink it just look at it and heal  " + value as string + " more per second";
    }
    public LifegainItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/milk") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
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
