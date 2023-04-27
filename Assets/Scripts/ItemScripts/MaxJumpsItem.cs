using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxJumpsItem : Item
{
    // Start is called before the first frame update
    
    
    public MaxJumpsItem()
    {
        value = 1;
        
        Texture2D texture = Resources.Load("Textures/ItemIcons/floor") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Invisible Floor";
        description = "you find another invisible floor so you can jump " + value as string + "more times";
    }
    public MaxJumpsItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/floor") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
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
