using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxSwing : MonoBehaviour
{
     public GameObject ax; // the GameObject that will swing
    public GameObject axis; // the axis GameObject

    public float amplitude = 30f; // the amount of swing (in degrees)
    public float speed = 1f; // the speed of the swing

    private Vector3 pivotPoint; // the point the ax will pivot around
    private Quaternion initialRotation; // the ax's initial rotation
    private float currentAngle = 0f; // the current angle of the swing

    void Start()
    {
        // set the pivot point to the position of the axis GameObject
        pivotPoint = axis.transform.position;

        // save the ax's initial rotation
        initialRotation = ax.transform.rotation;

        // Sets the damage it will deal to the players
        gameObject.GetComponent<DamageScript>().SetDamage(15f);
    }

    void Update()
    {
        // calculate the angle of the swing
        currentAngle = Mathf.Sin(Time.time * speed) * amplitude;

        // rotate the ax around the pivot point
        ax.transform.rotation = initialRotation * Quaternion.Euler(currentAngle, 0f, 0f);
        ax.transform.position = pivotPoint + ax.transform.up * -Mathf.Cos(currentAngle * Mathf.Deg2Rad);
    }
}
