using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public GameObject target;
    public Camera cam;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.eulerAngles = new Vector3(90f, cam.transform.eulerAngles.y, 0f) ;
        
        
    }
}
