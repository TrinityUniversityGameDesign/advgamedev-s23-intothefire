using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

public class BossRoamingMovement : MonoBehaviour
{
    /*
       public List<Transform> patrolPoints: A list of transform objects that represent the points the boss will move between during its patrol.
       public float moveSpeed: The speed at which the boss will move between patrol points.
       public float waitTime: The amount of time (in seconds) the boss will wait at each patrol point before moving to the next one.
       private int currentPointIndex: The index of the current patrol point the boss is moving towards.
       private bool isWaiting: A boolean flag that is used to indicate whether the boss is currently waiting at a patrol point or not.
    */
    public List<Transform> patrolPoints;
    public float moveSpeed;
    public float waitTime;

    private int currentPointIndex;
    private bool isWaiting;

    void Start()
    {
        currentPointIndex = 0;
        isWaiting = false;
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
    


