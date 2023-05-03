using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserEnemyAI : EnemyUpdate
{   
    NavMeshAgent navMesh;
    Animator animator;
    float attackStartRange = 3;
    float attackDistance = 2;
    int attackTimer = 40;
    int attackFrame = 10;
    int attackCooldown = 60;
    float damage = 20;
    float knockback = 1f;
    Vector3 attackAngle;

    protected override void EnemyInit() { 
        state = "Chase";  
        stateTimer = attackCooldown;
        navMesh = GetComponent<NavMeshAgent>();
        animator = transform.Find("SlimeModel").gameObject.GetComponent<Animator>();
    }

    void FixedUpdate() {
        Vector3 targetPos = GetTargetPosition();
        if (state == "Chase") {
            navMesh.destination = targetPos;
            --stateTimer;
            if (stateTimer <= 0 && Vector3.Distance(targetPos, transform.position) <= attackStartRange) {
                state = "Attack";
                stateTimer = attackTimer;
                navMesh.isStopped = true;
                Vector3 span = targetPos - transform.position;
                attackAngle = new Vector3(span.x, 0, span.z).normalized * attackDistance;
            }
        } else if (state == "Attack") {
            --stateTimer;
            ModifyVelocity(0.5f);
            if (stateTimer <= 0) {
                state = "Chase";
                stateTimer = attackCooldown;
                navMesh.isStopped = false;
            } else if (stateTimer == attackFrame) {
                MakeHitbox(attackAngle, damage, knockback);
            }
        }
        animator.SetBool("isAttacking", state == "Attack");
    }
}
