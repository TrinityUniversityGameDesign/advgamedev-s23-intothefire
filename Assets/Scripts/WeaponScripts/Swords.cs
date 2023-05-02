using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swords : Weapon
{
    // Start is called before the first frame update
    public Swords()
    {
        weapon = Resources.Load("Prefabs/Weapons/Sword") as GameObject;
        specialWeapon = Resources.Load("Prefabs/Weapons/Sword") as GameObject;
        name = "Sword";
        description = "Basic sword, pretty fast, with a dash slash to move around";
        specialDuration = 25;
        specialTimer = 0;
        specialKnockback = 60;
        lightDamage = 10;
        lightSpeed = 0.8f;
        lightKnockback = 30;
        heavyDamage = 15;
        heavySpeed = 0.8f;
        heavyKnockback = 60f;
        canMove = false;
    }
    
    public override bool SpecialAttack(float h, float v)
    {
        //Debug.Log("special timer: " +specialTimer + " special duration: " + specialDuration);
        //player.GetComponent<Rigidbody>().velocity = player.transform.forward * 30f;
        //player.GetComponent<JacksonCharacterMovement>().SetVelocity(player.transform.forward * 30f);
        if(specialTimer == 0)
        {
            hitbox.transform.localRotation = hitbox.transform.localRotation * Quaternion.AngleAxis(90f, Vector3.right);
            JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
            Vector3 tmp = lazy.GetVelocity();
            if (tmp.y < 0)
            {
                lazy.SetVelocity(new Vector3(tmp.x, 0f, tmp.z));
            }


            lazy.SetVelocity(lazy.GetVelocity() + player.transform.forward * 10f + new Vector3(0f, 65f, 0f));
            
            specialTimer++;
            return true;
        }
        else if (specialTimer > specialDuration)
        {

            specialTimer = 0;
            return false;
        }
        else
        {
            if (specialTimer == 8)
            {
                JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
                lazy.SetVelocity(player.transform.forward * 30f + new Vector3(0f, 1f, 0f));
            }
            hitbox.transform.localRotation = hitbox.transform.localRotation * Quaternion.AngleAxis(60f, Vector3.forward);
            specialTimer++;
            return true;
        }
    }
    public override void LoadWeapon()
    {
        weapon = Resources.Load("Prefabs/Weapons/Sword") as GameObject;
    }
}
