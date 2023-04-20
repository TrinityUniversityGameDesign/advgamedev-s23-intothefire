using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxSwing : MonoBehaviour
{
    [SerializeField] private GameObject ax; // the ax game object to swing
    [SerializeField] private Transform swingAxis; // the axis game object to swing around
    [SerializeField] private float maxAngle = 60.0f; // maximum angle of the swing
    [SerializeField] private float swingSpeed = 10.0f; // speed of the swing in degrees per second

    private float currentAngle = 0.0f; // current angle of the swing
    private float direction = 1.0f; // direction of the swing

    private void Update()
    {
        // calculate the new angle based on the current time and swing speed
        float deltaAngle = direction * swingSpeed * Time.deltaTime;
        currentAngle += deltaAngle;

        // flip the swing direction if the angle reaches the maximum
        if (Mathf.Abs(currentAngle) > maxAngle) {
            currentAngle = Mathf.Sign(currentAngle) * maxAngle;
            direction *= -1.0f;
        }

        // calculate the rotation axis and angle
        Vector3 rotationAxis = swingAxis.TransformDirection(Vector3.right);
        float rotationAngle = deltaAngle;

        // rotate the ax around the swing axis by the new angle
        ax.transform.RotateAround(swingAxis.position, rotationAxis, rotationAngle);
    }
}
