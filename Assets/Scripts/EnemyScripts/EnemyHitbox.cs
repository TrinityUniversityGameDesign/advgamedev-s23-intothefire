using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int duration;

    void FixedUpdate()
    {
        --duration;
        if (duration <= 0) {Destroy(gameObject);}
    }

    public void SetDuration(int i) {duration = i;}
    public void SetScale(float f) {transform.localScale = new Vector3(f, f, f);}
    public void SetDamage(float f) {GetComponent<DamageScript>().SetDamage(f);}
    public void SetKnockback(float f) {GetComponent<DamageScript>().SetKnockback(f);}
}
