using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swords : Weapon
{
    // Start is called before the first frame update
    public Swords()
    {
        name = "Sword";
        description = "Basic sword, pretty fast, with a dash slash to move around";
        specialDuration = 15;
        specialTimer = 0;
        lightDamage = 10;
        lightSpeed = 0.2f;
        heavyDamage = 20;
        heavySpeed = 0.1f;
        canMove = false;
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
