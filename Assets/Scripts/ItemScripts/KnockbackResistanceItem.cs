using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackResistanceItem : Item
{
    // Start is called before the first frame update
    
    
    public KnockbackResistanceItem()
    {
        value = 10;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "5 Pound Weight";
        description = "You finnally find a use for all those 5 pound weights you bought, you take " + value as string + "less knockback";
    }
    public KnockbackResistanceItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "5 Pound Weight";
        description = "You finnally find a use for all those 5 pound weights you bought, you take " + value as string + "less knockback";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeKnockbackResistance(value);
        return true;
    }
}
