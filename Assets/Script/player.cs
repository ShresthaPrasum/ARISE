using UnityEngine;
using UnityEngine.InputSystem;

public class PratikChalise : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 12f;
    
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public Animator animator;

    private Rigidbody2D rb;

    private float horizontalInput;
    private float coyoteCounter;
    
    public float jumpCutMultiplier = 0.69f;
    public float gracePeriod = 0.12f;
    public float jumpBufferTime = 0.12f;
    private float jumpForgiveTimer;
    public float normalGravity = 2f;
    public float droppingGravity = 4f;
    private bool isGrounded;
    private bool jumpCall;
    private bool jumpQueued;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        
        rb.linearDamping = 0f;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer
        );

        
        horizontalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed)
            {
                
                horizontalInput = 1f;
            }

            if (Keyboard.current.aKey.isPressed)
            {
                
                horizontalInput = -1f;
            }

            
            if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
            {
                jumpForgiveTimer = jumpBufferTime;
            }

            
            if (Keyboard.current.spaceKey.wasReleasedThisFrame ||Keyboard.current.wKey.wasReleasedThisFrame)
            {
                jumpCall = true;
            }
        }

        
        if (isGrounded)
        {
            coyoteCounter = gracePeriod;
        }

        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        
        if (jumpForgiveTimer > 0f)
        {
            jumpForgiveTimer -= Time.deltaTime;
        }

        
        if (jumpForgiveTimer > 0f && coyoteCounter > 0f)
        {
            jumpQueued = true;
            jumpForgiveTimer = 0f;
            coyoteCounter = 0f;
        }

        
        if (animator != null)
        {
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = horizontalInput * moveSpeed;

        if (jumpQueued)
        {
            rb.linearVelocityY = jumpForce;

            if (animator != null)
            {
                animator.SetTrigger("takeOff");
            }

            jumpQueued = false;
        }

        
        if (jumpCall && rb.linearVelocityY > 0f)
        {
            rb.linearVelocityY *= jumpCutMultiplier;
        }

        
        jumpCall = false;

        rb.gravityScale = rb.linearVelocityY < 0f? droppingGravity: normalGravity;

        if (animator == null)
        { 
            return;
        }
        animator.SetBool("isRunning", Mathf.Abs(horizontalInput) > 0.01f);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            checkRadius
        );
    }
}