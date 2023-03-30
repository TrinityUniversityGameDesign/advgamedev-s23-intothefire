using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swords : Weapon
{
    // Start is called before the first frame update
    public Swords()
    {
        specialDuration = 15;
        specialTimer = 0;
    }
    
    public override bool SpecialAttack()
    {
        //Debug.Log("special timer: " +specialTimer + " special duration: " + specialDuration);
        player.GetComponent<Rigidbody>().velocity = player.transform.forward * 30f;
        if (specialTimer > specialDuration)
        {

            specialTimer = 0;
            return false;
        }
        else
        {
            specialTimer++;
            return true;
        }
    }
}
