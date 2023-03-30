using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserEnemyAI : EnemyUpdate
{   
    NavMeshAgent navMesh;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.updateUpAxis = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        navMesh.destination = GetTargetPosition();
    }
}
