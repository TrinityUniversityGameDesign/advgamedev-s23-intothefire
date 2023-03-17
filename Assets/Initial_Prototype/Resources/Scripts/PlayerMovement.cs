using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

  public Camera camera;

  [SerializeField]
  private GameObject player;

  [Header("Movement")]
  [SerializeField]
  private float moveSpeed;
  //public Transform orientation;
  private Vector2 moveInput;
  private Rigidbody rb;
  private Vector3 moveDirection;

  private float currVel;

  [Header("Ground Checks")]
  [SerializeField]
  private float playerHeight;
  [SerializeField]
  private float groundDrag;
  private bool grounded;

  [Header("Jumping")]
  [SerializeField]
  private float gravity = -9.8f;
  private bool jumpInput;
  [SerializeField]
  private float jumpForce;
  [SerializeField]
  private float airMultiplier;


  private void Awake()
	{
    player = this.gameObject.transform.GetChild(0).gameObject;
    camera = this.gameObject.transform.GetChild(1).GetComponent<Camera>();
    rb = player.GetComponent<Rigidbody>();
  }

	void FixedUpdate()
  {

    ApplyGravity();

    GroundedCheck();

    Move();

    SpeedLimiter();

    ApplyDrag();
  }

  public void MoveInput(InputAction.CallbackContext ctx)
  {
    moveInput = ctx.ReadValue<Vector2>();
  }
  public void JumpInput(InputAction.CallbackContext ctx)
  {
    if (ctx.started && grounded) Jump();
  }

  private void Move()
  {
    { 
    //if (moveInput != Vector2.zero)
    //{
    /*//Get angle of movement (Atan2 is tangent, but always defined as clockwise rotation starting in positive xy space
    float inputAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

    inputAngle += camera.transform.rotation.eulerAngles.y;

    float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, inputAngle, ref currVel, 0.1f);

    //rotate player where they are moving
    player.transform.rotation = Quaternion.Euler(0f, inputAngle, 0f);

    //Input forward is camera forward, regardless of player rotation
    moveDirection = Quaternion.Euler(0f, inputAngle, 0f) * Vector3.forward;*/
    //Rotate character to be aligned with the camera
    //var rotation = Quaternion.LookRotation(forward);
    //rotation *= player.transform.rotation;
    //player.transform.rotation = rotation;
    //}
    }//old code


    Vector3 forward = camera.transform.forward;
    forward.y = 0;
    forward = forward.normalized;
    player.transform.rotation = Quaternion.LookRotation(forward);

    moveDirection = player.transform.forward * moveInput.y + player.transform.right * moveInput.x;

		if (grounded)
    {
      rb.AddForce(moveSpeed * 10f * moveDirection.normalized, ForceMode.Force);
    }

    else if (!grounded)
		{
      rb.AddForce(moveSpeed * 10f * airMultiplier * moveDirection.normalized, ForceMode.Force);
    }
	}

  private void Jump()
	{ 
    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
  }

  private void GroundedCheck()
	{
    grounded = Physics.Raycast(player.transform.position, Vector3.down, playerHeight * 0.5f + 0.1f);
	}

  private void ApplyDrag()
	{
    if (grounded)
      rb.drag = groundDrag;
    else
      rb.drag = 0;
	}

  private void SpeedLimiter()
	{
    Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    //limit velocity if needed
    if(flatVel.magnitude > moveSpeed)
		{
      Vector3 limitedVel = flatVel.normalized * moveSpeed;
      rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
		}
	}

  private void ApplyGravity()
	{
    rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
	}

}
