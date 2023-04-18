using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody rb;
    private Animator anim;

    public bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;

        if (movement.magnitude >= 0.1f)
        {
            rb.MovePosition(rb.position + transform.TransformDirection(movement) * moveSpeed * Time.deltaTime);

            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown("space") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            anim.SetBool("isGrounded", false);

            anim.SetTrigger("jump");
        }

        if (Input.GetKeyDown("e"))
        {
            anim.SetTrigger("lightAttack");
        }

        if (Input.GetKeyDown("r"))
        {
            anim.SetTrigger("heavyAttack");
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isGrounded",true);
        }
    }
}
