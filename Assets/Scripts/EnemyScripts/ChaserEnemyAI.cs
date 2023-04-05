using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserEnemyAI : EnemyUpdate
{   
    NavMeshAgent navMesh;
    float attackStartRange = 3;
    float attackDistance = 2;
    int attackTimer = 40;
    int attackFrame = 10;
    int attackCooldown = 60;
    float damage = 20;
    float knockback = 1f;

    protected override void EnemyInit() { 
        state = "Chase";  
        stateTimer = attackCooldown;
        navMesh = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate() {
        Vector3 targetPos = GetTargetPosition();
        if (state == "Chase") {
            navMesh.destination = targetPos;
            --stateTimer;
            if (stateTimer <= 0 && Vector3.Distance(targetPos, transform.position) <= attackStartRange) {
                state = "Attack";
                stateTimer = attackTimer;
                navMesh.enabled = false;
            }
        } else if (state == "Attack") {
            --stateTimer;
            if (stateTimer <= 0) {
                state = "Chase";
                stateTimer = attackCooldown;
                navMesh.enabled = true;
            } else if (stateTimer == attackFrame) {
                Vector3 span = targetPos - transform.position;
                Vector3 hitboxOffset = new Vector3(span.x, 0, span.z).normalized * attackDistance;
                MakeHitbox(hitboxOffset, damage, knockback);
            }
        }
    }
}
