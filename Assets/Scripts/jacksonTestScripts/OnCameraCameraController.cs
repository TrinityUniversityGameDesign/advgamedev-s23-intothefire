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
    Vector3 old;
    float flickTimer = 0;
    private float yaw = 0f;
    private float pitch = 90f;
    float lerpVal = 0.001f;
    float lerpValCam = 1;
    float lerpValPos = 1;
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
                yaw -= (Mathf.Clamp(inputs.x, -2f, 2f) * cameraSensitivity);
                pitch += inputs.y;
                lostFocus = false;
                // Limit pitch rotation
                //Debug.Log("here's the yaw: " + yaw);
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

                flickTimer = 0;
            }
            else
            {
                
                pitch = 60;
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
                if(flickTimer == 0)
                {
                    old = target.position;
                    lerpValCam = lerpVal;
                    lerpValPos = 1f;
                    flickTimer++;
                }
                if(flickTimer < 60)
                {
                    /*lerpValCam = lerpVal;
                    lerpValPos = 0f;
                    flickTimer++;*/
                }
                else
                {
                    lerpValCam = 1f;
                    lerpValPos = 1f;
                }
                Vector3 desiredPosition;
                if (targets[targetIndex] == null)
                {
                    lockOn = false;
                    targets[targetIndex] = target.gameObject;
                }
                else
                {
                    

                    // Calculate the desired position of the camera - You align the camera on the new player instead of the old player 
                    //Players never leave the selection radius - the cylinder is absolutely massive
                    Vector3 fakeTarget = new Vector3(target.position.x, target.position.y + 1f, target.position.z);
                    desiredPosition = new Vector3(targets[targetIndex].transform.position.x, target.position.y, targets[targetIndex].transform.position.z);
                    //desiredPosition = ((targets[targetIndex].transform.position /*.normalized * -radius*/));
                    //desiredPosition = ((target.position.normalized * -radius));
                    transform.position = fakeTarget + Vector3.Slerp(transform.position - fakeTarget, desiredPosition - fakeTarget, lerpValPos);
                    transform.position = fakeTarget + (fakeTarget - transform.position).normalized * 10f;

                    //if (flickTimer < 20)
                    //{


                    //transform.LookAt(targets[targetIndex].transform.position);
                    //Vector3 curr = transform.forward;
                    Vector3 curr = targets[targetIndex].transform.position;
                    //transform.forward = Vector3.Lerp(old, curr, lerpValCam);
                    //old = Vector3.Lerp(target.position, curr, lerpValCam);
                    old = Vector3.Lerp(old, curr, lerpValCam);
                    transform.LookAt(old);

                    //Debug.Log("look at pos: " + old + "target pos " + curr);
                    if (lerpValCam < 1)
                    {
                        lerpValCam += lerpVal;
                    }
                    else if (lerpValCam >= 1)
                    {
                        lerpValCam = 1;
                        lerpValPos = 1;
                    }
                    if (Mathf.Abs(target.position.y - targets[targetIndex].transform.position.y) > 20f)
                    {
                        lockOn = false;

                    }
                }
                /*if (Vector3.Distance(old, curr) < 0.5f)
                {
                    lerpValCam = 1;
                    lerpValPos = 1;
                }*/
                //transform.LookAt(Vector3.Lerp(old, curr, lerpVal));
                //flickTimer++;
                //}
                //else
                //{
                //transform.LookAt(targets[targetIndex].transform.position);
                //}
                if (!lockOn)
                {
                    targets.Clear();
                }

                float closestYaw = 0;
                float closestPos = 100000;
                for(float i = 0; i <=360; i+= 10)
                {
                    yaw = i;
                    pitch = Mathf.Clamp(pitch, 10f, 80f);
                    // Convert spherical coordinates to Cartesian coordinates
                    float x = radius * Mathf.Sin(pitch * Mathf.Deg2Rad) * Mathf.Cos(yaw * Mathf.Deg2Rad);
                    float y = radius * Mathf.Cos(pitch * Mathf.Deg2Rad);
                    float z = radius * Mathf.Sin(pitch * Mathf.Deg2Rad) * Mathf.Sin(yaw * Mathf.Deg2Rad);
                    desiredPosition = target.position + new Vector3(x, y, z);
                    if(Vector3.Distance(transform.position, desiredPosition) < closestPos)
                    {
                        closestYaw = i;
                        closestPos = Vector3.Distance(transform.position, desiredPosition);
                    }
                    yaw = closestYaw;
                    


                }
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
