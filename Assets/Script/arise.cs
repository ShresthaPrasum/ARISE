using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SkeletonSummoner : MonoBehaviour
{
    public GameObject skeletonPrefab;

    public int maxDummies = 3;

    public float summonRange = 5f;

    public float summonCooldown = 2f; 

    public LayerMask groundLayer;

    public Vector2 checkBoxSize = new Vector2(0.8f, 1.4f);

    public AudioSource audioSource;

    public AudioClip ariseSound;


    public Transform indicator;
    public SpriteRenderer indicatorRenderer;

    public Image cursorCooldownBar;

    public TMP_Text dummyCountText;
    public Color validColor = Color.white;
    
    public Color invalidColor = new Color(0.3f, 0.3f, 0.3f, 0.7f);

    public LineRenderer rangeLine;

    public int circleSegments = 50;

    public Animator playerAnimator;

    private List<GameObject> activeDummies = new List<GameObject>();

    private Vector2 spawnPosition;

    private bool canSummonHere;

    private Camera mainCam;

    private PlayerMovement player;

    private float cooldownTimer = 0f;

    private int dummiesRemaining;

    public List<Image> dummyIcons = new List<Image>();


    private void Start()
    {
        mainCam = Camera.main;
        player = GetComponent<PlayerMovement>();

        dummiesRemaining = maxDummies;
        SetupRangeCircle();
    }

    private void SetupRangeCircle()
    {
        rangeLine.useWorldSpace = false;
        rangeLine.positionCount = circleSegments + 1;

        float angle = 0f;
        for (int i = 0; i < (circleSegments + 1); i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * summonRange;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * summonRange;
            rangeLine.SetPosition(i, new Vector3(x, y, 0f));
            angle += 360f / circleSegments;
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

        UpdateCursorUI();   
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

        indicator.position = position;
        indicator.localScale = checkBoxSize * 0.9f;


            bool readyToSpawn = (cooldownTimer <= 0f) && (dummiesRemaining > 0);

    
            for (int i = 0; i < dummyIcons.Count; i++)
            {
                Color tempColor = Color.white;
                
                tempColor.a = 97f/255f;
  
                dummyIcons[i].color = (i < dummiesRemaining) ? Color.white : tempColor;   
            }

            indicatorRenderer.color = 
            (isValid && readyToSpawn) ? validColor : invalidColor;
    
    }

    private void HandleInput()
    {
        
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (canSummonHere && cooldownTimer <= 0f && dummiesRemaining > 0)
            {
                SummonSkeleton();
                cooldownTimer = summonCooldown;
            }
        }

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            ClearAllDummies();
        }
    }

    private void SummonSkeleton()
    {
        if (audioSource != null && ariseSound != null)
        {
            audioSource.PlayOneShot(ariseSound);
        }

        GameObject newDummy = Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity);
        activeDummies.Add(newDummy);

        dummiesRemaining--;

            playerAnimator.SetTrigger("summon");
    }

    public void ClearAllDummies()
    {
        for (int i = 0; i < activeDummies.Count; i++)
        {
            Destroy(activeDummies[i]);
        }
        activeDummies.Clear();

    }

    private void UpdateCursorUI()
    {
        if (cursorCooldownBar != null)
        {
            float fillAmount = 1f;
            if (summonCooldown > 0f)
            {
                fillAmount = 1f - (cooldownTimer / summonCooldown);
            }
            cursorCooldownBar.fillAmount = fillAmount;

            Color barColor = cursorCooldownBar.color;
            barColor.a = (fillAmount >= 1f) ? 0f : 1f;
            cursorCooldownBar.color = barColor;
        }

        if (dummyCountText != null)
        {
            dummyCountText.text = "DUMMIES: " + dummiesRemaining + " / " + maxDummies;
        }
    }
}