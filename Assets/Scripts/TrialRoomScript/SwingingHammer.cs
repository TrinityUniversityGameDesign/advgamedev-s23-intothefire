using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingHammer : MonoBehaviour
{
    public float maxAngle = 45f;
    public float swingSpeed = 6f;
    public float startAngle = 45f;
    public bool limitAngle;
    public float anglePerSec = 360;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(limitAngle){
            float angle = maxAngle * Mathf.Cos(Time.time * swingSpeed + startAngle);
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
        }
        else{
            float angle = Time.deltaTime * anglePerSec;
            transform.Rotate(angle, 0, 0);
        }
    }
}
