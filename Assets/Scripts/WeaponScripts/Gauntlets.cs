using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlets : Weapon
{
    // Start is called before the first frame update
    Vector3 boomPos;
    Quaternion boomRot;
    public Gauntlets()
    {
        name = "Gauntlets";
        description = "Punchy, Punchy, Go, Pow - Karate Joe";
        specialDuration = 18;
        specialTimer = 0;
        specialKnockback = 70;
        lightDamage = 10;
        lightSpeed = 0.5f;
        lightKnockback = 10;
        heavyDamage = 20;
        heavySpeed = 0.35f;
        heavyKnockback = 20;
        canMove = true;
    }
    
    public override bool SpecialAttack(float h, float v)
    {
        //Debug.Log("special timer: " +specialTimer + " special duration: " + specialDuration);
        if(specialTimer == 0)
        {
            JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
            Vector3 tmp = lazy.GetVelocity();
            if(tmp.y < 0)
            {
                lazy.SetVelocity(new Vector3(tmp.x, 0f, tmp.z));
            }
            

                lazy.SetVelocity(lazy.GetVelocity() + player.transform.forward * 5f + new Vector3(0f, 65f, 0f));
            
            hitbox.transform.localRotation = hitbox.transform.localRotation * Quaternion.AngleAxis(-90f, Vector3.right);
            boomRot = hitbox.transform.localRotation;
            hitbox.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            hitbox.transform.position = new Vector3(hitbox.transform.position.x, hitbox.transform.position.y - 2f, hitbox.transform.position.z);
            
        }
        
        if (specialTimer > specialDuration)
        {
            
            specialTimer = 0;
            return false;
        }
        else
        {
           if(specialTimer == 8)
            {
                JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
                lazy.SetVelocity(player.transform.forward * 5f + new Vector3(0f, 5f, 0f));
            }
            player.GetComponent<JacksonCharacterMovement>().MovementManagement(h, v);
            player.GetComponent<JacksonCharacterMovement>().SetVelocity(player.GetComponent<JacksonCharacterMovement>().GetVelocity() + new Vector3(0f, -0.5f, 0f));
            
            specialTimer++;
            return true;
        }
    }
}
