using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Possible Attacks: Dash, Ground Pound, 

public class MiniBossAi : MonoBehaviour
{
    public float speed;
    public float range; // the range within which to choose a random point
    public float angularSpeed;

    private NavMeshAgent navAgent; // reference to the NavMeshAgent component
    Animator _anim;
    GameObject _damager;
    GameObject _navSphere;

    List<float> damageTracker;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>(); // get reference to the NavMeshAgent component
        navAgent.speed = speed;
        navAgent.angularSpeed = angularSpeed;
        _navSphere = transform.GetChild(0).gameObject;
        _navSphere.transform.parent = null;
        SetRandomDestination(); // set initial random destination

        _anim = GetComponentInChildren<Animator>();
        _damager = transform.GetChild(1).gameObject;
        damageTracker = new List<float>(GameManager.Instance.players.Count) { 0 };

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
            navAgent.SetDestination(hit.position);
            _navSphere.transform.position = hit.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Damage")
        {
            int damageDealer = GameManager.Instance.players.IndexOf(other.gameObject.transform.parent.gameObject);
            DamageScript otherScript = other.gameObject.GetComponent<DamageScript>();
            
            if(damageDealer != -1) {
                //Get the damage total
                damageTracker[damageDealer] += otherScript.GetDamage();

                //Compute the DOT
                float dotAmt = otherScript.GetDamageOverTime();
                if(dotAmt > 0) StartCoroutine(TickDOT(damageDealer, dotAmt));

            }
        }
    }

    IEnumerator TickDOT(int player, float dotAmt)
    {
        for(int i = 1; i <= 10; i++)
        {
            yield return new WaitForSeconds(1);
            damageTracker[player] += dotAmt;
        }
    }

    public int GetWinner()
    {
        float maxVal = 0;
        int player = 0;
        for(int i = 0; i < damageTracker.Count; i++)
        {
            if(damageTracker[i] > maxVal)
            {
                maxVal = damageTracker[i];
                player = i;
            }
        }

        return player;
    }
}
