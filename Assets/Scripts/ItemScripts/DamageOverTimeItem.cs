using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeItem : Item
{
    // Start is called before the first frame update
    
    
    public DamageOverTimeItem()
    {
        value = 2;
        Texture2D texture = Resources.Load("Textures/ItemIcons/garlic") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        
        name = "Garlic";
        description = "You put garlic all over your weapon and it stings for " + value as string + "damage over 10 seconds";
    }
    public DamageOverTimeItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/garlic") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
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
