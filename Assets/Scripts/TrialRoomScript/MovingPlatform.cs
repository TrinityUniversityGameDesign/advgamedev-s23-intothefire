using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject startNode;

    private GameObject nextNode;

    private Rigidbody rb;

    public float distanceCutoff = 0.5f;
    public float speed = 2f;
    public float stepSize = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startNode.transform.position;
        nextNode = startNode.GetComponent<MovingPlatformNode>().nextNode;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, nextNode.transform.position) >= distanceCutoff){
            //rb.velocity = (nextNode.transform.position - transform.position).normalized * speed;
            transform.position = Vector3.MoveTowards(transform.position, nextNode.transform.position, stepSize);
        }
        else{
            nextNode = nextNode.GetComponent<MovingPlatformNode>().nextNode;
        }
    }

    void OnTriggerEnter(Collider target){
        // if(target.transform.tag == "Player"){
        //     target.transform.parent.transform.SetParent(transform);
        // }
        // else if(target.transform.tag == "Object"){
        //     target.transform.SetParent(transform);
        // }
        if(target.transform.tag == "Player" || target.transform.tag == "Object"){
            target.transform.SetParent(transform);
        }
    }

    // void OnTriggerStay(Collider target){
    //     if(target.transform.tag == "Player"){
    //         target.transform.parent.transform.position = Vector3.MoveTowards(target.transform.parent.transform.position, nextNode.transform.position, stepSize);
    //     }
    //     else if(target.transform.tag == "Object"){
    //         target.transform.position = Vector3.MoveTowards(target.transform.position, nextNode.transform.position, stepSize);
    //     }
    // }

    void OnTriggerExit(Collider target){
        // if(target.transform.tag == "Player"){
        //     target.transform.parent.transform.SetParent(null);
        // }
        // else if(target.transform.tag == "Object"){
        //     target.transform.SetParent(null);
        // }
        if(target.transform.tag == "Player" || target.transform.tag == "Object"){
            target.transform.SetParent(null);
        }
    }
}
