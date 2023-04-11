using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserEnemyAI : EnemyUpdate
{   
    NavMeshAgent navMesh;

    protected override void EnemyInit() {   
        navMesh = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate() {
        navMesh.destination = GetTargetPosition();
    }
}
