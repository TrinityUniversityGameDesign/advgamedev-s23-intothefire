using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxSwing : MonoBehaviour
{
    public GameObject axe; // the axe game object to sway
    public GameObject axis; // the axis game object to sway around
    private float swaySpeed = 1.0f; // speed of the sway in degrees per second
    private float swayAngle = 1.0f; // maximum angle of the sway

    private bool isSwayingRight = true; // flag to indicate which direction the axe is swaying

    void Update()
    {
        // calculate the new angle based on the current time and sway speed
        float newAngle = isSwayingRight ? swayAngle : -swayAngle;
        newAngle *= Mathf.Sin(Time.time * swaySpeed * Mathf.Deg2Rad) * Time.deltaTime;

        // rotate the axe towards the new angle
        Quaternion targetRotation = Quaternion.Euler(0, newAngle, 0);
        axe.transform.rotation = Quaternion.RotateTowards(axe.transform.rotation, targetRotation, swaySpeed * Time.deltaTime);

        // flip the sway direction if the angle reaches the maximum
        if (Mathf.Abs(newAngle) >= swayAngle)
        {
            isSwayingRight = !isSwayingRight;
        }
    }

    
}
