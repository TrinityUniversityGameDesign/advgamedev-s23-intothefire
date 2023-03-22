using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemyAI : EnemyUpdate
{   
    public float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = GetTargetPosition();
        if (targetPos != null) {
            Vector3 vel = (targetPos - transform.position).normalized * moveSpeed;
            rb.AddForce(vel, ForceMode.Force);
        }
    }
}
