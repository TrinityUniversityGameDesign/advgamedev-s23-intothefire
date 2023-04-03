using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnCameraCameraController : MonoBehaviour
{
    public Transform target; // The object that the camera will follow
    public float smoothSpeed = 0.125f; // The smoothing speed of the camera movement
    public Vector3 offset; // The offset of the camera from the target
    public float radius = 3f; // The distance of the camera from the target
    public float cameraSensitivity = 4f; // The sensitivity of the camera movement

    private float yaw = 0f;
    private float pitch = 0f;
    Vector2 inputs;
    public bool lostFocus = true;

    private void LateUpdate()
    {
        if (target != null)
        {
            /*
            Gamepad gamepad = Gamepad.current;
            if (gamepad != null)
            {
                Vector2 stickL = gamepad.rightStick.ReadValue();
                yaw -= stickL.x;
                pitch += stickL.y;
            }
            
            // Get mouse inputs for camera rotation
            yaw -= Input.GetAxis("Mouse X") * cameraSensitivity;
            pitch += Input.GetAxis("Mouse Y") * cameraSensitivity;
     
            */

            yaw -= inputs.x;
            pitch += inputs.y;
            lostFocus = false;
            // Limit pitch rotation

            pitch = Mathf.Clamp(pitch, 10f, 80f);
            // Convert spherical coordinates to Cartesian coordinates
            float x = radius * Mathf.Sin(pitch * Mathf.Deg2Rad) * Mathf.Cos(yaw * Mathf.Deg2Rad);
            float y = radius * Mathf.Cos(pitch * Mathf.Deg2Rad);
            float z = radius * Mathf.Sin(pitch * Mathf.Deg2Rad) * Mathf.Sin(yaw * Mathf.Deg2Rad);

            // Calculate the desired position of the camera
            Vector3 desiredPosition = target.position + new Vector3(x, y, z);

            // Smoothly move the camera to the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Make the camera look at the target
            transform.LookAt(target);
        } else
        {
            lostFocus = true;
        }
    }
    public void CameraInput(InputAction.CallbackContext ctx)
    {
        //Debug.Log("we moving");
        inputs = ctx.ReadValue<Vector2>();
        //yaw -= lazy.x * cameraSensitivity;
        //pitch += lazy.y * cameraSensitivity;


    }
}
