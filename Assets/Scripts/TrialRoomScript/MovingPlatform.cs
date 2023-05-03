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

    private Collider[] colliders;

    [SerializeField]
    private Transform children;

    void OnTriggerEnter(Collider target){
        // if(target.transform.tag == "Player"){
        //     target.transform.parent.transform.SetParent(transform);
        // }
        // else if(target.transform.tag == "Object"){
        //     target.transform.SetParent(transform);
        // }
        if(target.transform.tag == "Player" || target.transform.tag == "Object"){
            target.transform.SetParent(children);
            if(target.transform.tag == "Player"){
                DontDestroyOnLoad(target.gameObject);
            }
            // if(target.transform.tag == "Player"){
            //     Debug.Log("stay");
            // }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!children){
            children = transform.parent.transform.Find("Children");
            children.position = transform.position;
        }
        transform.position = startNode.transform.position;
        nextNode = startNode.GetComponent<MovingPlatformNode>().nextNode;
        rb = GetComponent<Rigidbody>();
        colliders = GetComponents<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        children.position = transform.position;
        foreach(Transform child in children){
            if(child.tag == "Player"){
                //Debug.Log("player");
                Collider col = child.GetComponent<Collider>();
                if(!col.bounds.Intersects(colliders[0].bounds)){
                    child.SetParent(null);
                    DontDestroyOnLoad(child.gameObject);
                }
            }
        }

        if(Vector3.Distance(transform.position, nextNode.transform.position) >= distanceCutoff){
            //rb.velocity = (nextNode.transform.position - transform.position).normalized * speed;
            transform.position = Vector3.MoveTowards(transform.position, nextNode.transform.position, stepSize);
        }
        else{
            nextNode = nextNode.GetComponent<MovingPlatformNode>().nextNode;
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
        if(target.transform.tag == "Player"){
            target.transform.SetParent(null);
            DontDestroyOnLoad(target.gameObject);
            //Debug.Log("left platform");
        }
        else if(target.transform.tag == "Object"){
            if(target.GetComponent<Box>()){
                target.transform.SetParent(target.GetComponent<Box>().hostRoom.transform);
            }
        }
    }
}
