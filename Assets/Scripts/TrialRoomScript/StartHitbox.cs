using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHitbox : MonoBehaviour
{   
    private void OnTriggerStay(Collider other) 
    {   
        Debug.Log(other.tag);
        if (other.tag == "Player") {
            Destroy(gameObject);
            transform.parent.GetComponent<EmptyRoom>().StartTrial();
        }
    }
}
