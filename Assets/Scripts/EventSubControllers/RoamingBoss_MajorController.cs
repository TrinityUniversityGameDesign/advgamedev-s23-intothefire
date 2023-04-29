using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingBoss_MajorController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public float speed = 3f;

    public float viewDistance = 10f; // the maximum distance the game object can "see"
    public float fieldOfViewAngle = 90f; // the field of view angle in degrees


    private void Start()
    {
        // Get reference to NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        animator = GetComponent<Animator>();
        // Start moving to a random point on the NavMesh
        MoveToRandomPoint();
    }

    private void MoveToRandomPoint()
    {
        // Get a random point on the NavMesh surface
        Vector3 randomPoint = RandomNavMeshLocation(200f);

        // Check if the random point is reachable
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(randomPoint, path))
        {
            animator.SetBool("IsWalking", true); // set IsMoving to true
            // Set the NavMeshAgent destination to the random point
            agent.SetPath(path);
            StartCoroutine(WaitForDestination()); // wait for the agent to reach the destination
        }
        else
        {
            // If the random point is not reachable, try again
            MoveToRandomPoint();
        }
    }

    private IEnumerator WaitForDestination()
    {
        // Wait for the agent to reach the destination
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }
        animator.SetBool("IsWalking", false); // set IsMoving to false
        MoveToRandomPoint(); // move to another random point
    }

    private Vector3 RandomNavMeshLocation(float radius)
    {
    // Get a random point inside a circle of the given radius
    Vector3 randomDirection = Random.insideUnitSphere * radius;
    randomDirection += transform.position;

    // Project the point onto the NavMesh surface
    NavMeshHit hit;
    NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);

    // Check if the hit point collides with PlaceholderRoom
    Collider[] colliders = Physics.OverlapSphere(hit.position, 10f);
    foreach (Collider collider in colliders)
    {
        if (collider.gameObject.name == "PlaceholderRoom" || collider.gameObject.name == "PlaceholderCenterRoom" 
            || collider.gameObject.name == "Spawnroom" || collider.gameObject.name == "Floor" )
        {
            // If it does, get another random point
            return RandomNavMeshLocation(radius);
        }
    }

        //Debug.Log("Hit Position for Minotaur" + hit.position);
        return hit.position;
    }

    private void Update()
    {
        // check if the game object can "see" any player or wall objects
        Collider[] objectsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance);
        foreach (Collider objCollider in objectsInViewRadius)
        {
            // check if the object is a wall or player
            if (objCollider.gameObject.CompareTag("Wall") || objCollider.gameObject.CompareTag("Player"))
            {
                // check if the object is within the field of view angle
                Vector3 directionToTarget = objCollider.gameObject.transform.position - transform.position;
                if (Vector3.Angle(transform.forward, directionToTarget) < fieldOfViewAngle / 2)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToTarget, out hit, viewDistance))
                    {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                    // if the player is seen, stop moving and attack the player
                    agent.SetDestination(transform.position);
                    Debug.Log("Spotted Player!");
                    animator.SetBool("IsAttacking", true);
                    }
                }
            }
        }
    }
    }
}


