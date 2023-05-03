using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MajorEvenMinotaur_Movement : MonoBehaviour
{
    // Reference to the NavMeshAgent component
    NavMeshAgent agent;

    void Awake()
    {
        // Get a reference to the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
        // Generate a random position on the NavMesh surface
        NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas);
        Debug.Log("SamplePositions: " + hit.position);
        // Set the destination of the NavMeshAgent to the random position
        agent.SetDestination(hit.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the agent has reached the destination
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            // Stop the agent from moving
            agent.isStopped = true;
        }
    }
}

