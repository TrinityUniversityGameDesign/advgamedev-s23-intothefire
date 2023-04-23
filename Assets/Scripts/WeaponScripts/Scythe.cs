using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : Weapon
{
    // Start is called before the first frame update
    Quaternion targetRot;
    float lerpTime;
    Vector3 lazyLook;
    public Scythe()
    {
        name = "Scythe";
        description = "Nice and edgy";
        specialDuration = 15;
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
        //player.GetComponent<JacksonCharacterMovement>().SetVelocity(player.transform.forward * 30f);

        if(specialTimer == 0)
        {
            RaycastHit hit;
            lazyLook = player.transform.position;
            if (Physics.SphereCast(player.transform.position, 1f, player.transform.forward, out hit, 750f))
            {
                if (hit.transform.gameObject.tag == "Player" || hit.transform.gameObject.tag == "Enemy")
                {
                    lazyLook = hit.transform.position;
                }
            }
            player.GetComponent<JacksonCharacterMovement>().SetVelocity(player.transform.forward * 750f);
            targetRot = hitbox.transform.localRotation * Quaternion.AngleAxis(-45f, Vector3.up); //* currSword.transform.localRotation;
            hitbox.transform.localRotation = hitbox.transform.localRotation * Quaternion.AngleAxis(45f, Vector3.up); //* currSword.transform.localRotation; //* Quaternion.Euler(0f, -45f, 0f);
            lerpTime = lightSpeed + player.GetComponent<JacksonCharacterMovement>().GetAttackSpeed();
            hitbox.GetComponent<DamageScript>().SetParent(player);
            hitbox.GetComponent<DamageScript>().SetDamage(player.GetComponent<JacksonCharacterMovement>().CalculateDamage(specialDamage));
            hitbox.GetComponent<DamageScript>().SetKnockback(specialKnockback + player.GetComponent<JacksonCharacterMovement>().GetKnockback());
            hitbox.GetComponent<DamageScript>().SetDamageOverTime(player.GetComponent<JacksonCharacterMovement>().GetDamageOverTime());
            if (player.GetComponent<JacksonCharacterMovement>().GetLifesteal() > 0)
            {
                hitbox.GetComponent<DamageScript>().DoLifesteal(player);
            }
            specialTimer++;
            return true;
        }
        else if (specialTimer > specialDuration || hitbox.transform.localRotation == targetRot)
        {

            specialTimer = 0;
            return false;
        }
        else
        {
            player.transform.LookAt(lazyLook);
            hitbox.transform.localRotation = Quaternion.Lerp(hitbox.transform.localRotation, targetRot, lerpTime);
            player.GetComponent<JacksonCharacterMovement>().SetVelocity(Vector3.zero);
            specialTimer++;
            return true;
        }
    }
}
