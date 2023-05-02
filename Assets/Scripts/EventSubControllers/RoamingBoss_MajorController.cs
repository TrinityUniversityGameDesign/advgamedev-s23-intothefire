using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingBoss_MajorController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public float speed = 3f;

    public float viewDistance = 30f; // the maximum distance the game object can "see"
    public float fieldOfViewAngle = 90f; // the field of view angle in degrees
    public float followDistance = 30f; // the distance at which the object will stop following the player
    public string[] floorObjectNames; // the names of the objects on which the object can move

    private bool isAttacking = false;

    private void Start()
    {
        // Get reference to NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.acceleration = 2f;
        agent.angularSpeed = 120f;
        animator = GetComponent<Animator>();
        // Start moving to a random point on the NavMesh
        MoveToRandomPoint();

        gameObject.GetComponent<DamageScript>().SetDamage(30f);

    }


  private void MoveToRandomPoint()
{
    // Check if the player is within the agent's line of sight
    Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player")?.transform?.position ?? Vector3.zero;
    if (playerPosition != Vector3.zero && Physics.Linecast(transform.position, playerPosition, out RaycastHit hit))
    {
        if (hit.collider.CompareTag("Player"))
        {
            // Set the destination to the player's position
            animator.SetBool("IsWalking", true); // set IsMoving to true
            agent.SetDestination(playerPosition);
            StartCoroutine(WaitForChargeAndAttack(playerPosition));
            //StartCoroutine(WaitForDestination()); // wait for the agent to reach the destination
            return;
        }
    }

    NavMeshPath path = new NavMeshPath();
    bool foundPath = false;

    while (!foundPath)
    {
        // Get a random point on the NavMesh surface
        Vector3 randomPoint = RandomNavMeshLocation(200f);

        // Check if the random point is reachable
        if (agent.CalculatePath(randomPoint, path))
        {
            animator.SetBool("IsWalking", true); // set IsMoving to true
            // Set the NavMeshAgent destination to the random point
            agent.SetPath(path);
            StartCoroutine(WaitForDestination()); // wait for the agent to reach the destination
            foundPath = true;
        }
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

        // Check if the hit point collides with a floor object
        Collider[] colliders = Physics.OverlapSphere(hit.position, 10f);
        foreach (Collider collider in colliders)
        {
            if (ArrayContains(floorObjectNames, collider.gameObject.name))
            {
                // If it does, return the hit point
                return RandomNavMeshLocation(radius);
            }
        }

        // If the hit point does not collide with a floor object, get another random point
        return hit.position;
    }

    private bool ArrayContains(string[] array, string value)
    {
        // Check if an array contains a value
        foreach (string item in array)
        {
            if (item == value)
            {
                return true;
            }
        }

        return false;
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
                if (!isAttacking && Vector3.Angle(transform.forward, directionToTarget) < fieldOfViewAngle / 2)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToTarget, out hit, viewDistance))
                    {
                        if (hit.collider.gameObject.CompareTag("Player"))
                        {
                            if (Vector3.Distance(transform.position, objCollider.gameObject.transform.position) <= followDistance)
                            {
                                Debug.Log("Hit collided with: " + objCollider.gameObject.transform.position);
                                speed = speed * 1.2f;
                                Debug.Log("Spotted Player!");
                                animator.SetTrigger("Running");
                                StartCoroutine(WaitForChargeAndAttack(objCollider.gameObject.transform.position));
                            }

                            Debug.Log("Not in reach!");
                        }
                        else
                        {
                            // If the object is not the player, move to a random point
                            MoveToRandomPoint();              
                        }
                    }
                }
            }
        }
    }



private IEnumerator WaitForChargeAndAttack(Vector3 targetPosition)
{
    agent.SetDestination(targetPosition);
    
    Debug.Log("Spotted Player Again!");
    isAttacking = true;
    speed = speed / 2;
    animator.SetTrigger("Attack"); 
    gameObject.GetComponent<DamageScript>().SetDamage(30f);
    yield return new WaitForSeconds(1f);
    StartCoroutine(WaitForAttack());

    //animator.SetBool("IsWalking", true);
    //isAttacking = true;    
    //agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
    //yield return new WaitForSeconds(1.5f);
    //isAttacking = false;

}

private IEnumerator WaitForAttack()
{
    yield return new WaitForSeconds(1f);
    animator.SetBool("IsWalking", true);
    isAttacking = false;

    //agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
    //Debug.Log("Spotted Player Again!");
    //isAttacking = true;
    //yield return new WaitForSeconds(1.5f);
}

}






/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingBoss_MajorController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public float speed = 3f;

    public float viewDistance = 50f; // the maximum distance the game object can "see"
    public float fieldOfViewAngle = 90f; // the field of view angle in degrees
    private bool isAttacking = false;
    


    private void Start()
    {
        // Get reference to NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.acceleration = 2f;
        agent.angularSpeed = 120f;
        animator = GetComponent<Animator>();
        // Start moving to a random point on the NavMesh
        MoveToRandomPoint();

        gameObject.GetComponent<DamageScript>().SetDamage(30f);
    }

    private void MoveToRandomPoint()
    {
        // Get a random point on the NavMesh surface
        Vector3 randomPoint = RandomNavMeshLocation(80f);

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
            if (!isAttacking && Vector3.Angle(transform.forward, directionToTarget) < fieldOfViewAngle / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToTarget, out hit, viewDistance))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        // if the player is seen, stop moving and attack the player
                        isAttacking = true;
                        agent.SetDestination(objCollider.gameObject.transform.position);
                        Debug.Log("Hit collided with: " + objCollider.gameObject.transform.position);
                        speed = speed * 2;
                        Debug.Log("Spotted Player!");
                        animator.SetTrigger("Attack");
                        StartCoroutine(WaitForAttack());
                    }
                    else
                    {
                        // If the object is not the player, move to a random point
                        MoveToRandomPoint();
                        //agent.SetDestination(hit.collider.gameObject.transform.position);
                        
                    }
                }
            }
        }
    }
}


private IEnumerator WaitForAttack()
{
    yield return new WaitForSeconds(2f);
    agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
    Debug.Log("Spotted Player Again!");
    animator.SetBool("IsWalking", true);
    isAttacking = true;

    yield return new WaitForSeconds(1.5f);
    isAttacking = false;

}


}
*/


