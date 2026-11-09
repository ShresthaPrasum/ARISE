using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 12f;
    public float jumpCutMultiplier = 0.5f; 

    public float normalGravity = 2f;
    public float droppingGravity = 4f; 


    public Transform groundCheck;
    public Vector2 checkSize = new Vector2(0.6f, 0.1f);
    public LayerMask groundLayer;

    public Animator animator;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool facingRight = true;
    public bool isDead = false;
    private Vector3 respawnPoint;

    public float respawnDelay = 1.5f;

    public Color deadColor = Color.red;

    public AudioSource audioSource;

    public AudioClip walkSound;

    public AudioClip landingSound;

    public AudioClip jumpSound;

    
    public Menu PauseMenu; // Reference to the Menu script

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        rb.linearDamping = 0f;
    }

    private void Update()
    {
        if (isDead) return;

        if(PauseMenu.GameIsPaused) return;

        
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapBox(groundCheck.position, checkSize, 0f, groundLayer);

        
        horizontalInput = 0f;
        
        if (Keyboard.current != null)
        {
            
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                
                horizontalInput = 1f;
            }
            else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                
                horizontalInput = -1f;
            }

            
            if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
            {
                if (isGrounded)
                {
                    Jump();
                }
            }

            
            if (Keyboard.current.spaceKey.wasReleasedThisFrame || Keyboard.current.wKey.wasReleasedThisFrame)
            {
                if (rb.linearVelocityY > 0f)
                {
                    rb.linearVelocityY *= jumpCutMultiplier;
                }
            }
        }

        if(horizontalInput > 0f && !facingRight)
        {
            facingRight = true;
            Vector3 currentScale = transform.localScale;
            currentScale.x = Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }
        else if(horizontalInput < 0f && facingRight)
        {
            facingRight = false;
            Vector3 currentScale = transform.localScale;
            currentScale.x = -Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }

        
        UpdateAnimations(wasGrounded);
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        
        rb.linearVelocityX = horizontalInput * moveSpeed;

        
        rb.gravityScale = (rb.linearVelocityY < 0f) ? droppingGravity : normalGravity;
    }


    public void FootSteps()
    {

        if (isGrounded && audioSource != null && walkSound != null)
        {
            audioSource.PlayOneShot(walkSound);
        }
    }
    private void Jump()
    {
        rb.linearVelocityY = jumpForce;

            animator.SetTrigger("takeOff"); 

            audioSource.PlayOneShot(jumpSound);
    }

    private void UpdateAnimations(bool wasGrounded)
    {
        if (animator == null) return;

        
        
        if (!wasGrounded && isGrounded)
        {
            animator.ResetTrigger("takeOff"); 

            audioSource.PlayOneShot(landingSound);
        }

        animator.SetBool("isRunning", Mathf.Abs(horizontalInput) > 0.01f);
        animator.SetBool("isGrounded", isGrounded);
    }


    public void setCheckpoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
    }

    public void Die()
    {
        if (isDead) return;
        
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in allSprites)
        {
            sprite.color = deadColor;
        }
        
        if (animator != null) 
            animator.SetTrigger("finalExit");
            
        Invoke(nameof(Respawn), respawnDelay);
    }

    public void Respawn()
    {
        transform.position = respawnPoint;

        rb.linearVelocity = Vector2.zero;

        SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in allSprites)
        {
            sprite.color = Color.white;
        }
        isDead = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, checkSize);
    }
}