using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MajorEvenMinotaur_Movement : MonoBehaviour
{
    // Reference to the destination point where the object will move
    [SerializeField] Transform goal;

    // Reference to the NavMeshAgent component
    UnityEngine.AI.NavMeshAgent agent;

    void Awake()
    {
        // Get a reference to the NavMeshAgent component
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
        // Find the destination point inside the scene
        goal = GameObject.FindGameObjectWithTag("Destination").transform;
        Debug.Log("Found Destination: " + goal.position);

        // Set the destination of the NavMeshAgent to the goal position
        agent.destination = goal.position;
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

