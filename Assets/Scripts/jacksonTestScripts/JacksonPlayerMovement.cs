using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JacksonPlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public enum PlayerState
    {
        idle,
        attack,
        special,
        invincible,
        spawn,
        dead
    }
    PlayerState state = PlayerState.spawn;
    //JacksonPlayerControls playerMovement;
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
    float specialPress = 0f;
    bool specialHold = false;
    bool isDodging = false;
    float iFrames = -1f;
    bool airStrike = false;
    float lerpTime = 0.2f;
    Vector3 oldSpeed = Vector3.zero;
    Camera cam;
    Vector2 ul;
    Vector3 lassoPoint = Vector3.zero;
    //JacksonPlayerControls.JacksonControlsActions inputs;
    float v;
    float h;
    Rigidbody rb;
    bool lasso = false;
    float maxSpeed = 20f;
    float timer = 0;
    GameObject player;
    void Start()
    {
        //playerMovement = new JacksonPlayerControls();
        //playerMovement.Enable();    
        cam = transform.parent.GetChild(1).gameObject.GetComponent<Camera>();
        //inputs = playerMovement.jacksonControls;
        player = transform.GetChild(0).gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        sword = Resources.Load("Prefabs/TempJacksonPrefabs/Sword") as GameObject;
        lr = GetComponent<LineRenderer>();
        GameManager.Instance?.FirstTimeStartup.AddListener(StartPlayer);
        if(GameManager.Instance == null)
        {
            state = PlayerState.idle;
            transform.position = FindObjectOfType<PlayerInputManager>().transform.position;
        }
    }

    private void OnEnable()
    {
        
    }
    public void JumpInput(InputAction.CallbackContext ctx)
    {
        
        if (ctx.started)
        {
            jumpHold = true;
            jumpPress = 3;
            
        }
        else if (ctx.canceled)
        {
            jumpHold = false;
        }
        else
        {
            jumpHold = true;
        }
    }
    public void LightInput(InputAction.CallbackContext ctx)
    {
       
        if (ctx.started)
        {
            lightHold = true;
            lightPress = 3;
            
        }
        else if (ctx.canceled)
        {
            lightHold = false;
        }
        else
        {
            lightHold = true;
        }
    }
    public void HeavyInput(InputAction.CallbackContext ctx)
    {
        //Debug.Log("we jumping");
        if (ctx.started)
        {
            heavyHold = true;
            heavyPress = 3;
            //Debug.Log("we jumping");
        }
        else if (ctx.canceled)
        {
            heavyHold = false;
        }
        else
        {
            heavyHold = true;
        }
    }
    public void SpecialInput(InputAction.CallbackContext ctx)
    {
        //Debug.Log("we jumping");
        if (ctx.started)
        {
            specialHold = true;
            specialPress = 3;
            //Debug.Log("we jumping");
        }
        else if (ctx.canceled)
        {
            specialHold = false;
        }
        else
        {
            specialHold = true;
        }
    }
    public void MoveInput(InputAction.CallbackContext ctx)
    {
        //Debug.Log("we moving");
        ul = ctx.ReadValue<Vector2>();
    }
    // Update is called once per frame
    void Update()
    {
        //jumpHold = inputs.Jump.ReadValue<float>() > 0.1f;
       // lightHold = inputs.LightAttack.ReadValue<float>() > 0.1f;
        //heavyHold = inputs.HeavyAttack.ReadValue<float>() > 0.1f;
        //dodgeHold = inputs.Dodgeroll.ReadValue<float>() > 0.1f;
        if(lightHold)
        {
            //Debug.Log("butthonos wokr");
        }
        if (jumpHold)
        {
            //Debug.Log("jumpweeeeeee");
        }
        //if (inputs.Jump.WasPerformedThisFrame())
        //{
         //   jumpPress = 3;
        //}
        /*
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
        */


    }

    private void FixedUpdate()
    {
        //Vector2 ul = inputs.Move.ReadValue<Vector2>();
        //Debug.Log(ul);
        //Debug.Log(health);
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
        if(specialPress > 0)
        {
            specialPress--;
        }

        switch (state)
        {
            case PlayerState.idle:
                {
                    if(health < 0)
                    {
                        state = PlayerState.dead;
                    }
                    rb.AddForce(new Vector3(0f, -1f, 0f) * gravity);
                    MovementManagement(h, v);
                    if (lightPress > 0)
                    {
                        lightPress = 0f;
                        state = PlayerState.attack;
                        currSword = Instantiate(sword, transform.position, transform.rotation);
                        currSword.GetComponent<DamageScript>().SetDamage(10f);
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
                        currSword.GetComponent<DamageScript>().SetDamage(20f);
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
                    if(specialPress > 0)
                    {
                        //isDodging = true;
                        state = PlayerState.special;
                        Rotating(h, v);
                        currSword = Instantiate(sword, transform.position, transform.rotation);
                        currSword.transform.parent = transform;
                        currSword.GetComponent<DamageScript>().SetDamage(15f);
                        rb.AddRelativeForce(new Vector3(0, 0, 10f), ForceMode.VelocityChange);

                    }
                    
                }
                break;
            case PlayerState.attack:
                {
                    //Movement shit
                    if ((grounded && !airStrike) || (h == 0f && v == 0f))
                    {
                        if (Magnitude() < 3f)
                        {
                            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                        }
                        else
                        {
                            Vector2 drag = new Vector2(rb.velocity.x, rb.velocity.z);
                            drag = drag.normalized;
                            Vector3 lazy = new Vector3(drag.x, 0f, drag.y);

                            rb.velocity = lazy * (Magnitude() - 1f) + new Vector3(0f, rb.velocity.y, 0f);
                        }
                    }
                    else if (!grounded)
                    {
                        airStrike = true;
                    }
                    if (airStrike)
                    {
                        if (ArbitraryMagnitude(oldSpeed) > Magnitude() && (h != 0 && v !=0))
                        {
                            rb.velocity = new Vector3(oldSpeed.x, rb.velocity.y, oldSpeed.z);
                        }
                    }
                    
                    if (ArbitraryMagnitude(oldSpeed) < Magnitude())
                    {
                        oldSpeed = rb.velocity;
                    }

                    rb.AddForce(new Vector3(0f, -1f, 0f) * gravity);

                    //combat shit
                    currSword.transform.localRotation = Quaternion.Lerp(currSword.transform.localRotation, targetRot, lerpTime) ;
                    if(currSword.transform.localRotation == targetRot || timer < 0)
                    {
                        Destroy(currSword);
                        if (h != 0f || v != 0f)
                        {
                            rb.velocity = new Vector3(oldSpeed.x, rb.velocity.y, oldSpeed.z);
                        }
                        MovementManagement(h, v);
                        oldSpeed = Vector3.zero;
                        airStrike = false;
                        state = PlayerState.idle;
                    }
                    else
                    {
                        timer--;
                    }
                    //Movement shit
                    
                }
                break;
            case PlayerState.special:
                {
                    rb.velocity = transform.forward * 30f;

                    if (iFrames > 15)
                    {
                        iFrames = 15;
                    }
                }
                if (iFrames == -1)
                {
                    iFrames = 60;
                }
                else if (iFrames == 0)
                {
                    iFrames = -1;
                    Destroy(currSword);
                    state = PlayerState.idle;
                    //isDodging = false;
                }
                else
                {
                    iFrames--;
                }
                break;
            case PlayerState.invincible:
                {
                    if (isDodging)
                    {
                        rb.velocity = transform.forward * 30f;

                        if (iFrames > 15)
                        {
                            iFrames = 15;
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
            case PlayerState.dead:
                {
                    gravity = -30;
                    rb.AddForce(new Vector3(0f, -1f, 0f) * gravity);
                }
                break;
            case PlayerState.spawn:
                {
                    rb.velocity = Vector3.zero;
                }break;

        }
        
    }
    public void StartPlayer()
    {
        state = PlayerState.idle;
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
                    rb.AddRelativeForce(new Vector3(0, 0, 2f), ForceMode.VelocityChange);
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
                    rb.AddRelativeForce(new Vector3(0, 0, 2f), ForceMode.VelocityChange);
                }
                else
                {
                    if (Mathf.Abs(angle) < 30)
                    {

                    }
                    else
                    {
                        rb.velocity = Magnitude() * .95f * new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized + new Vector3(0f, rb.velocity.y, 0f);
                    }
                    if (ArbitraryMagnitude(rb.velocity) < maxSpeed)
                    {
                        rb.AddRelativeForce(new Vector3(0, 0, 2f), ForceMode.VelocityChange);
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

                    rb.velocity = lazy * (Magnitude() - 1f) + new Vector3(0f, rb.velocity.y, 0f);
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
        targetDirection = cam.transform.TransformDirection(targetDirection);
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
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Damage")
        {
            health -= other.gameObject.GetComponent<DamageScript>().GetDamage();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Damage")
        {
            health -= other.gameObject.GetComponent<DamageScript>().GetDamage();
        }
    }
}
