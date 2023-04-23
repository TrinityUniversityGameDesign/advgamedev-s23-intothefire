using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnImpact : MonoBehaviour
{
    Collider collider;

    void Start() {
        collider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collider.isTrigger){Destroy(transform.parent.gameObject);}
    }
}
