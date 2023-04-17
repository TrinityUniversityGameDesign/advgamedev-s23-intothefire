using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapScript : MonoBehaviour
{
    public GameObject objectToMove;
    private float speed = 15f;
    private float distance = 10f;

    private float startY;

    void Start()
    {
        startY = objectToMove.transform.position.y;
    }

    void Update()
    {
        float newY = startY - Mathf.PingPong(Time.time * speed, distance);

        Vector3 newPosition = new Vector3(objectToMove.transform.position.x, newY, objectToMove.transform.position.z);
        objectToMove.transform.position = newPosition;
    }
}
