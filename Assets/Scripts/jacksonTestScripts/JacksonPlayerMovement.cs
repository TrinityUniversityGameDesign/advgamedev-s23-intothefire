using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacksonPlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public enum PlayerState
    {
        idle,
        attack,
        invincible,
        dead
    }
    PlayerState state = PlayerState.idle;
    JacksonPlayerControls playerMovement;
    GameObject sword;
    GameObject currSword = null;
    Quaternion targetRot;
    bool grounded = true;
    private LineRenderer lr;
    GameObject enemy = null;
    float health = 100;
    float gravity = 30f;
    float jumpPress = 0f;
    bool jumpHold = false;
    float lightPress = 0f;
    public LayerMask lm;
    bool lightHold = false;
    float heavyPress = 0f;
    bool heavyHold = false;
    float dodgePress = 0f;
    bool dodgeHold = false;
    bool isDodging = false;
    float iFrames = -1f;
    float lerpTime = 0.2f;

    Vector3 lassoPoint = Vector3.zero;
    JacksonPlayerControls.JacksonControlsActions inputs;
    float v;
    float h;
    Rigidbody rb;
    bool lasso = false;
    float maxSpeed = 20f;
    float timer = 0;
    void Start()
    {
        playerMovement = new JacksonPlayerControls();
        playerMovement.Enable();
        inputs = playerMovement.jacksonControls;
        rb = GetComponent<Rigidbody>();
        sword = Resources.Load("Prefabs/TempJacksonPrefabs/Sword") as GameObject;
        lr = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        jumpHold = inputs.Jump.ReadValue<float>() > 0.1f;
        lightHold = inputs.LightAttack.ReadValue<float>() > 0.1f;
        heavyHold = inputs.HeavyAttack.ReadValue<float>() > 0.1f;
        dodgeHold = inputs.Dodgeroll.ReadValue<float>() > 0.1f;
        if(lightHold)
        {
            //Debug.Log("butthonos wokr");
        }
        if (jumpHold)
        {
            //Debug.Log("jumpweeeeeee");
        }
        if (inputs.Jump.WasPerformedThisFrame())
        {
            jumpPress = 3;
        }
        if (inputs.LightAttack.triggered)
        {
           // Debug.Log("YOOO WE FIGHTING");
            lightPress = 3;
        }
        if (inputs.HeavyAttack.WasPerformedThisFrame())
        {
            heavyPress = 3;
        }
        if (inputs.Dodgeroll.triggered)
        {
            dodgePress = 3;
        }


    }

    private void FixedUpdate()
    {
        Vector2 ul = inputs.Move.ReadValue<Vector2>();
        //Debug.Log(ul);
        h = ul.x;
        v = ul.y;
        if(jumpPress > 0)
        {
            jumpPress--;
        }
        if(lightPress > 0)
        {
            lightPress--;
        }
        if(heavyPress > 0)
        {
            heavyPress--;
        }
        if(dodgePress > 0)
        {
            dodgePress--;
        }

        switch (state)
        {
            case PlayerState.idle:
                {
                    rb.AddForce(new Vector3(0f, -1f, 0f) * gravity);
                    MovementManagement(h, v);
                    if (lightPress > 0)
                    {
                        lightPress = 0f;
                        state = PlayerState.attack;
                        currSword = Instantiate(sword, transform.position, transform.rotation);
                        currSword.transform.parent = transform;
                        //currSword.transform.rotation = Quaternion.AngleAxis(90f, Vector3.right) * transform.rotation;// * Quaternion.Euler(0f, 0f, 90f);
                        timer = 30f;
                        // currSword.transform.localRotation = transform.rotation * Quaternion.Euler(0f, 0f, 90f);
                        targetRot = currSword.transform.localRotation * Quaternion.AngleAxis(-45f, Vector3.up); //* currSword.transform.localRotation;
                        currSword.transform.localRotation = currSword.transform.localRotation * Quaternion.AngleAxis(45f, Vector3.up); //* currSword.transform.localRotation; //* Quaternion.Euler(0f, -45f, 0f);
                        lerpTime = 0.2f;
                    }
                    if(heavyPress > 0)
                    {
                        heavyPress = 0f;
                        state = PlayerState.attack;
                        currSword = Instantiate(sword, transform.position, transform.rotation);
                        
                        currSword.transform.parent = transform;
                        //currSword.transform.rotation = Quaternion.AngleAxis(90f, Vector3.right) * transform.rotation;// * Quaternion.Euler(0f, 0f, 90f);
                        timer = 30f;
                        // currSword.transform.localRotation = transform.rotation * Quaternion.Euler(0f, 0f, 90f);
                        targetRot = currSword.transform.localRotation;  //* currSword.transform.localRotation;
                        currSword.transform.localRotation = currSword.transform.localRotation * Quaternion.AngleAxis(-90f, Vector3.right); //* currSword.transform.localRotation; //* Quaternion.Euler(0f, -45f, 0f);                       
                           // GameObject bullet = Instantiate(Resources.Load("Prefabs/IceBullet") as GameObject, transform.position + transform.forward * 2f, transform.rotation);
                            //bullet.GetComponent<Rigidbody>().velocity = transform.forward * 15f;
                       
                        lerpTime = 0.1f;
                    }
                    if(jumpPress > 0 && grounded)
                    {
                        grounded = false;
                        jumpPress = 0;
                        if (rb.velocity.y < 0)
                        {
                            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                        }
                        rb.AddRelativeForce(new Vector3(0, 15f, 0f), ForceMode.VelocityChange);
                    }
                    if(dodgePress > 0)
                    {
                        isDodging = true;
                        state = PlayerState.invincible;
                        Rotating(h, v);
                        rb.AddRelativeForce(new Vector3(0, 0, 10f), ForceMode.VelocityChange);

                    }
                    
                }
                break;
            case PlayerState.attack:
                {
                    MovementManagement(h, v);
                    rb.AddForce(new Vector3(0f, -1f, 0f) * gravity);
                    currSword.transform.localRotation = Quaternion.Lerp(currSword.transform.localRotation, targetRot, lerpTime) ;
                    if(currSword.transform.localRotation == targetRot || timer < 0)
                    {
                        Destroy(currSword);
                        state = PlayerState.idle;
                    }
                    else
                    {
                        timer--;
                    }
                    
                }
                break;
            case PlayerState.invincible:
                {
                    if (isDodging)
                    {
                        rb.velocity = transform.forward * 15f;
                        if(iFrames > 30)
                        {
                            iFrames = 30;
                        }
                    }
                    else
                    {
                        MovementManagement(h, v);
                        //rb.velocity = Vector3.zero;
                    }
                    if(iFrames == -1)
                    {
                        iFrames = 60;
                    }
                    else if(iFrames == 0)
                    {
                        iFrames = -1;
                        state = PlayerState.idle;
                        isDodging = false;
                    }
                    else
                    {
                        iFrames--;
                    }

                }
                break;

        }
        
    }

    void MovementManagement(float horizontal, float vertical)
    {
        // If there is some axis input...
        if (horizontal != 0f || vertical != 0f)
        {
            //Debug.Log("we moving");
            // ... set the players rotation and set the speed parameter to 5.3f.

            /*next steps make a way to get magnitude of vectors, 
             * implement a speed cap
             * make the player maintain more speed when turning
             * skid stop quick turn
             * faster acceleration?
             * make movement snappier and play around with numbers
             * */
            Rotating(horizontal, vertical);
            float angle = Vector3.Angle(rb.velocity, transform.forward);
            if (grounded)
            {
                if (Magnitude() < maxSpeed)
                {
                    rb.velocity = Magnitude() * transform.forward + new Vector3(0f, rb.velocity.y, 0f); ;
                    rb.AddRelativeForce(new Vector3(0, 0, 1f), ForceMode.VelocityChange);
                }
                else if (Magnitude() >= maxSpeed)
                {
                    rb.velocity = Magnitude() * transform.forward + new Vector3(0f, rb.velocity.y, 0f);
                }
            }
            else
            {
                if(Magnitude() < maxSpeed)
                {
                    rb.AddRelativeForce(new Vector3(0, 0, 1f), ForceMode.VelocityChange);
                }
                else
                {
                    rb.velocity = Magnitude() * .95f * new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized + new Vector3(0f, rb.velocity.y, 0f);
                    if (ArbitraryMagnitude(rb.velocity) < maxSpeed)
                    {
                        rb.AddRelativeForce(new Vector3(0, 0, 1f), ForceMode.VelocityChange);
                    }

                    /*
                    if (ArbitraryMagnitude(rb.velocity + new Vector3(0, 0, 1f)) <= maxSpeed)
                    {
                        rb.AddRelativeForce(new Vector3(0, 0, 1f), ForceMode.VelocityChange);
                    }
                    else
                    {
                        rb.velocity = Magnitude() * .95f * transform.forward + new Vector3(0f, rb.velocity.y, 0f); 
                    }
                    */
                }
            }
            //anim.SetFloat("Speed", 5.3f, speedDampTime, Time.deltaTime);
        }
        else
        {
            //if (!lasso.IsGrapple())
            //{
                if (Magnitude() < 3f)
                {
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                }
                else
                {
                    Vector2 drag = new Vector2(rb.velocity.x, rb.velocity.z);
                    drag = drag.normalized;
                    Vector3 lazy = new Vector3(drag.x, 0f, drag.y);

                    rb.velocity = lazy * (Magnitude() - 0.5f) + new Vector3(0f, rb.velocity.y, 0f);
                }
            //}
        }
        // Otherwise set the speed parameter to 0.
        //anim.SetFloat("Speed", 0);
    }

    void Rotating(float horizontal, float vertical)
    {
        // Create a new vector of the horizontal and vertical inputs.
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        targetDirection = Camera.main.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;

        // Create a rotation based on t$$anonymous$$s new vector assuming that up is the global y axis.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Create a rotation that is an increment closer to the target rotation from the player's rotation.
        //Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
        Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, 1);

        // Change the players rotation to t$$anonymous$$s new rotation.
        rb.MoveRotation(newRotation);
    }

    float Magnitude()
    {
        return Mathf.Sqrt((rb.velocity.x * rb.velocity.x) + (rb.velocity.z * rb.velocity.z));
    }

    float ArbitraryMagnitude(Vector3 mag)
    {
        return Mathf.Sqrt((mag.x * mag.x) + (mag.z * mag.z));
    }
    Vector3 RotateTowardsUp(Vector3 start, float angle)
    {
        start.Normalize();
        Vector3 axis = Vector3.Cross(start, Vector3.up);

        if (axis == Vector3.zero) axis = Vector3.right;

        return Quaternion.AngleAxis(angle, axis) * start;
    }

    public void SetGrounded(bool g)
    {
        grounded = g;
    }

    public void HurtPlayer(float f)
    {
        health -= f;
    }
}
