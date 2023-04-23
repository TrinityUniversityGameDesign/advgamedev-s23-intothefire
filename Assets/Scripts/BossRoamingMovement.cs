using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossRoamingMovement : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public float moveSpeed;
    //public float waitTime = 10f;
    public float attackRadius;

    private Animator animator;
    private int currentPointIndex;
    private Vector3 targetPosition;
    private bool isWaiting = false;
    bool playerFound = false;
    float attackCooldown = 2f;


    // Define a maximum angle for the field of view
    public float maxAngle = 45f;


    [SerializeField] private float idleTime = 10f;

    void Start()
    {
        currentPointIndex = 0;
        animator = GetComponent<Animator>();
        Debug.Log("Patrol Count: " + patrolPoints.Count);
        if (patrolPoints.Count > 0)
        {
            targetPosition = patrolPoints[currentPointIndex].position;
        }
    }

    void Update()
{
    if (patrolPoints != null && patrolPoints.Count > 0)
    {
        Transform currentPoint = patrolPoints[currentPointIndex];
        //Debug.Log("Distance between: " + Vector3.Distance(transform.position, currentPoint.position));
        
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, currentPoint.position - transform.position, Vector3.Distance(transform.position, currentPoint.position));
        Transform closestPatrolPoint = null;
        float closestDistance = Mathf.Infinity;


        // Get the forward direction of the patrol object
        Vector3 patrolDirection = transform.forward;

        foreach (RaycastHit hit in hits)
        {
            Debug.Log("Hits so far: " + hit.collider);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Vector3 hitDirection = (hit.collider.transform.position - transform.position).normalized;

                // Check the angle between the hit direction and the patrol object's forward direction
                float angle = Vector3.Angle(patrolDirection, hitDirection);

                // Check if the angle is within the field of view
                if (angle <= maxAngle)
                {
                    if (!IsHitPointBehindWall(hit.collider.transform.position) && Vector3.Distance(transform.position, hit.collider.transform.position) < closestDistance)
                    {
                        closestPatrolPoint = hit.collider.transform;
                        Debug.Log("Closest Player: " + closestPatrolPoint);
                        closestDistance = Vector3.Distance(transform.position, hit.collider.transform.position);
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * (2 * moveSpeed));
                    }
                }
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.Log("Hit a Wall");
                // Handle wall hit here
            }
        }

        
        if (Vector3.Distance(transform.position, currentPoint.position) < 6f)
        {
            
            if (!isWaiting)
            {
                animator.SetBool("IsIdle", true);
                animator.SetBool("IsWalking", false);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 0);
                StartCoroutine(WaitForSecondsCoroutine(idleTime));
                isWaiting = true;
            }
            else
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsWalking", true);
                Debug.Log("Patrol Point Location: " + patrolPoints[currentPointIndex]);
                targetPosition = patrolPoints[currentPointIndex].position;
                RotateTowards(targetPosition);
                isWaiting = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            animator.SetBool("IsWalking", true);

            // Check if there are any players within the attack radius
        Collider[] colliderHits = Physics.OverlapSphere(transform.position, attackRadius);
        float attackDistance = Mathf.Infinity;
        Transform closestPlayerPoint = null;
        foreach (Collider chits in colliderHits)
        {
            if (chits.gameObject.CompareTag("Player"))
            {
                float distanceToPlayer = Vector3.Distance(transform.position, chits.transform.position);
                if (distanceToPlayer < attackDistance)
                {
                    closestPlayerPoint = chits.transform;
                    attackDistance = distanceToPlayer;
                }
                    playerFound = true;
                }
                else
                {
                    animator.SetBool("IsWalking", true);
                }
                
                if (playerFound && attackCooldown <= 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, closestPlayerPoint.position, Time.deltaTime * (2 * moveSpeed));
                    StartCoroutine(AttackPlayer(chits));
                    attackCooldown = 5f; //maxAttackCooldown;
                    break;
                }
            }

                    if (!playerFound)
                    {
                        animator.SetBool("IsWalking", true);
                    }
                }
            }
}

IEnumerator WaitForSecondsCoroutine(float waitTime)
{
    yield return new WaitForSeconds(waitTime);
    isWaiting = false;
}

IEnumerator AttackPlayer(Collider playerCollider)
{
    // Attack the player
    Debug.Log("Attacking Player: " + playerCollider.gameObject.name);
    animator.SetBool("IsAttacking", true);

    // Disable the player's collider to prevent repeated attacks during cooldown
    playerCollider.enabled = false;

    // Wait for some time to simulate an attack cooldown
    yield return new WaitForSeconds(attackCooldown);

    // Re-enable the player's collider and resume other functions
    playerCollider.enabled = true;
    animator.SetBool("IsAttacking", false);
    animator.SetBool("IsWalking", true);
}

private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //Debug.Log("Rotation Angle: " + rotationAngle);
        transform.rotation = Quaternion.Euler(new Vector3(0, rotationAngle, 0));
    }


private bool IsHitPointBehindWall(Vector3 hitPoint)
    {
        Vector3 direction = hitPoint - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                return true;
            }
        }
        return false;
    }


}











/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

public class BossRoamingMovement : MonoBehaviour
{
    //public GameObject roamingBossPrefab;
    public List<Transform> patrolPoints;
    private Animator animator;
    public float moveSpeed;
    public float waitTime;
    public GameObject detectionRadius;
    private float rotationSpeed = 5f;
    private Vector3 currentDirection;
    public LayerMask layerMask;

