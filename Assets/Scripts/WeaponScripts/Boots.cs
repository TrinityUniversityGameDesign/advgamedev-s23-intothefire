using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boots : Weapon
{
    // Start is called before the first frame update
    Vector3 boomPos;
    float GroundPoundVal = 0;
    bool dash = false;
    public Boots()
    {
        name = "Boots";
        description = "Ground Pfound";
        specialDuration = 10000;
        specialTimer = 0;
        lightDamage = 15;
        lightSpeed = 0.35f;
        heavyDamage = 30;
        heavySpeed = 0.25f;
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
            hitbox.transform.localRotation = hitbox.transform.localRotation * Quaternion.AngleAxis(-90f, Vector3.right);
            hitbox.transform.localScale = new Vector3(1.5f, 1.5f, .5f);
            hitbox.transform.position = new Vector3(hitbox.transform.position.x, hitbox.transform.position.y - 2f, hitbox.transform.position.z);

            lazy.SetVelocity( new Vector3 (0f, -0f, 0f));
               
            
            //boomPos = player.transform.position;
        }
        
        if (specialTimer > specialDuration)
        {
            GroundPoundVal = 0f;
            specialTimer = 0;
            dash = false;
            return false;
        }
        else
        {
            if (!dash)
            {
                JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();

                Vector3 targetDirection = new Vector3(h, 0f, v);
                targetDirection = lazy.GetCamera().transform.TransformDirection(targetDirection);
                targetDirection.y = 0.0f;

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                Quaternion newRotation = Quaternion.Lerp(player.transform.rotation, targetRotation, 1);


                lazy.SetVelocity(lazy.GetVelocity() + new Vector3(0f, GroundPoundVal, 0f));
                GroundPoundVal -= 0.25f;
                Debug.Log("gp velocity" + lazy.GetVelocity().y);
                if (lazy.GetVelocity().y < -40)
                {
                    player.transform.rotation = newRotation;
                    lazy.SetVelocity(new Vector3(0f, -40f, 0f));
                }
                //hitbox.transform.position = boomPos;
                specialTimer++;
                if (!lazy.GetSpecialHold() || lazy.GetGrounded())
                {
                    dash = true;
                    lazy.SetVelocity(player.transform.forward * (-1 * lazy.GetVelocity().y * 2) + new Vector3(0f, 10f, 0f));
                    specialTimer = specialDuration - 5;
                }
            }
            else
            {
                specialTimer++;
            }
            return true;
        }
    }
}
