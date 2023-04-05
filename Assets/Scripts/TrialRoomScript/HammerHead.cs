using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHead : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<DamageScript>().SetDamage(30f);
        //GetComponent<Rigidbody>().AddForce(10,0,0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
