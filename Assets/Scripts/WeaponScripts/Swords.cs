using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swords : Weapon
{
    // Start is called before the first frame update
    bool upToggle = true;
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
        lightSpeed = 1.1f;
        lightKnockback = 30;
        heavyDamage = 15;
        heavySpeed = 1.2f;
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

            if (lazy.GetJumpHold())
            {
                lazy.SetVelocity(lazy.GetVelocity() + player.transform.forward * 10f + new Vector3(0f, 65f, 0f));
            }
            else
            {
                lazy.SetVelocity(player.transform.forward * 30f);
            }
            
            specialTimer++;
            return true;
        }
        else if (specialTimer > specialDuration)
        {

            specialTimer = 0;
            upToggle = true;
            return false;
        }
        else
        {
            JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
            if (specialTimer == 8)
            {
                
                lazy.SetVelocity(player.transform.forward * 30f + new Vector3(0f, 1f, 0f));
            }
            else if(specialTimer < 8 && lazy.GetJumpHold() && upToggle)
            {
                specialTimer = 1;
                upToggle = false;
                lazy.SetVelocity(lazy.GetVelocity() + player.transform.forward * 10f + new Vector3(0f, 65f, 0f));
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
