using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingBoss_MajorController : MonoBehaviour
{
    /*
    public float speed;
    public float range; // the range within which to choose a random point
    public float angularSpeed;
    public GameObject roamingBossPrefab;
    public GameObject destination;

    private NavMeshAgent navAgent; // reference to the NavMeshAgent component
    Animator _anim;
    //GameObject _damager;
    GameObject _navSphere;

    //List<float> damageTracker;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>(); // get reference to the NavMeshAgent component
        navAgent.speed = speed;
        navAgent.angularSpeed = angularSpeed;
        _navSphere = transform.GetChild(0).gameObject;
        _navSphere.transform.parent = null;
        SetRandomDestination(); // set initial random destination

        
        _anim = roamingBossPrefab.GetComponent<Animator>();
        //_damager = transform.GetChild(1).gameObject;
        //damageTracker = new List<float>(GameManager.Instance.players.Count) { 0 };

    }

    // Update is called once per frame
    void Update()
    {
        // if the NavMeshAgent has reached its destination, set a new random destination
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            SetRandomDestination();
        }
    }

    // sets a random destination within the range
    void SetRandomDestination()
    {
        // choose a random point within the range
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;

        // find the nearest point on the NavMesh to the random point
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            // set the NavMeshAgent's destination to the nearest point on the NavMesh
            navAgent.destination = destination.transform.position;
            navAgent.SetDestination(hit.position);
            _navSphere.transform.position = hit.position;
            _anim.SetBool("IsWalking", false);
        }
    }

    */
    public List<Transform> patrolPoints;
    public float moveSpeed;
    public float attackRadius;

    public GameObject roamingBossPrefab;
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
        animator = roamingBossPrefab.GetComponent<Animator>();
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
            RaycastHit[] hits = Physics.RaycastAll(roamingBossPrefab.transform.position, currentPoint.position - roamingBossPrefab.transform.position, Vector3.Distance(roamingBossPrefab.transform.position, currentPoint.position));
            Transform closestPatrolPoint = null;
            float closestDistance = Mathf.Infinity;


            // Get the forward direction of the patrol object
            Vector3 patrolDirection = roamingBossPrefab.transform.forward;

            foreach (RaycastHit hit in hits)
            {
                Debug.Log("Hits so far: " + hit.collider);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Vector3 hitDirection = (hit.collider.transform.position - roamingBossPrefab.transform.position).normalized;
                    float angle = Vector3.Angle(patrolDirection, hitDirection);
                    if (angle <= maxAngle)
                    {
                        if (!IsHitPointBehindWall(hit.collider.transform.position) && Vector3.Distance(roamingBossPrefab.transform.position, hit.collider.transform.position) < closestDistance)
                        {
                            closestPatrolPoint = hit.collider.transform;
                            Debug.Log("Closest Player: " + closestPatrolPoint);
                            closestDistance = Vector3.Distance(roamingBossPrefab.transform.position, hit.collider.transform.position);
                            roamingBossPrefab.transform.position = Vector3.MoveTowards(roamingBossPrefab.transform.position, targetPosition, Time.deltaTime * (2 * moveSpeed));
                        }
                    }
                }

                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    Debug.Log("Hit a Wall");
                }

            }

            if (Vector3.Distance(roamingBossPrefab.transform.position, currentPoint.position) < 6f)
            {
                if (!isWaiting)
                {
                    animator.SetBool("IsIdle", true);
                    animator.SetBool("IsWalking", false);
                    roamingBossPrefab.transform.position = Vector3.MoveTowards(roamingBossPrefab.transform.position, targetPosition, Time.deltaTime * 0);
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
                roamingBossPrefab.transform.position = Vector3.MoveTowards(roamingBossPrefab.transform.position, targetPosition, Time.deltaTime * moveSpeed);
                animator.SetBool("IsWalking", true);

            // Check if there are any players within the attack radius
            Collider[] colliderHits = Physics.OverlapSphere(roamingBossPrefab.transform.position, attackRadius);
            float attackDistance = Mathf.Infinity;
            Transform closestPlayerPoint = null;
            foreach (Collider chits in colliderHits)
            {
                if (chits.gameObject.CompareTag("Player"))
                {
                    float distanceToPlayer = Vector3.Distance(roamingBossPrefab.transform.position, chits.transform.position);
                    if (distanceToPlayer < attackDistance)
                    {
                        Debug.Log("Distance to player: " + distanceToPlayer);
                        Debug.Log("Distance Attack: " + attackCooldown);
                        closestPlayerPoint = chits.transform;
                        attackDistance = distanceToPlayer;
                    }
                        playerFound = true;
                    }
                    else
                    {
                        animator.SetBool("IsWalking", true);
                    }
                    
                    if (playerFound && attackCooldown <= 2f)
                    {
                        roamingBossPrefab.transform.position = Vector3.MoveTowards(roamingBossPrefab.transform.position, closestPlayerPoint.position, Time.deltaTime * (2 * moveSpeed));
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
        Vector3 direction = (targetPosition - roamingBossPrefab.transform.position).normalized;
        float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //Debug.Log("Rotation Angle: " + rotationAngle);
        roamingBossPrefab.transform.rotation = Quaternion.Euler(new Vector3(0, rotationAngle, 0));
    }


private bool IsHitPointBehindWall(Vector3 hitPoint)
    {
        Vector3 direction = hitPoint - roamingBossPrefab.transform.position;
        Ray ray = new Ray(roamingBossPrefab.transform.position, direction);
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


