using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : Weapon
{
    // Start is called before the first frame update
    Vector3 startPos;
    Vector3 anchor;
    Vector3 orgin;
    public Whip()
    {

        name = "Whip";
        description = "Whip it good";
        specialDuration = 35;
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
            startPos = player.transform.position;
            anchor = player.transform.position + (player.transform.forward * 40f);
            orgin = player.transform.position + ((player.transform.forward * 40f) / 2f) + new Vector3(0, 1, 0);
            JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
            lazy.SetVelocity(Vector3.zero);

            //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z);
            hitbox.transform.localRotation = hitbox.transform.localRotation * Quaternion.AngleAxis(-90f, Vector3.right);
            hitbox.transform.position = new Vector3(hitbox.transform.position.x, hitbox.transform.position.y - 2f, hitbox.transform.position.z);
            hitbox.transform.localScale = new Vector3(3f, 3f, 3f);
            //hitbox.transform.position = new Vector3(hitbox.transform.position.x, hitbox.transform.position.y - 2f, hitbox.transform.position.z);
            //boomPos = player.transform.position;
            anchor = anchor - orgin;
            startPos = startPos - orgin;
        }
        
        if (specialTimer > specialDuration)
        {
            
            specialTimer = 0;
            return false;
        }
        else
        {
            JacksonCharacterMovement lazy = player.GetComponent<JacksonCharacterMovement>();
            /*
            Vector3 trans = Vector3.Slerp(player.transform.position, anchor, 0.07f);
            //trans = (orgin- trans).normalized * 15;
            lazy.SetVelocity((trans - player.transform.position) / Time.deltaTime + new Vector3 (0f, -1f, 0f));
            //lazy.SetVelocity(trans);
            //hitbox.transform.position = boomPos;
            */
            //player.transform.RotateAround(anchor, player.transform.right, -70 * Time.deltaTime);

            //player.transform.position = Vector3.Slerp(player.transform.position - orgin, anchor, 0.1f);
            Vector3 trans = Vector3.Slerp(player.transform.position - orgin, anchor, 0.07f);

            //player.transform.position += orgin;
            trans += orgin;
            lazy.SetVelocity((trans - player.transform.position) / Time.deltaTime);
            specialTimer++;
            if(!lazy.GetSpecialHold())
            {
                specialTimer = specialDuration + 1;
            }
            return true;
        }
    }
}
