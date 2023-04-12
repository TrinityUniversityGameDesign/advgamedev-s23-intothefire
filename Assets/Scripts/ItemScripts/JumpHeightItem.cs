using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHeightItem : Item
{
    // Start is called before the first frame update
    
    
    public JumpHeightItem()
    {
        value = 0.3f;
        icon = Resources.Load("Textures/ItemIcons/spring") as Sprite;
        name = "Spring";
        description = "You take appart a pen and put the spring on your foot, letting you jump " + value as string + "higher";
    }
    public JumpHeightItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/ItemIcons/spring") as Sprite;
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
