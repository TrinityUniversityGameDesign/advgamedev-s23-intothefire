using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class NewCharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public enum PlayerState
    {
        idle,
        attack,
        special,
        invincible,
        hitstun,
        spawn,
        dead
    }
    PlayerState state = PlayerState.spawn;
    //JacksonPlayerControls playerMovement;
    GameObject sword;
    GameObject currSword = null;
    Quaternion targetRot;
    bool grounded = true;
    bool damnYouGabriel = false;
    float gravMult = 1f;
    private LineRenderer lr;
    GameObject enemy = null;
    
    public List<Item> inventory = new List<Item>();
    
    
    List<Item> Moveinventory = new List<Item>();
    List<Item> Jumpinventory = new List<Item>();
    List<Item> Lightinventory = new List<Item>();
    List<Item> Heavyinventory = new List<Item>();
    List<Item> Specialinventory = new List<Item>();
    List<Item> Cooldowninventory = new List<Item>();
    private List<ItemData> _inventory = new List<ItemData>();

    Animation anim;
    float gravity = 1f;
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
    float stunTimer = 0f;
    float stunValue = 0f;
    float burnTime = 0f;
    Weapon weapon = new FryingPan();
    GameObject lastDam;


    //Here's a list of all the stats a player can obtain / items can modify:
    private PlayerStats _stats = new PlayerStats(new Dictionary<StatType, float>{
        { StatType.MaxHealth, 100f },
        { StatType.Health, 100f },
        { StatType.Armor, 0f },
        { StatType.AttackSpeed, 0f },
        { StatType.Critical, 0.1f },
        { StatType.Damage, 0f },
        { StatType.DamageOverTime, 0f },
        { StatType.KnockBack, 0f },
        { StatType.KnockBackResist, 0f },
        { StatType.LifeGain, 0f },
        { StatType.LifeSteal, 0f },
        { StatType.MaxJumps, 1f },
        { StatType.MaxSpecials, 1f },
        { StatType.Speed, 20f } 
    });
    //public float health { get; private set; } = 100f;
    int damTime = 0;
    float currBurn = 0f;
    float currJumps = 1f;
    float currSpecials = 1f;
    float jumpHeight = 20f;
    float repeatTimer = 0f;

    bool invincible = false;


    Vector3 oldSpeed = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    Camera cam;
    Vector2 ul;
    Vector3 lassoPoint = Vector3.zero;
    //JacksonPlayerControls.JacksonControlsActions inputs;
    float v;
    float h;
    Rigidbody rb;
    CharacterController cc;
    bool lasso = false;
    
    float timer = 0;
    GameObject player;
    
    // UI Variables
    public Sprite icon;
    private Camera _minicam;
    private HUDController _hud;
    
    private void Awake()
    {
        GameManager.Instance?.StartupNewGameBegin.AddListener(StartPlayer);
        GameManager.Instance?.EnablePlayerInvincibility.AddListener(EnableInvincible);
        GameManager.Instance?.DisablePlayerInvincibility.AddListener(DisableInvincible);
    }
    void Start()
    {
        GameObject plsWork = GameObject.Find("GameLogicDriver");

        if(plsWork == null)
        {
            //Debug.Log("there's no game logic");
            GameObject.Find("HUD").SetActive(false);
            //Debug.Log("yerr a wizard marry");
            transform.GetChild(1).gameObject.SetActive(false);
            damnYouGabriel = true;
            transform.position = GameObject.Find("PlayerInputManager").transform.position;
        }
        CapsuleCollider lazy = GetComponent<CapsuleCollider>();
        lazy.material.dynamicFriction = 0f;
        lazy.material.staticFriction = 0f;

        lazy.material.frictionCombine = PhysicMaterialCombine.Minimum;
        //playerMovement = new JacksonPlayerControls();
        //playerMovement.Enable();    
        cam = transform.GetChild(0).gameObject.GetComponent<Camera>();

        cam.transform.parent = null;
        cc = gameObject.GetComponent<CharacterController>();

        // Other variables
        sword = Resources.Load("Prefabs/TempJacksonPrefabs/Sword") as GameObject;
        
        //anim = GetComponent<Animation>();
        lr = GetComponent<LineRenderer>();
        
        weapon.AssignPlayer(this.gameObject);
        
        // UI Controllers
        _hud = GetComponentInChildren<HUDController>();
        // _minicam = GetComponentInChildren<Camera>();
        _minicam = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Camera>();
        Debug.Log(_minicam.name);
        // UI Variables
        icon = Resources.Load<Sprite>("Sprites/gun");
    }

    private void UpdateMinimap()
    {
        Transform camTransform = _minicam.transform;
        camTransform.position = new Vector3(transform.position.x, camTransform.position.y, transform.position.z);
        camTransform.eulerAngles = new Vector3(90f, 0f, 0f) ;
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
    public void ToggleInventory(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _hud.ToggleInventory();
    }
    // Update is called once per frame
    void Update()
    {
        //jumpHold = inputs.Jump.ReadValue<float>() > 0.1f;
        // lightHold = inputs.LightAttack.ReadValue<float>() > 0.1f;
        //heavyHold = inputs.HeavyAttack.ReadValue<float>() > 0.1f;
        //dodgeHold = inputs.Dodgeroll.ReadValue<float>() > 0.1f;
        
        if (damnYouGabriel)
        {
            transform.GetChild(1).gameObject.SetActive(false);
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
        // UpdateMinimap();

    }

    private void FixedUpdate()
    {
        //Vector2 ul = inputs.Move.ReadValue<Vector2>();
        //Debug.Log(ul);
        //Debug.Log(health);
        grounded = cc.isGrounded;
        h = ul.x;
        v = ul.y;
        if(damTime == 0)
        {
            damTime--;
            lastDam = null;
        }
        else if(damTime > 0)
        {
            damTime--;
        }

        if(Magnitude() < 1f)
        {
            //anim.Play("idle");
        }
        else
        {
            //anim.Play("run");
        }
        if(velocity.y > 0)
        {
            //anim.Play("jumpUp");
        }
        else if(velocity.y < 0 && !grounded)
        {
            //anim.Play("jumpDown");
        }
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

        if(repeatTimer >= 50)
        {
            repeatTimer = 0;
            if (currBurn > 0)
            {
                _stats.TakeDamage(-Mathf.Ceil(currBurn / 10f));
                //burnTime--;
                currBurn -= Mathf.Ceil(currBurn / 10f);
                if (currBurn <= 0)
                {
                    currBurn = 0;
                    burnTime = 0;
                }
            }
            _stats.Heal(_stats.LifeGain);
            // if(_stats.Health > _stats.MaxHealth) _stats.SetHealth(_stats.MaxHealth);
            foreach (Item it in Cooldowninventory)
            {
                it.ItemCooldown();
            }
        }
        else
        {
            repeatTimer++;
        }
        //Debug.Log("grounded: " + grounded);
        switch (state)
        {
            
            case PlayerState.idle:
                {
                    if (grounded)
                    {
                        //rb.useGravity = true;
                        gravMult = 3;
                        currSpecials = _stats.MaxSpecials;
                        currJumps = _stats.MaxJumps;
                        velocity = new Vector3(velocity.x, 0f, velocity.z);
                    }
                    else
                    {
                        gravMult = 1;
                        //rb.useGravity = false;
                    }
                    if(_stats.Health < 0)
                    {
                        state = PlayerState.dead;
                    }
                    velocity = velocity + (new Vector3(0f, -1f, 0f) * (gravMult * gravity));
                    //rb.AddForce(new Vector3(0f, -1f, 0f) * (gravMult*gravity));
                    MovementManagement(h, v);
                    if (lightPress > 0)
                    {
                        foreach(Item it in Lightinventory)
                        {
                            it.ItemLight();
                        }
                        lightPress = 0f;
                        state = PlayerState.attack;
                        //anim.Play("lightAttack");
                        currSword = Instantiate(sword, transform.position, transform.rotation);
                        currSword.transform.parent = transform;
                        currSword.GetComponent<DamageScript>().SetParent(this.gameObject);
                        currSword.GetComponent<DamageScript>().SetDamage(CalculateDamage(weapon.lightDamage));
                        currSword.GetComponent<DamageScript>().SetKnockback(weapon.lightKnockback + _stats.KnockBack);
                        currSword.GetComponent<DamageScript>().SetDamageOverTime(_stats.DamageOverTime);
                        if (_stats.LifeSteal > 0)
                        {
                            currSword.GetComponent<DamageScript>().DoLifesteal(this.gameObject);
                        }
                        
                        //currSword.transform.rotation = Quaternion.AngleAxis(90f, Vector3.right) * transform.rotation;// * Quaternion.Euler(0f, 0f, 90f);
                        timer = 30f;
                        // currSword.transform.localRotation = transform.rotation * Quaternion.Euler(0f, 0f, 90f);
                        targetRot = currSword.transform.localRotation * Quaternion.AngleAxis(-45f, Vector3.up); //* currSword.transform.localRotation;
                        currSword.transform.localRotation = currSword.transform.localRotation * Quaternion.AngleAxis(45f, Vector3.up); //* currSword.transform.localRotation; //* Quaternion.Euler(0f, -45f, 0f);
                        lerpTime = weapon.lightSpeed + _stats.AttackSpeed;
                    }
                    if(heavyPress > 0)
                    {
                        foreach (Item it in Heavyinventory)
                        {
                            it.ItemHeavy();
                        }
                        heavyPress = 0f;
                        state = PlayerState.attack;
                        //anim.Play("heavyAttack");
                        currSword = Instantiate(sword, transform.position, transform.rotation);
                        
                        currSword.transform.parent = transform;
                        currSword.GetComponent<DamageScript>().SetParent(this.gameObject);
                        currSword.GetComponent<DamageScript>().SetDamage(CalculateDamage(weapon.heavyDamage));
                        currSword.GetComponent<DamageScript>().SetKnockback(weapon.heavyKnockback + _stats.KnockBack);
                        currSword.GetComponent<DamageScript>().SetDamageOverTime(_stats.DamageOverTime);
                        if(_stats.LifeSteal > 0)
                        {
                            currSword.GetComponent<DamageScript>().DoLifesteal(this.gameObject);
                        }
                        //currSword.transform.rotation = Quaternion.AngleAxis(90f, Vector3.right) * transform.rotation;// * Quaternion.Euler(0f, 0f, 90f);
                        timer = 30f;
                        // currSword.transform.localRotation = transform.rotation * Quaternion.Euler(0f, 0f, 90f);
                        targetRot = currSword.transform.localRotation;  //* currSword.transform.localRotation;
                        currSword.transform.localRotation = currSword.transform.localRotation * Quaternion.AngleAxis(-90f, Vector3.right); //* currSword.transform.localRotation; //* Quaternion.Euler(0f, -45f, 0f);                       
                           // GameObject bullet = Instantiate(Resources.Load("Prefabs/IceBullet") as GameObject, transform.position + transform.forward * 2f, transform.rotation);
                            //bullet.GetComponent<Rigidbody>().velocity = transform.forward * 15f;
                       
                        lerpTime = weapon.heavySpeed + _stats.AttackSpeed;
                    }
                    if(jumpPress > 0 && currJumps >0)
                    {
                        foreach (Item it in Jumpinventory)
                        {
                            it.ItemJump();
                        }
                        grounded = false;
                        currJumps--;
                        jumpPress = 0;
                        if (velocity.y < 0)
                        {
                            velocity = new Vector3(velocity.x, 0f, velocity.z);
                        }
                        velocity = velocity + new Vector3(0f, jumpHeight, 0f);
                        //rb.AddRelativeForce(new Vector3(0, jumpHeight, 0f), ForceMode.VelocityChange);
                    }
                    else if (jumpPress > 0 && currJumps == 0)
                    {
                        jumpPress = 0;
                        currJumps--;
                        if (velocity.y < 0)
                        {
                            velocity = new Vector3(velocity.x, 0f, velocity.z);
                        }
                        //rb.AddRelativeForce(new Vector3(0, jumpHeight / 2f, 0f), ForceMode.VelocityChange);
                        velocity = velocity + new Vector3(0f, jumpHeight / 2f, 0f);

                    }
                    if(specialPress > 0 && currSpecials > 0)
                    {
                        foreach (Item it in Specialinventory)
                        {
                            it.ItemSpecial();
                        }
                        currSpecials--;
                        //isDodging = true;
                        state = PlayerState.special;
                        //anim.Play("specialAttack");
                        if (h != 0 || v != 0)
                        {
                            Rotating(h, v);
                        }
                        currSword = Instantiate(sword, transform.position, transform.rotation);
                        currSword.transform.parent = transform;
                        currSword.GetComponent<DamageScript>().SetParent(this.gameObject);
                        currSword.GetComponent<DamageScript>().SetDamage(CalculateDamage(weapon.specialDamage));
                        currSword.GetComponent<DamageScript>().SetKnockback(weapon.specialKnockback + _stats.KnockBack);
                        currSword.GetComponent<DamageScript>().SetDamageOverTime(_stats.DamageOverTime);
                        if (_stats.LifeSteal > 0)
                        {
                            currSword.GetComponent<DamageScript>().DoLifesteal(this.gameObject);
                        }
                        weapon.AssignHitbox(currSword);
                        //rb.AddRelativeForce(new Vector3(0, 0, 10f), ForceMode.VelocityChange);

                    }
                    
                }
                break;
            case PlayerState.attack:
                {
                    //Movement shit
                    if ((grounded && !airStrike) || (h == 0f && v == 0f))
                    {
                        if (Magnitude() < 4f)
                        {
                            velocity = new Vector3(0f, velocity.y, 0f);
                        }
                        else
                        {
                            Vector2 drag = new Vector2(velocity.x, velocity.z);
                            drag = drag.normalized;
                            Vector3 lazy = new Vector3(drag.x, 0f, drag.y);

                            velocity = lazy * (Magnitude() - 3f) + new Vector3(0f, velocity.y, 0f);
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
                            velocity = new Vector3(oldSpeed.x, velocity.y, oldSpeed.z);
                        }
                    }
                    
                    if (ArbitraryMagnitude(oldSpeed) < Magnitude())
                    {
                        oldSpeed = velocity;
                    }
                        
                    //rb.AddForce(new Vector3(0f, -1f, 0f) * gravity);
                    velocity = velocity + (new Vector3(0f, -1f, 0f) * gravity);

                    //combat shit
                    currSword.transform.localRotation = Quaternion.Lerp(currSword.transform.localRotation, targetRot, lerpTime) ;
                    if(currSword.transform.localRotation == targetRot || timer < 0)
                    {
                        Destroy(currSword);
                        if (h != 0f || v != 0f)
                        {
                            velocity = new Vector3(oldSpeed.x, velocity.y, oldSpeed.z);
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
                    weapon.AssignHitbox(currSword);
                    Debug.Log("currently in the special state");
                    bool ahhh = weapon.SpecialAttack(h,v);
                    if (ahhh)
                    {

                    }
                    else
                    {
                        iFrames = -1;
                        Destroy(currSword);
                        state = PlayerState.idle;
                    }
                    //isDodging = false;
                }
                
                break;
            case PlayerState.invincible:
                {
                    if (isDodging)
                    {
                        velocity = transform.forward * 30f;

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
                    velocity = velocity + (new Vector3(0f, -1f, 0f) * gravity);
                    //rb.AddForce(new Vector3(0f, -1f, 0f) * gravity);
                }
                break;
            case PlayerState.spawn:
                {
                    velocity = Vector3.zero;
                }break;
            case PlayerState.hitstun:
                {
                    //rb.AddForce(new Vector3(0f, -1f, 0f) * gravity);
                    velocity = velocity + (new Vector3(0f, -1f, 0f) * gravity);
                    if ((grounded && stunTimer <= 0) || stunTimer < -80)
                    {
                        //stunValue = 0;
                        stunTimer = 0;
                        transform.rotation = Quaternion.identity;
                        state = PlayerState.idle;
                    }
                    else
                    {
                        stunTimer--;
                        if(h !=0 || v != 0)
                        {
                            Rotating(h, v);
                            //rb.AddRelativeForce(new Vector3(0, 0, 0.5f), ForceMode.VelocityChange);
                            velocity = velocity + (transform.forward.normalized * 0.5f);
                        }
                    }
                }break;

        }
        //cc.Move((Quaternion.AngleAxis(Vector3.Angle(velocity.normalized, transform.forward.normalized), Vector3.up)* velocity)* Time.deltaTime);
        cc.Move(velocity * Time.deltaTime);
    }
    public void StartPlayer()
    {
        Debug.Log("Starting player: " + gameObject.name);
        state = PlayerState.idle;
        //AddItem(new DamageItem());
        //AddItem(new KnockbackResistanceItem());
        //AddItem(new KnockbackItem());
        //AddItem(new DamageOverTimeItem());
        //AddItem(new AttackSpeedItem());
        //AddItem(new ArmorItem());
        AddItem(Resources.Load<ItemData>("Items/Spikey Ball"));
        AddItem(Resources.Load<ItemData>("Items/5 Pound Weight"));
        AddItem(Resources.Load<ItemData>("Items/Bassball Bat"));
        AddItem(Resources.Load<ItemData>("Items/Garlic"));
        AddItem(Resources.Load<ItemData>("Items/Feet"));
        AddItem(Resources.Load<ItemData>("Items/Torso"));
        AddItem(Resources.Load<ItemData>("Items/Energy Drink"));
        Debug.Log("Before HUD");
        //_hud.InitializePlayerHUD(icon, _stats.Health, _stats.MaxHealth, GetInventory(), GetInventoryStats());
        _hud.InitializePlayerHUD(icon, _stats.Health, _stats.MaxHealth, _inventory, _stats);
    }
    public void MovementManagement(float horizontal, float vertical)
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
            float angle = Vector3.Angle(velocity, transform.forward);
            if (grounded)
            {
                if (Magnitude() < _stats.Speed)
                {
                    velocity = Magnitude() * transform.forward + new Vector3(0f, velocity.y, 0f); ;
                    //rb.AddRelativeForce(new Vector3(0, 0, 2f), ForceMode.VelocityChange);
                    velocity = velocity + (transform.forward.normalized * 2f);
                }
                else if (Magnitude() >= _stats.Speed)
                {
                    velocity = Magnitude() * transform.forward + new Vector3(0f, velocity.y, 0f);
                }
            }
            else
            {
                if(Magnitude() < _stats.Speed)
                {
                    //rb.AddRelativeForce(new Vector3(0, 0, 2f), ForceMode.VelocityChange);
                    velocity = velocity + (transform.forward.normalized * 2f);
                }
                else
                {
                    if (Mathf.Abs(angle) < 30)
                    {

                    }
                    else
                    {
                        velocity = Magnitude() * .95f * new Vector3(velocity.x, 0f, velocity.z).normalized + new Vector3(0f, velocity.y, 0f);
                    }
                    if (ArbitraryMagnitude(velocity) < _stats.Speed)
                    {
                        //rb.AddRelativeForce(new Vector3(0, 0, 2f), ForceMode.VelocityChange);
                        velocity = velocity + (transform.forward.normalized * 2f);
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
                if (Magnitude() < 4f)
                {
                    velocity = new Vector3(0f, velocity.y, 0f);
                }
                else
                {
                    Vector2 drag = new Vector2(velocity.x, velocity.z);
                    drag = drag.normalized;
                    Vector3 lazy = new Vector3(drag.x, 0f, drag.y);

                    velocity = lazy * (Magnitude() - 3f) + new Vector3(0f, velocity.y, 0f);
                }
            //}
        }
        // Otherwise set the speed parameter to 0.
        //anim.SetFloat("Speed", 0);
        
    }

    public void Rotating(float horizontal, float vertical)
    {
        // Create a new vector of the horizontal and vertical inputs.
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        targetDirection = cam.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;

        // Create a rotation based on t$$anonymous$$s new vector assuming that up is the global y axis.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Create a rotation that is an increment closer to the target rotation from the player's rotation.
        //Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
        Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, 1);

        // Change the players rotation to t$$anonymous$$s new rotation.
        
        //rb.MoveRotation(newRotation);
        transform.rotation = newRotation;
    }

   public Camera GetCamera()
    {
        return cam;
    }
    public float Magnitude()
    {
        return Mathf.Sqrt((velocity.x * velocity.x) + (velocity.z * velocity.z));
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

    public bool GetGrounded()
    {
        return grounded;
    }
    public void StealLife(float steal)
    {
        _stats.Heal(Mathf.Ceil(steal*_stats.LifeSteal));
        // if(_stats.Health > _stats.MaxHealth) _stats.SetHealth(_stats.MaxHealth);
    }
    public void HurtPlayer(GameObject other)
    {
        DamageScript temp = other.GetComponent<DamageScript>();
        float hurts = Mathf.Max(0f, (temp.GetDamage() - _stats.Armor));

        if (!invincible)
        {
            _stats.SetHealth(_stats.Health - hurts);
            currBurn = temp.GetDamageOverTime();
            if (temp.GetLifesteal())
            {
                other.GetComponentInParent<JacksonPlayerMovement>().StealLife(hurts);
            }
        }

        float kb = temp.GetKnockback() - _stats.KnockBackResist;
        if (kb < 1) { kb = 1f; }
        state = PlayerState.hitstun;
        transform.LookAt(new Vector3(other.transform.position.x, transform.position.y - 1f, other.transform.position.z));
        stunValue = kb;
        kb *= -20;
        grounded = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        stunTimer = 10f;
        velocity = kb * transform.forward;
        Destroy(currSword);
    }

    public List<Item> GetInventory()
    {
        return inventory;
    }

    public void AddItem(Item i)
    {
        i.AssignPlayer(gameObject);
        i.ItemPickup();
        inventory.Add(i);
        if (i.ItemLight()) { Lightinventory.Add(i); }
        if (i.ItemHeavy()) { Heavyinventory.Add(i); }
        if (i.ItemJump()) { Jumpinventory.Add(i); }
        if (i.ItemMove()) { Moveinventory.Add(i); }
        if (i.ItemCooldown()) { Cooldowninventory.Add(i); }
        if (i.ItemSpecial()) { Specialinventory.Add(i); }
    }
    
    public void AddItem(ItemData itemData)
    {
        _stats.AddItem(itemData);
        Debug.Log("Pickup confirmed");
        _inventory.Add(itemData);
        Debug.Log("Added to inventory: " + _inventory);
    }
    public void AssignWeapon(Weapon w)
    {
        weapon = w;
        weapon.AssignPlayer(gameObject);
    }
    public float CalculateDamage(float d)
    {
        float rand = Random.Range(0f, 1f);
        float dmg = d + _stats.Damage;
        if (rand > _stats.Critical)
        {
            if (_stats.Critical > 1)
            {
                dmg = dmg * (1f + _stats.Critical);
            }
            else
            {
                dmg *= 2;
            }
        }
        dmg = Mathf.Ceil(dmg);
        return dmg;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Damage" && other.gameObject != lastDam && this.gameObject != other.gameObject.GetComponent<DamageScript>().GetParent())
        {
            //Debug.Log("ow damage");
            lastDam = other.gameObject;
            damTime = 60;
            HurtPlayer(other.gameObject);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Damage" && other.gameObject != lastDam && this.gameObject != other.gameObject.GetComponent<DamageScript>().GetParent())
        {
            //Debug.Log("ow damage");
            lastDam = other.gameObject;
            damTime = 60;
            HurtPlayer(other.gameObject);

        }
    }

    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }
    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public bool GetSpecialHold()
    {
        return specialHold;
    }

    public void ChangeHealth(float f)
    {
        _stats.AddStat("Health", f);
    }

    public void ChangeSpeed(float f)
    {
        _stats.AddStat("Speed", f);
    }

    public void ChangeDamage(float f)
    {
        _stats.AddStat("Damage", f);
    }

    public void ChangeAttackSpeed(float f)
    {
        _stats.AddStat("AttackSpeed", f);
    }

    public void ChangeArmor(float f)
    {
        _stats.AddStat("Armor", f);
    }

    public void ChangeCrit(float f)
    {
        _stats.AddStat("Critical", f);
    }

    public void ChangeLifesteal(float f)
    {
        _stats.AddStat("LifeSteal", f);
    }

    public void ChangeLifegain(float f)
    {
        _stats.AddStat("LifeGain", f);
    }

    public void ChangeDamageOverTime(float f)
    {
        _stats.AddStat("DamageOverTime", f);
    }

    public void ChangeKnockback(float f)
    {
        _stats.AddStat("KnockBack", f);
    }

    public void ChangeKnockbackResistance(float f)
    {
        _stats.AddStat("KnockBackResist", f);
    }

    public void ChangeMaxSpecials(float f)
    {
        _stats.AddStat("MaxSpecials", f);
    }

    public void ChangeMaxJumps(float f)
    {
        _stats.AddStat("MaxJumps", f);
    }
    public void ChangeJumpHeight(float f) { jumpHeight += f; }

    public void ChangeRange(float f) { }

    public List<(string, float)> GetInventoryStats()
    {
        List<(string, float)> temp = new List<(string, float)>();
        //temp.Add(("Max Health", maxHealth));
        //temp.Add(("Current Health", health));
        temp.Add(("Max Speed", _stats.Speed));
        temp.Add(("Damage", _stats.Damage));
        temp.Add(("Attack Speed", _stats.AttackSpeed));
        temp.Add(("Armor", _stats.Armor));
        temp.Add(("Crit Rate", _stats.Critical));
        temp.Add(("Lifesteal", _stats.LifeSteal));
        temp.Add(("Lifegain", _stats.LifeGain));
        temp.Add(("Damage Over Time", _stats.DamageOverTime));
        temp.Add(("Knockback", _stats.KnockBack));
        temp.Add(("Knockback Resist", _stats.KnockBackResist));
        temp.Add(("Max Air Specials", _stats.MaxSpecials));
        temp.Add(("Max Jumps", _stats.MaxJumps));
        temp.Add(("Jump Height", jumpHeight));
        return temp;
    }

    public float GetMaxHealth() { return _stats.MaxHealth; }
    public float GetHealth() { return _stats.Health; }
    public float GetSpeed() { return _stats.Speed; }
    public float GetDamage() { return _stats.Damage; }
    public float GetAttackSpeed() {return _stats.AttackSpeed; }
    public float GetArmor() {return _stats.Armor; }
    public float GetCrit() { return _stats.Critical; }
    public float GetLifesteal() { return _stats.LifeSteal; }
    public float GetLifegain() { return _stats.LifeGain; }
    public float GetDamageOverTime() { return _stats.DamageOverTime; }
    public float GetKnockback() { return _stats.KnockBack; }
    public float GetKockbackResistance() { return _stats.KnockBackResist; }
    public float GetMaxSpecials() {return _stats.MaxSpecials; }
    public float GetMaxJumps() { return _stats.MaxJumps; }
    public float GetJumpHeight() { return jumpHeight; }

    private void DisableInvincible()
    {
        invincible = false;
    }
    private void EnableInvincible()
    {
        invincible = true;
    }

    private void ToggleInventory()
    {
        _hud.ToggleInventory();
    }
}
