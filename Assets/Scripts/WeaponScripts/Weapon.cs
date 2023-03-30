using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    // Start is called before the first frame update
    public string name = "";
    public string description = "";
    public float lightSpeed;
    public float lightDamage;
    public float heavySpeed;
    public float heavyDamage;
    public float specialDamage;
    public float specialDuration;
    public float specialTimer = 0;
    public GameObject player;
    public GameObject hitbox;

    public Weapon() { }

    public void LightAttack() { }
    public void HeavyAttack()
    {

    }
    public virtual bool SpecialAttack() { return true; }

        public void AssignPlayer(GameObject g)
    {
        player = g;
    }

    public void AssignHitbox(GameObject g)
    {
        hitbox = g;
    }
}
