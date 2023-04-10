using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedItem : Item
{
    // Start is called before the first frame update
    
    
    public AttackSpeedItem()
    {
        value = .02f;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Energy Drink";
        description = "you pour this over your weapon?????? and you attack " + value as string + "faster";
    }
    public AttackSpeedItem(float val)
    {
        value = val;
        icon = Resources.Load("Textures/grud") as Sprite;
        name = "Energy Drink";
        description = "you pour this over your weapon?????? and you attack " + value as string + "faster";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeAttackSpeed(value);
        return true;
    }
}
