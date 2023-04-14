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

    private int currentPointIndex;
    private bool isWaiting;

    void Start()
    {
        currentPointIndex = 0;
        isWaiting = false;

        // Instantiate the roamingBoss game object and use its transform as the starting position
        //GameObject roamingBoss = Instantiate(roamingBossPrefab);
        //transform.position = roamingBoss.transform.position;
        animator = GetComponent<Animator>();
    }

    // This function is called once per frame and is responsible for moving
    // the boss towards the current patrol point. 
    // If the boss reaches the current patrol point, it will move to the next 
    // point and start waiting at it.
    void Update()
    {
        if (!isWaiting && patrolPoints.Count > 0)
        {
            //animator.SetBool("IsWalking", true);
            Transform currentPoint = patrolPoints[currentPointIndex];

            RaycastHit hit;
            // Player.pos - minotaur's positon
            Vector3 hitDirection = (currentPoint.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, hitDirection, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player detected.");
                    // Check if the hit point is not behind a wall
                    if (!IsHitPointBehindWall(hit.point))
                    {
                        Debug.Log("Wall detected.");
                        transform.position = Vector3.MoveTowards(transform.position, hit.collider.transform.position, moveSpeed * 2 * Time.deltaTime);
                        RotateTowards(hit.collider.transform.position);
                        return;
                    }
                }
            }
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
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentPoint.position) < 0.01f)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
                animator.SetBool("IsWalking", false);
                
                StartCoroutine(WaitAtPoint());

                RotateTowards(patrolPoints[currentPointIndex].position);
            }
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        if (waitTime >= 0f)
        {
            yield return new WaitForSeconds(waitTime);
        }
        isWaiting = false;
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

    private void Dectect()
    {

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
    


