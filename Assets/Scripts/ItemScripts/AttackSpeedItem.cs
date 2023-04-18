using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedItem : Item
{
    // Start is called before the first frame update
    
    
    public AttackSpeedItem()
    {
        value = .02f;
        Texture2D texture = Resources.Load("Textures/ItemIcons/energySword") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Energy Drink";
        description = "you pour this over your weapon?????? and you attack " + value as string + "faster";
    }
    public AttackSpeedItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/energySword") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
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