    private int currentPointIndex;
    private bool isWaiting;

    void Start()
    {
        currentPointIndex = 0;
        isWaiting = false;
        animator = GetComponent<Animator>();
    }

    // This function is called once per frame and is responsible for moving
    // the boss towards the current patrol point. 
    // If the boss reaches the current patrol point, it will move to the next 
    // point and start waiting at it.
void Update()
    {
        if (patrolPoints.Count > 0)
        {
            Transform currentPoint = patrolPoints[currentPointIndex];
            Debug.Log("current point" + currentPoint);

        RaycastHit sphit;
        if (Physics.SphereCast(transform.position, 10f, transform.forward, out sphit, 1000f, (1 << 31))) {
            Debug.Log("Close to " + sphit.transform.gameObject.name);
            if (sphit.transform == currentPoint) {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
                currentPoint = patrolPoints[currentPointIndex];
                Debug.Log("current point: " + currentPoint.gameObject.name);
            }
        }
        RaycastHit sphit;
        if (Physics.SphereCast(transform.position, 10f, transform.forward, out sphit, 1000f, layerMask))
        {
            if (sphit.transform == patrolPoints[currentPointIndex])
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            }
        }

        Vector3 targetPosition = patrolPoints[currentPointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        


            RaycastHit[] hits = Physics.RaycastAll(transform.position, currentPoint.position - transform.position, Vector3.Distance(transform.position, currentPoint.position));
            Transform closestPatrolPoint = null;
            float closestDistance = Mathf.Infinity;

            foreach (RaycastHit hit in hits)
            {
                Debug.Log("Hits so far: " + hit.collider);
                if (hit.collider.CompareTag("Player"))
                {
                    Vector3 hitDirection = (hit.collider.transform.position - transform.position).normalized;
                    if (!IsHitPointBehindWall(hit.collider.transform.position) && Vector3.Distance(transform.position, hit.collider.transform.position) < closestDistance)
                    {
                        closestPatrolPoint = hit.collider.transform;
                        Debug.Log("Closest Point: " + closestPatrolPoint);
                        closestDistance = Vector3.Distance(transform.position, hit.collider.transform.position);
                    }
               
                }
            }

            if (closestPatrolPoint != null)
            {
                Debug.Log("Player detected.");
                currentDirection = (closestPatrolPoint.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, closestPatrolPoint.position, moveSpeed * 2 * Time.deltaTime);
                RotateTowards(closestPatrolPoint.position);
            }

            else
            {
                if (!isWaiting)
                {
                    currentDirection = (currentPoint.position - transform.position).normalized;
                    transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, moveSpeed * Time.deltaTime);
                }

                if (Vector3.Distance(transform.position, currentPoint.position) < 0.01f)
                {
                    currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
                    animator.SetBool("IsWalking", false);
                    StartCoroutine(WaitAtPoint(currentPoint));
                    Debug.Log("Patrol Point Location: " + patrolPoints[currentPointIndex]);
                }
                else
                {
                    RotateTowards(currentPoint.position);
                }
            }
        }
    }

IEnumerator WaitAtPoint(Transform currentPoint)
{
    isWaiting = true;
    animator.SetBool("IsWalking", false);
    animator.SetBool("IsIdle", true);
    yield return new WaitForSeconds(waitTime);
    isWaiting = false;
    animator.SetBool("IsWalking", true);
    Debug.Log("Moving to next point.");
} 



    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //Debug.Log("Rotation Angle: " + rotationAngle);
        transform.rotation = Quaternion.Euler(new Vector3(0, rotationAngle, 0));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Transform point in patrolPoints)
        {
            Gizmos.DrawSphere(point.position, 0.25f);
        }
    }

    private bool IsHitPointBehindWall(Vector3 hitPoint)
    {
        Vector3 direction = hitPoint - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                return true;
            }
        }
        return false;
    }
}


    public Camera cam;
    public Transform[] points;
    public float speed;
    


    public NavMeshAgent agent;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                 agent.SetDestination(hit.point);
            }
        }
    }*/
    
/*
            if (detectionRadius != null)
            {
                animator.SetBool("IsWalking", true);
                Collider[] hitColliders = Physics.OverlapSphere(detectionRadius.transform.position, detectionRadius.transform.localScale.x / 2);
                //Debug.Log("Barrier: " + detectionRadius.transform.position);
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Player"))
                    {
                        Debug.Log("In contact with the player.");
                        transform.position = Vector3.MoveTowards(transform.position, hitCollider.transform.position, moveSpeed * 2 * Time.deltaTime);
                        RotateTowards(hitCollider.transform.position);
                        return;
                    }
                }
            }
            
                    foreach (RaycastHit hit in hits)
        {
            Debug.Log("Hits so far: " + hit.collider);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Vector3 hitDirection = (hit.collider.transform.position - transform.position).normalized;
                if (!IsHitPointBehindWall(hit.collider.transform.position) && Vector3.Distance(transform.position, hit.collider.transform.position) < closestDistance)
                {
                    closestPatrolPoint = hit.collider.transform;
                    Debug.Log("Closest Player: " + closestPatrolPoint);
                    closestDistance = Vector3.Distance(transform.position, hit.collider.transform.position);
                }
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.Log("Hit a Wall");
                // Handle wall hit here
            }
        }
            
            */

