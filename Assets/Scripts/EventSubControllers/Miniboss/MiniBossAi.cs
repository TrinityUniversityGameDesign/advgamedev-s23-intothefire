using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniBossAI : MonoBehaviour
{
    float range = 50f; // Distance to target for attacking
    public float moveSpeed = 10f; // Movement speed
    float turnSpeed = 3600f; // Rotation speed

    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Animator animator; // Reference to the Animator component

    GameObject stomp;
    GameObject charge;

    private bool isAttacking; // Flag for whether the character is currently attacking

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = turnSpeed;

        animator = GetComponentInChildren<Animator>();
        isAttacking = false;

        stomp = transform.GetChild(2).gameObject;
        charge = transform.GetChild(3).gameObject;

        SetRandomDestination();
    }

    private void Update()
    {
        // If we're not attacking, move towards the target
        if (!isAttacking)
        {
            // Check if we're close enough to attack
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                isAttacking = true;

                if (Random.Range(0f, 1f) <= .7f)
                {
                    animator.SetTrigger("Stomp");
                    stomp.SetActive(true);
                }
                else
                {
                    animator.SetTrigger("Charge");
                    agent.speed = moveSpeed * 10;
                    charge.SetActive(true);
                } 
            }
        }
    }

    // Called by the Animator when the attack animation finishes
    public void FinishAttack()
    {
        isAttacking = false;
        agent.speed = moveSpeed;

        stomp.SetActive(false);
        charge.SetActive(false);

        SetRandomDestination();
    }

    void SetRandomDestination()
    {
        // choose a random point within the range
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;

        // find the nearest point on the NavMesh to the random point
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            // set the NavMeshAgent's destination to the nearest point on the NavMesh
            agent.SetDestination(hit.position);
        }
    }
}
