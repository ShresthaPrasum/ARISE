using UnityEngine;
using UnityEngine.InputSystem;

public class SkeletonSummoner : MonoBehaviour
{
    public GameObject skeletonPrefab;

    public float summonRange = 5f;
    public LayerMask groundLayer;

    public Vector2 checkBoxSize = new Vector2(1.0f, 1.0f);

    public Transform indicator;

    public SpriteRenderer indicatorRenderer;

    public Color validColor = Color.white;

    public Color invalidColor = new Color(0.3f, 0.3f, 0.3f, 0.7f);

    public Animator playerAnimator;
    private SkeletonServant currentSkeleton;
    private Vector2 spawnPosition;
    private bool canSummonHere;
    private Camera mainCam;
    private PlayerMovement player;

    public LineRenderer rangeLine;
    public int circleSegments = 50;

    private void Start()
    {
        mainCam = Camera.main;
        player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (player != null && player.isDead) return;

        CheckMousePosition();
        HandleInput();
    }

    private void CheckMousePosition()
    {
        if (Mouse.current == null || mainCam == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);

        
        if (Vector2.Distance(transform.position, mouseWorldPos) > summonRange)
        {
            canSummonHere = false;
            UpdateIndicator(mouseWorldPos, false);
            return;
        }

        
        
        
        Vector2 rayStart = new Vector2(mouseWorldPos.x, mouseWorldPos.y + 1f);

        RaycastHit2D groundHit = Physics2D.Raycast(rayStart, Vector2.down, 2f, groundLayer);

        if (groundHit.collider != null)
        {
            
            spawnPosition = groundHit.point + new Vector2(0f, checkBoxSize.y * 0.5f);

            
            Collider2D wallCheck = Physics2D.OverlapBox(spawnPosition, checkBoxSize * 0.9f, 0f, groundLayer);
            canSummonHere = (wallCheck == null);

            
            UpdateIndicator(spawnPosition, canSummonHere);
        }
        else
        {
            canSummonHere = false;
            UpdateIndicator(mouseWorldPos, false); 
        }
    }

    private void UpdateIndicator(Vector2 position, bool isValid)
    {
        if (indicator == null) return;
        indicator.position = position;
        
        if (indicatorRenderer != null) 
        {
            indicatorRenderer.color = isValid ? validColor : invalidColor;
        }
    }

    private void HandleInput()
    {
        
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && canSummonHere)
        {
            SummonSkeleton();
        }

        
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            DismissSkeleton();
        }
    }

    private void SummonSkeleton()
    {
        if (currentSkeleton != null) 
        {
            Destroy(currentSkeleton.gameObject);
        }

        GameObject newObj = Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity);
        currentSkeleton = newObj.GetComponent<SkeletonServant>();

        
        float dir = Mathf.Sign(transform.localScale.x);
        currentSkeleton.SetDirection(dir);

        if (playerAnimator != null) 
        {
            playerAnimator.SetTrigger("summon");
        }
    }

    public void DismissSkeleton()
    {
        if (currentSkeleton != null)
        {
            Destroy(currentSkeleton.gameObject);
            currentSkeleton = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, summonRange);
        
        if (Application.isPlaying)
        {   
            Gizmos.color = canSummonHere ? Color.green : Color.red;      
            
            Gizmos.DrawWireCube(spawnPosition, checkBoxSize * 0.9f);
        }
    }
}