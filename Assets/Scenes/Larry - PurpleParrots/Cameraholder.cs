using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraholder : MonoBehaviour
{
    private float fixedYPosition = 2.5f; // Adjust this value to set the desired fixed y position

    void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = fixedYPosition;
        transform.position = cameraPosition;
    }
}
