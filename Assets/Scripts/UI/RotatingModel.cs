using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingModel : MonoBehaviour
{
    public float rotationSpeed = 100f; // adjust this to control rotation speed

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // rotates around Y axis
    }
}
