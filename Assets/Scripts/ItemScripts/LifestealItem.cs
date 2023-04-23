using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifestealItem : Item
{
    // Start is called before the first frame update
    
    
    public LifestealItem()
    {
        value = 0.1f;
        
        Texture2D texture = Resources.Load("Textures/ItemIcons/bath_sponges") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Sponge";
        description = "You put sponges all over your weapon to soak up the opponents life???? heal  " + value as string + " percent on hit";
    }
    public LifestealItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/bath_sponges") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Sponge";
        description = "You put sponges all over your weapon to soak up the opponents life???? heal  " + value as string + " percent on hit";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeLifesteal(value);
        return true;
    }
}