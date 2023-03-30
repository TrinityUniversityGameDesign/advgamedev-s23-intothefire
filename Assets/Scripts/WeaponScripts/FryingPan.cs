using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : Weapon
{
    // Start is called before the first frame update
    Vector3 boomPos;
    public FryingPan()
    {
        name = "Sword";
        description = "Basic sword, pretty fast, with a dash slash to move around";
        specialDuration = 60;
        specialTimer = 0;
        lightDamage = 10;
        lightSpeed = 0.2f;
        heavyDamage = 20;
        heavySpeed = 0.1f;
    }
    
    public override bool SpecialAttack()
    {
        //Debug.Log("special timer: " +specialTimer + " special duration: " + specialDuration);
        if(specialTimer == 0)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.velocity = player.transform.forward * 30f;
            rb.velocity = rb.velocity + new Vector3(0f, 20f, 0f);
            boomPos = player.transform.position;
        }
        
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
