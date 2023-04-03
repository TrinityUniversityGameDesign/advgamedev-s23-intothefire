using UnityEngine;
using UnityEngine.AI;

public class MoveInStraightLine : MonoBehaviour
{
    public float speed = 5f;

    private NavMeshAgent agent;

    void Start()
    {
        // Get a reference to the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();

        // Set the speed of the NavMeshAgent
        agent.speed = speed;

        // Start moving the agent in a straight line
        agent.SetDestination(transform.position + transform.forward);
    }

    void Update()
    {
        // Keep moving the agent in a straight line
        agent.SetDestination(transform.position + transform.forward);
    }
}
