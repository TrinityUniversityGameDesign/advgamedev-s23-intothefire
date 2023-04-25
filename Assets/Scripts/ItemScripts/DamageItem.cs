using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageItem : Item
{
    // Start is called before the first frame update
    public DamageItem()
    {
        value = 5;
        Texture2D texture = Resources.Load("Textures/ItemIcons/ball") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "spikeyBall";
        description = "putting this spikey ball on your weapon should make it hurt some more. It does +" + value as string + "more damage now";
    }
    public DamageItem(float val)
    {
        value = val;
        Texture2D texture = Resources.Load("Textures/ItemIcons/ball") as Texture2D;
        icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        name = "spikeyBall";
        description = "putting this spikey ball on your weapon should make it hurt some more. It does +" + value as string + "more damage now";
    }
    

    // Update is called once per frame
   public override bool ItemPickup()
    {
        Debug.Log("Attempt to change value of damage for player; " + player.name);
        JacksonCharacterMovement jcm = player.GetComponent<JacksonCharacterMovement>();
        Debug.Log("Got jcm? " + (jcm != null));
        jcm.ChangeDamage(value);
        Debug.Log("Changed damage value for player");
        return true;
    }
}
