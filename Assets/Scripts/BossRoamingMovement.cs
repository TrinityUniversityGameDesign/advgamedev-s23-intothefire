using System.Collections;
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

            RaycastHit[] hits = Physics.RaycastAll(transform.position, currentPoint.position - transform.position, Vector3.Distance(transform.position, currentPoint.position));
            Transform closestPatrolPoint = null;
            float closestDistance = Mathf.Infinity;

            foreach (RaycastHit hit in hits)
            {
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


   /* public Camera cam;
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
            }*/

