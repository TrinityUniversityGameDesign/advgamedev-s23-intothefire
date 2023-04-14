using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

public class BossRoamingMovement_MajorController : MonoBehaviour
{
    public GameObject roamingBossPrefab;
    public List<Transform> patrolPoints;
    public float moveSpeed;
    public float waitTime;
    public GameObject detectionRadius;

    private int currentPointIndex;
    private bool isWaiting;

    void Start()
    {
        currentPointIndex = 0;
        isWaiting = false;

        // Instantiate the roamingBoss game object and use its transform as the starting position
        GameObject roamingBoss = Instantiate(roamingBossPrefab);
        transform.position = roamingBoss.transform.position;
    }

    // This function is called once per frame and is responsible for moving
    // the boss towards the current patrol point. 
    // If the boss reaches the current patrol point, it will move to the next 
    // point and start waiting at it.
    void Update()
    {
        if (!isWaiting && patrolPoints.Count > 0)
        {
            Transform currentPoint = patrolPoints[currentPointIndex];
            if (detectionRadius != null)
            {
                Collider[] hitColliders = Physics.OverlapSphere(detectionRadius.transform.position, detectionRadius.transform.localScale.x / 2);
                Debug.Log("Barrier: " + detectionRadius.transform.position);
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Player"))
                    {
                        Debug.Log("In contact with the player.");
                        transform.position = Vector3.MoveTowards(transform.position, hitCollider.transform.position, moveSpeed * 2 * Time.deltaTime);
                        return;
                    }
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, moveSpeed * Time.deltaTime);

            if (transform.position == currentPoint.position)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
                StartCoroutine(WaitAtPoint());
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Transform point in patrolPoints)
        {
            Gizmos.DrawSphere(point.position, 0.25f);
        }
    }

    
}