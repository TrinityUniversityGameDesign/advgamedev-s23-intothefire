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
    public int UniqueAttackCooldown = 5;

    private NavMeshAgent navAgent; // reference to the NavMeshAgent component
    Animator _anim;
   
    GameObject base_damage;
    GameObject stomp;
    GameObject jump;
    GameObject charge;

    List<float> damageTracker;

    bool canAttack = true;
    bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>(); // get reference to the NavMeshAgent component
        navAgent.speed = speed;
        navAgent.angularSpeed = angularSpeed;
        SetRandomDestination(); // set initial random destination

        _anim = GetComponentInChildren<Animator>();
        base_damage = transform.GetChild(1).gameObject;
        stomp = transform.GetChild(2).gameObject;
        jump = transform.GetChild(3).gameObject;
        charge = transform.GetChild(4).gameObject;
        damageTracker = new List<float>(GameManager.Instance.players.Count) { 0 };

    }

    // Update is called once per frame
    void Update()
    {
        //If I am at the navigation point, then see if can attack
        //If I can, then lets trigger the attack. 


        // if the NavMeshAgent has reached its destination, set a new random destination
        if (navAgent.remainingDistance <= navAgent.stoppingDistance && !attacking)
        {
            if (Random.Range(0f, 1f) <= .25)
            {
                SelectRandomAttack();
            } else
            {
                SetRandomDestination();
            }
        }
    }

    IEnumerator AttackCooldown(int delay)
    {
        attacking = true;
        yield return new WaitForSeconds(delay);
        canAttack = true;
        attacking = false;
        navAgent.speed = speed;
        SetRandomDestination();

        base_damage.SetActive(true);
        charge.SetActive(false);
        jump.SetActive(false);
        stomp.SetActive(false);
    }

    //Returns false if we cant make a new attack
    void SelectRandomAttack()
    {
        base_damage.SetActive(false);
        //Lets see if we should make a new attack and then do that. 
        int attack = Random.Range(0, 3);
        switch (attack)
        {
            case 0:
                PerformStompAttack();
                break;
            //case 1:
            //    PerformJumpAttack();
            //    break;
            case 2:
                PerformChargeAttack();
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    private IEnumerator PerformStompAttack()
    { 
        stomp.SetActive(true);
        canAttack = false;
        return AttackCooldown(5);
    }

    private IEnumerator PerformJumpAttack()
    {  
        canAttack = false;
        Debug.Log("Jump Attack Triggered");
        return AttackCooldown(4);
    }

    private IEnumerator PerformChargeAttack()
    {
        charge.SetActive(true);
        canAttack = false;
        RaycastHit hit;
        Physics.Raycast(new Ray(transform.position, transform.forward * 100), out hit);

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(hit.point, out navHit, 50, NavMesh.AllAreas))
        {
            navAgent.SetDestination(navHit.position);
            navAgent.speed = speed * 10;
        }

        return AttackCooldown(3);
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
