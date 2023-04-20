using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    // Start is called before the first frame update
    
    
    public HealthItem()
    {
        value = 10;
        
        Texture2D texture = Resources.Load("Textures/ItemIcons/torso") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Torso";
        description = "you get more torso meaning more blood and places to take damage so you can take more. gain " + value as string + "more max health";
    }
    public HealthItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/torso") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
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
