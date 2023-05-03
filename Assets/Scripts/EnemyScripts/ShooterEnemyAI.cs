using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShooterEnemyAI : EnemyUpdate
{   
    NavMeshAgent navMesh;
    float attackStartRange = 3;
    float attackDistance = 2;
    int attackTimer = 40;
    int attackFrame = 10;
    int attackCooldown = 60;
    float damage = 20;
    float knockback = 1f;
    float projSpeed = 0.5f;

    protected override void EnemyInit() { 
        state = "Idle";  
        stateTimer = attackCooldown;
        navMesh = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate() {
        Vector3 targetPos = GetTargetPosition();
        Vector3 trajectory = targetPos - transform.position;
        if (state == "Idle") {
            if (!Physics.Raycast(transform.position, trajectory, trajectory.magnitude, LayerMask.GetMask("Ground"))) {
                --stateTimer;
                navMesh.isStopped = true;
                ModifyVelocity(0.5f);
                if (stateTimer <= 0) {
                    state = "Attack";
                    stateTimer = attackTimer;
                }
            } else {
                navMesh.isStopped = false;
                navMesh.destination = targetPos;
            }
        } else if (state == "Attack") {
            --stateTimer;
            navMesh.isStopped = true;
            ModifyVelocity(0.5f);
            if (stateTimer <= 0) {
                state = "Idle";
                stateTimer = attackCooldown;
            } else if (stateTimer == attackFrame) {
                MakeProjectile(trajectory.normalized * projSpeed, damage, knockback);
            }
        }
    }
}
