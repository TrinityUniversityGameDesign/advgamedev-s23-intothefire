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
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startNode.transform.position;
        nextNode = startNode.GetComponent<MovingPlatformNode>().nextNode;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, nextNode.transform.position) >= distanceCutoff){
            rb.velocity = (nextNode.transform.position - transform.position).normalized * speed;
        }
        else{
            nextNode = nextNode.GetComponent<MovingPlatformNode>().nextNode;
        }
    }
}
