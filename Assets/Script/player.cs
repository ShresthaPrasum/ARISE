using UnityEditor.Callbacks;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PratikChalise : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float jumpForce = 12f;

    public Animator animator;

    Rigidbody2D rb;

    public Transform groundCheck;
    public float checkRadius = 0.2f;

    public LayerMask groundLayer;

    public bool isGrounded;

    private float horizontalInput;

    private bool jumpCall;

    // private AudioManager audioManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        // audioManager = FindAnyObjectByType<AudioManager>();
    }
    

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer

        );

        horizontalInput = 0f;

        if (Keyboard.current.dKey.isPressed)
        {
            horizontalInput = 1f;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            horizontalInput = -1f;
        }

        if(isGrounded&&(Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.Space)))
        {
            jumpCall = true;
        }

        if(isGrounded&&(Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.Space))&& rb.linearVelocityY>0f)
        {
            rb.linearVelocityY *=0.69f;
        }

        if (rb.linearVelocityY < 0f)
        {
            rb.gravityScale = 10f;
        }

        else
        {
            rb.gravityScale = 2f;
        }

        rb.linearDamping = isGrounded ? 3f: 0f;

        animator.SetBool("isJumping", !isGrounded);

        }
    private void FixedUpdate()
    {
        rb.linearVelocityX = horizontalInput * moveSpeed;

        if(horizontalInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (jumpCall)
        {
            rb.AddForceY(jumpForce * 60f);
            animator.SetTrigger("takeOff");

            jumpCall = false;
        }

        
    
    }
    void OnDrawGizmosSelected()
    {
        if(groundCheck == null)
        return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }     


}