using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int duration;
    public bool isProjectile;
    public GameObject terrainCollider;
    Vector3 velocity = Vector3.zero;

    void Start() {
        terrainCollider = transform.Find("TerrainCollider").gameObject;
    }

    void FixedUpdate() {  
        --duration;
        if (duration <= 0) {Destroy(gameObject);}
        transform.position += velocity;
        if (isProjectile) {
            terrainCollider.transform.localScale = transform.localScale;
            terrainCollider.GetComponent<Collider>().isTrigger = false;
        }
    }

    public void SetDuration(int i) {duration = i;}
    public void SetScale(float f) {transform.localScale = new Vector3(f, f, f);}
    public void SetDamage(float f) {GetComponent<DamageScript>().SetDamage(f);}
    public void SetKnockback(float f) {GetComponent<DamageScript>().SetKnockback(f);}
    public void SetVelocity(Vector3 v) {velocity = v;}
}
