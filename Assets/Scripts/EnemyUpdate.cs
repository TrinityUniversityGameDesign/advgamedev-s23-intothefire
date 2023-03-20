using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpdate : MonoBehaviour
{   
    //Public values to be set in the editor
    public float maxHitPoints;
    public Rigidbody rb;

    //Public values to be ignored in the editor
    public float hitPoints;
    public float frameCount;

    void Start()
    {
        hitPoints = maxHitPoints;
        frameCount = 0;
    }

    void Update()
    {
        if (hitPoints <= 0){
            Kill();
        } else {
            frameCount += 1;
        }
    }

    public bool TakeDamage(float amount){
        hitPoints = hitPoints - amount;
        return true;
    }

    public void Kill(){
        Destroy(gameObject);
    }
}
