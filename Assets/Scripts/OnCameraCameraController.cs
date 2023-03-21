using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnCameraCameraController : MonoBehaviour
{

    [SerializeField]
    public Camera camera;
    public Transform target; // The object that the camera will follow
    public float smoothSpeed = 0.125f; // The smoothing speed of the camera movement
    public Vector3 offset; // The offset of the camera from the target
    public float radius = 3f; // The distance of the camera from the target
    public float cameraSensitivity = 4f; // The sensitivity of the camera movement

    private Vector2 lookInput;


    private float yaw = 0f;
    private float pitch = 0f;

    public bool lostFocus = true;

  public void LookInput(InputAction.CallbackContext ctx)
  {
    lookInput = ctx.ReadValue<Vector2>();
  }

  private void Update()
    {
        if (target != null)
        {
          yaw -= lookInput.x;
          pitch += lookInput.y;

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
          camera.transform.position = Vector3.Lerp(camera.transform.position, desiredPosition, smoothSpeed);

          // Make the camera look at the target
          camera.transform.LookAt(target);
        } 
        else
        {
            lostFocus = true;
        }
    }
}
