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
        specialDuration = 20;
        specialTimer = 0;
        specialKnockback = 60;
        lightDamage = 10;
        lightSpeed = 0.2f;
        lightKnockback = 30;
        heavyDamage = 20;
        heavySpeed = 0.1f;
        heavyKnockback = 60f;
        canMove = false;
    }
    
    public override bool SpecialAttack(float h, float v)
    {
        //Debug.Log("special timer: " +specialTimer + " special duration: " + specialDuration);
        //player.GetComponent<Rigidbody>().velocity = player.transform.forward * 30f;
        player.GetComponent<JacksonCharacterMovement>().SetVelocity(player.transform.forward * 30f);

        if (specialTimer > specialDuration)
        {

            specialTimer = 0;
            return false;
        }
        else
        {
            hitbox.transform.localRotation = hitbox.transform.localRotation * Quaternion.AngleAxis(60f, Vector3.up);
            specialTimer++;
            return true;
        }
    }
}
