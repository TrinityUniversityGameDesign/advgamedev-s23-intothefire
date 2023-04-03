using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : Weapon
{
    // Start is called before the first frame update
    Vector3 boomPos;
    Vector3 anchor;
    public Whip()
    {

        name = "Hammer";
        description = "Heavy slow big hammer, with a very powerful charge";
        specialDuration = 10000;
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
            anchor = player.transform.position + (player.transform.forward * 20f) + player.transform.up * 20f;
            JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
            Vector3 tmp = lazy.GetVelocity();
            if(tmp.y < 0)
            {
                lazy.SetVelocity(new Vector3(tmp.x, 0f, tmp.z));
            }
            if(lazy.Magnitude() < 10f)
            {
                lazy.SetVelocity(lazy.GetVelocity() + player.transform.forward * 30f + new Vector3(0f, 1f, 0f));
            }
            else
            {
                lazy.SetVelocity(lazy.GetVelocity() + player.transform.forward * 15f + new Vector3(0f, 1f, 0f));
            }
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

            Vector3 targetDirection = new Vector3(h, 0f, v);
            targetDirection = lazy.GetCamera().transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            Quaternion newRotation = Quaternion.Lerp(player.transform.rotation, targetRotation, 0.03f);

            player.transform.rotation = newRotation;
            lazy.SetVelocity((lazy.Magnitude() * player.transform.forward) + new Vector3 (0f, lazy.GetVelocity().y, 0f) + new Vector3(0f, -0.33f, 0f));
            
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
