using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniBossAi : MonoBehaviour
{
    float range = 50f; // Distance to target for attacking
    public float moveSpeed = 10f; // Movement speed
    float turnSpeed = 3600f; // Rotation speed

    public bool isCenterBoss = false; 

    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Animator animator; // Reference to the Animator component

    GameObject stomp;
    GameObject charge;
    GameObject navSphere;

    public List<float> damageTracker;

    private bool isAttacking; // Flag for whether the character is currently attacking

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = turnSpeed;

        animator = GetComponentInChildren<Animator>();
        isAttacking = false;

        damageTracker = new List<float> { 0, 0, 0, 0};
    
        stomp = transform.GetChild(2).gameObject;
        charge = transform.GetChild(3).gameObject;
        navSphere = null;
        if(navSphere != null) navSphere.transform.parent = null;

        SetRandomDestination();
    }

    private void Update()
    {
        // If we're not attacking, move towards the target
        if (!isAttacking)
        {
            // Check if we're close enough to attack
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                isAttacking = true;

                if (Random.Range(0f, 1f) <= .7f)
                {
                    animator.SetTrigger("Stomp");
                    stomp.SetActive(true);
                }
                else
                {
                    animator.SetTrigger("Charge");
                    agent.speed = moveSpeed * 10;
                    charge.SetActive(true);
                } 
            }
        }
    }

    // Called by the Animator when the attack animation finishes
    public void FinishAttack()
    {
        isAttacking = false;

        stomp.SetActive(false);
        charge.SetActive(false);

        agent.speed = 10;

        SetRandomDestination();
    }

    void SetRandomDestination()
    {
        // choose a random point within the range
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;

        // find the nearest point on the NavMesh to the random point
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            // set the NavMeshAgent's destination to the nearest point on the NavMesh
            agent.SetDestination(hit.position);
            if (navSphere != null) navSphere.transform.position = hit.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Damage")
        {
            Transform potentialParent = other.gameObject.transform;
            while(potentialParent.parent != null)
		        {
              potentialParent = potentialParent.parent;
		        }

            //Debug.Log(potentialParent.name);
            

            int damageDealer = GameManager.Instance.players.IndexOf(potentialParent.gameObject);
            DamageScript otherScript = other.gameObject.GetComponent<DamageScript>();
            Debug.Log(damageDealer);
            Debug.Log(damageTracker.Count);
            if (damageDealer != -1)
            {
                //Get the damage total
                damageTracker[damageDealer] += otherScript.GetDamage();
                
        
                if(isCenterBoss)
        				{
                  transform.GetComponent<CenterBossScript>().UpdateDamageSum(damageDealer);
				        }

                //Compute the DOT
                float dotAmt = otherScript.GetDamageOverTime();
                if (dotAmt > 0) StartCoroutine(TickDOT(damageDealer, dotAmt));

            }
        }
    }

    IEnumerator TickDOT(int player, float dotAmt)
    {
        for (int i = 1; i <= 10; i++)
        {
            yield return new WaitForSeconds(1);
            damageTracker[player] += dotAmt;
        }
    }

    public int GetWinner()
    {
        float maxVal = 0;
        int player = 0;
        for (int i = 0; i < damageTracker.Count; i++)
        {
            if (damageTracker[i] > maxVal)
            {
                maxVal = damageTracker[i];
                player = i;
            }
        }
        return player;
    }
}
