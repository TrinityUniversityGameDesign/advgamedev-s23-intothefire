using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHeightItem : Item
{
    // Start is called before the first frame update
    
    
    public JumpHeightItem()
    {
        value = 0.3f;
        
        Texture2D texture = Resources.Load("Textures/ItemIcons/spring") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Spring";
        description = "You take appart a pen and put the spring on your foot, letting you jump " + value as string + "higher";
    }
    public JumpHeightItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/spring") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Spring";
        description = "You take appart a pen and put the spring on your foot, letting you jump " + value as string + "higher";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeJumpHeight(value);
        return true;
    }
}
