using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : Weapon
{
    // Start is called before the first frame update
    Vector3 boomPos;
    float zoomVal;
    public Jetpack()
    {
        name = "Hammer";
        description = "Heavy slow big hammer, with a very powerful charge";
        specialDuration = 60;
        specialTimer = 0;
        lightDamage = 15;
        lightSpeed = 0.35f;
        heavyDamage = 30;
        heavySpeed = 0.2f;
        canMove = true;
    }
    
    public override bool SpecialAttack(float h, float v)
    {
        //Debug.Log("special timer: " +specialTimer + " special duration: " + specialDuration);
        if(specialTimer == 0)
        {
            zoomVal = 35;
            JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
            hitbox.transform.localRotation = hitbox.transform.localRotation * Quaternion.AngleAxis(90f, Vector3.right);
            Vector3 tmp = lazy.GetVelocity();
            if(tmp.y < 0)
            {
                lazy.SetVelocity(new Vector3(tmp.x, 0f, tmp.z));
            }
            if(lazy.Magnitude() < 35)
            {
                zoomVal = 35;
                lazy.SetVelocity(player.transform.forward * 35f);
            }
            else
            {
                zoomVal = lazy.Magnitude();
            }

            lazy.SetVelocity(player.transform.forward * zoomVal);

            //boomPos = player.transform.position;
        }
        
        if (specialTimer > specialDuration)
        {

            specialTimer = 0;
            return false;
        }
        else
        {
            JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
            if (h != 0 || v != 0)
            {
                
                Vector3 targetDirection = new Vector3(h, 0f, v);
                targetDirection = lazy.GetCamera().transform.TransformDirection(targetDirection);
                targetDirection.y = 0.0f;

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                Quaternion newRotation = Quaternion.Lerp(player.transform.rotation, targetRotation, 1);

                player.transform.rotation = newRotation;
            }
            lazy.SetVelocity(player.transform.forward * zoomVal);
            
            //hitbox.transform.position = boomPos;
            specialTimer++;
            if(!lazy.GetSpecialHold())
            {
                specialTimer = specialDuration + 1;
            }
            return true;
        }
    }
}
