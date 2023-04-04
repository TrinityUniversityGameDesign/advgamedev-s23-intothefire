using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeEnemyTrigger : MonoBehaviour
{
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider target){
        if(target.transform.tag == "Player"){
            Vector3 direction = -(target.transform.position - transform.parent.transform.position).normalized;
            Rigidbody parentRB = transform.parent.GetComponent<Rigidbody>();
            parentRB.velocity = direction * speed;
        }
    }
}
