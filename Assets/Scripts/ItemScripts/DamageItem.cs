using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageItem : Item
{
    // Start is called before the first frame update
    
    void start()
    {
        value = 10;
    }
    

    // Update is called once per frame
   public new void ItemPickup()
    {
        player.GetComponent<JacksonPlayerMovement>().ChangeDamage(value);
    }
}
