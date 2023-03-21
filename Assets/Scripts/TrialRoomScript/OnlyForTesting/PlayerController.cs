using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    //public WeaponType weapon;

    public Transform cam;

    public float speed = 5000f;
    public float jumpPower = 5000f;
    public float turnSpeed = 1000f;
    public LayerMask groundMask;
    public float airDamping = 0.1f;
    private float playerHeight = 1f;
    
    private Transform groundCheck;
    public float groundDistance = 0.15f;
    public Rigidbody rb;

    private float angleVelocity;

    public bool canBeHit;

    void Start()
    {
        groundCheck = transform.Find("GroundCheck");
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        canBeHit = true;

        // if(!weapon){
        //     //weapon = new Sword(GetComponent<Transform>());
        //     weapon = gameObject.AddComponent<Sword>();
        // }
        // else{
        //     weapon.setPlayer(this);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // if(weapon){
        //     if(Input.GetMouseButtonDown(0)){
        //         //Debug.Log("Left Clicked");
        //         weapon.leftClick();
        //     }
            
        //     if(Input.GetMouseButtonDown(1)){
        //         //Debug.Log("Right Clicked");
        //         weapon.rightClick();
        //     }

        //     if(Input.GetKeyDown(KeyCode.LeftShift)){
        //         weapon.shift();
        //     }
        // }

        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        
        Vector3 moveForce = Vector3.zero;

        if (cam == null)
        {
            moveForce = vert * transform.forward + horiz * transform.right;
            float turnAmount =  turnSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
            transform.Rotate(transform.up, turnAmount);
        }
        else 
        {
            Vector3 inputDirection = new Vector3(horiz, 0f, vert);
            if (inputDirection.magnitude > 0.1f)
            {
                float inputAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                //Update inputAngle to move relative to camera angle
                inputAngle += cam.eulerAngles.y;

                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, inputAngle, ref angleVelocity, 0.1f);
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                moveForce = Quaternion.Euler(0f, inputAngle, 0f) * Vector3.forward;
            }
        }

        //bool grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //bool grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f);
        bool grounded = true;
        if (grounded)
        {
            rb.AddForce(speed * Time.deltaTime * moveForce);
            //rb.velocity = (speed * moveForce);
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpPower);    
            }
        }
        else // in air
        {
            rb.AddForce(airDamping * speed * Time.deltaTime * moveForce);
            Debug.Log("not grounded");
            //rb.velocity = (airDamping * speed * moveForce);
        }
    }



}
