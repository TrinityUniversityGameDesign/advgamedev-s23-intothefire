using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : Item
{
    // Start is called before the first frame update
    
    
    public SpeedItem()
    {
        value = 10;
        
        Texture2D texture = Resources.Load("Textures/ItemIcons/foot") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Feet";
        description = "you put these feet on your feet and now you have more feet per feet. you move " + value as string + "faster";
    }
    public SpeedItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/foot") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Feet";
        description = "you put these feet on your feet and now you have more feet per feet. you move " + value as string + "faster";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeSpeed(value);
        return true;
    }
}
