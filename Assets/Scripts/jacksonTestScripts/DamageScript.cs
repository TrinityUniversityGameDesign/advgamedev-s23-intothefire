using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    // Start is called before the first frame update
    float damage = 0f;
    float knockback = 5f;
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
        knockback = 0f;
    }
    public float GetKnockback()
    {
        return knockback;
    }
}
