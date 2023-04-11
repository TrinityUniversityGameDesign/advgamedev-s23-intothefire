using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Transform _target;
    public Camera cam;

    void Start()
    {
        _target = transform.parent;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_target.position.x, transform.position.y, _target.position.z);
        transform.eulerAngles = new Vector3(90f, 0f, 0f) ;
        
        
    }
}
