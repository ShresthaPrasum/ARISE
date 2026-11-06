using UnityEngine;
using UnityEngine.InputSystem;

public class SkeletonSummoner : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public float summonRange = 5f;
    public float summonCooldown = 2f; 
    public LayerMask groundLayer;
    public Vector2 checkBoxSize = new Vector2(0.8f, 1.4f);
    public AudioSource audioSource;
    public AudioClip ariseSound;


    public Transform indicator;
    public SpriteRenderer indicatorRenderer;
    public Color validColor = Color.white;
    public Color invalidColor = new Color(0.3f, 0.3f, 0.3f, 0.7f);

    public LineRenderer rangeLine;
    public int circleSegments = 50;

    public Animator playerAnimator;

    private SkeletonServant currentSkeleton;
    private Vector2 spawnPosition;
    private bool canSummonHere;
    private Camera mainCam;
    private PlayerMovement player;
    
    private float cooldownTimer = 0f; 

    private void Start()
    {
        mainCam = Camera.main;
        player = GetComponent<PlayerMovement>();
        SetupRangeCircle();
    }

    private void SetupRangeCircle()
    {
        if (rangeLine == null) return;
        rangeLine.useWorldSpace = false;
        rangeLine.positionCount = circleSegments + 1;

        float angle = 0f;
        for (int i = 0; i < (circleSegments + 1); i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * summonRange;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * summonRange;
            rangeLine.SetPosition(i, new Vector3(x, y, 0f));
            angle += (360f / circleSegments);
        }
    }

    private void Update()
    {
        if (player != null && player.isDead) return;

        
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

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
        indicator.localScale = checkBoxSize * 0.9f;
        
        if (indicatorRenderer != null) 
        {
            
            if (cooldownTimer > 0f)
            {
                indicatorRenderer.color = invalidColor;
            }
            else
            {
                indicatorRenderer.color = isValid ? validColor : invalidColor;
            }
        }
    }

    private void HandleInput()
    {
        
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && canSummonHere && cooldownTimer <= 0f)
        {
            SummonSkeleton();
            cooldownTimer = summonCooldown; 
        }

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            DismissSkeleton();
        }
    }

    private void SummonSkeleton()
    {
        if (currentSkeleton != null) Destroy(currentSkeleton.gameObject);

        
        if (audioSource != null && ariseSound != null)
        {
            audioSource.PlayOneShot(ariseSound);
        }

        GameObject newObj = Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity);
        currentSkeleton = newObj.GetComponent<SkeletonServant>();

        playerAnimator.SetTrigger("Arise");
    }

    public void DismissSkeleton()
    {
        if (currentSkeleton != null)
        {
            Destroy(currentSkeleton.gameObject);
            currentSkeleton = null;
        }
    }
}