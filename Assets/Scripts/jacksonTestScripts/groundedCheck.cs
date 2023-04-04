using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundedCheck : MonoBehaviour
{
    // Start is called before the first frame update
    JacksonPlayerMovement parent;

    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<JacksonPlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        parent.SetGrounded(true); 
    }
    private void OnTriggerExit(Collider other)
    {
        parent.SetGrounded(false);
    }

}
