using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class OnCameraCameraController : MonoBehaviour
{
    public Transform target; // The object that the camera will follow
    public float smoothSpeed = 0.125f; // The smoothing speed of the camera movement
    public Vector3 offset; // The offset of the camera from the target
    public float radius = 3f; // The distance of the camera from the target
    public float cameraSensitivity = 4f; // The sensitivity of the camera movement
    bool lockOn = false;
    private float yaw = 0f;
    private float pitch = 0f;
    bool leftMove = true;
    bool rightMove = true;
    List<GameObject> targets = new List<GameObject>();
    int targetIndex = 0;
    Vector2 inputs;
    public bool lostFocus = true;
    public bool shouldProcessInput = false;

    private void Start()
    {
        GameManager.Instance?.LobbyBegin.AddListener(DisableInput);
        GameManager.Instance?.LabyrinthExploreBegin.AddListener(EnablePlaying);
        gameObject.GetComponent<Camera>().enabled = false;
    }

    void DisableInput()
    {
        shouldProcessInput = false;
    }

    void EnablePlaying()
    {
        shouldProcessInput = true;
        gameObject.GetComponent<Camera>().enabled = true;
    }


    private void LateUpdate()
    {
        if (target != null && shouldProcessInput)
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
            if(targets.Count == 0)
            {
                lockOn = false;
            }
            if (!lockOn)
            {
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
            }
            else
            {
                if(inputs.x > 0.5f && rightMove)
                {
                    targetIndex = (targetIndex + 1) % targets.Count;
                    rightMove = false;
                }
                else if(inputs.x < 0.5f)
                {
                    rightMove = true;
                }
                if(inputs.x < -0.5f && leftMove)
                {
                    targetIndex = targetIndex - 1;
                    leftMove = false;
                    if(targetIndex < 0)
                    {
                        targetIndex = Mathf.Max(targets.Count - 1, 0);
                    }
                    
                }
                else if(inputs.x > -0.5f)
                {
                    leftMove = true;
                }
                Vector3 desiredPosition;
                // Calculate the desired position of the camera - You align the camera on the new player instead of the old player 
                //Players never leave the selection radius - the cylinder is absolutely massive
                Vector3 fakeTarget = new Vector3(target.position.x, target.position.y + 5f, target.position.z);

                desiredPosition = ((targets[targetIndex].transform.position /*.normalized * -radius*/));
                //desiredPosition = ((target.position.normalized * -radius));
                transform.position = fakeTarget + Vector3.Slerp(transform.position - fakeTarget, desiredPosition - fakeTarget, 1);
                transform.position = fakeTarget + (fakeTarget - transform.position).normalized * 10f;
                transform.LookAt(targets[targetIndex].transform.position);
            }
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

    public void ToggleLockInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            lockOn = !lockOn;
            //Debug.Log("we jumping");
        }
        
    }
    public void AddTarget(GameObject g)
    {
        if (!targets.Contains(g))
        {
            targets.Add(g);
        }
    }

    public void RemoveTarget(GameObject g)
    {
        targets.Remove(g);
    }
    public GameObject GetTarget()
    {
        return target.gameObject;
    }
}
