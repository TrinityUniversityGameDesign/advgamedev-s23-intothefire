using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    // Start is called before the first frame update
    public string name = "";
    public string description = "";
    public float lightStartup;
    public float lightActive;
    public float lightEndlag;
    public float lightDamage;
    public float heavyStartup;
    public float heavyActive;
    public float heavyEndlag;
    public float heavyDamage;

    public float specialDamage;
    public float specialDuration;
    public float specialTimer = 0;
    public GameObject player;

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
}
