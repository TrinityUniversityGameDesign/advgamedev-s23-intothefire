using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    // Start is called before the first frame update
    float damage = 0f;
    float dot = 0f;
    float knockback = 5f;
    bool lifesteal = false;
    GameObject parent;
    GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDamage(float f)
    {
        damage = f;
    }
    public float GetDamage()
    {
        return damage;
    }

    public void SetKnockback(float f)
    {
        knockback = f;
    }
    public float GetKnockback()
    {
        return knockback;
    }

    public void SetDamageOverTime(float f)
    {
        dot = f;
    }

    public float GetDamageOverTime()
    {
        return dot;
    }

    public void DoLifesteal(GameObject play)
    {
        player = play;
        lifesteal = true;
    }

    public bool GetLifesteal()
    {
        return lifesteal;
    }
    public void SetParent(GameObject g)
    {
        parent = g;
    }
    public GameObject GetParent()
    {
        return parent;
    }
}
