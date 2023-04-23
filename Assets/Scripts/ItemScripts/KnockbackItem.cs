using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackItem : Item
{
    // Start is called before the first frame update
    
    
    public KnockbackItem()
    {
        value = 0.5f;
        
        Texture2D texture = Resources.Load("Textures/ItemIcons/bassball") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Bassball bat";
        description = "you put a bass guitar duct taped to a baseball bat on your weapon, letting you slap homers and giving your attacks " + value as string + "more knockback";
    }
    public KnockbackItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/bassball") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "Bassball bat";
        description = "you put a bass guitar duct taped to a baseball bat on your weapon, letting you slap homers and giving your attacks " + value as string + "more knockback";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        player.GetComponent<JacksonCharacterMovement>().ChangeKnockback(value);
        return true;
    }
}