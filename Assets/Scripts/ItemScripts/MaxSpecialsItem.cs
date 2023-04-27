using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpecialsItem : Item
{
    // Start is called before the first frame update
    
    
    public MaxSpecialsItem()
    {
        value = 1;
        
        Texture2D texture = Resources.Load("Textures/ItemIcons/dragon") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Dragon Sphere";
        description = "You find a non tv show specific sphere which lets you use your special " + value as string + "more times";
    }
    public MaxSpecialsItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/dragon") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
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
