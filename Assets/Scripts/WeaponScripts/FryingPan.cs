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
        specialDuration = 30;
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
            JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
            Vector3 tmp = lazy.GetVelocity();
            if(tmp.y < 0)
            {
                lazy.SetVelocity(new Vector3(tmp.x, 0f, tmp.z));
            }
            if(lazy.Magnitude() < 10f)
            {
                lazy.SetVelocity(lazy.GetVelocity() + player.transform.forward * 25f + new Vector3(0f, 20f, 0f));
            }
            else
            {
                lazy.SetVelocity(lazy.GetVelocity() + player.transform.forward * 10f + new Vector3(0f, 20f, 0f));
            }
            boomPos = player.transform.position;
        }
        
        if (specialTimer > specialDuration)
        {

            specialTimer = 0;
            return false;
        }
        else
        {
            player.GetComponent<JacksonCharacterMovement>().MovementManagement(h, v);
            player.GetComponent<JacksonCharacterMovement>().SetVelocity(player.GetComponent<JacksonCharacterMovement>().GetVelocity() + new Vector3(0f, -0.5f, 0f));
            hitbox.transform.position = boomPos;
            specialTimer++;
            return true;
        }
    }
}
