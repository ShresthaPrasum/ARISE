using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public Animator playerAnimator;

    public Transform indicator;
    public SpriteRenderer indicatorRenderer;

    public Image cursorCoolDownBarBg;
    public Image cursorCooldownBar;
    public List<Image> dummyIcons = new List<Image>();

    public Color validColor = Color.white;
    public Color invalidColor = new Color(0.3f, 0.3f, 0.3f, 0f);

    private List<GameObject> activeDummies = new List<GameObject>();
    private Vector2 spawnPosition;
    private bool canSummonHere;
    private Camera mainCam;
    private PlayerMovement player;
    private float cooldownTimer = 0f;
    private int dummiesRemaining;

    public Menu PauseMenu;



    private void Start()
    {
        mainCam = Camera.main;
        player = GetComponent<PlayerMovement>();
        dummiesRemaining = maxDummies;
    }

    private void Update()
    {
        
        if (player.isDead || PauseMenu.GameIsPaused) return;
        
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;

        CheckMousePosition();
        HandleInput();
        UpdateCursorUI();   
    }

    private void CheckMousePosition()
    {
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Vector2.Distance(transform.position, mouseWorldPos) > summonRange)
        {
            canSummonHere = false;
            ToggleIndicatorVisibility(false); 
            return;
        }

        Vector2 rayStart = new Vector2(mouseWorldPos.x, mouseWorldPos.y + 1f);
        RaycastHit2D groundHit = Physics2D.Raycast(rayStart, Vector2.down, 2f, groundLayer);

        if (groundHit.collider != null)
        {
            spawnPosition = groundHit.point + new Vector2(0f, checkBoxSize.y * 0.5f);
            
            
            canSummonHere = !Physics2D.OverlapBox(spawnPosition, checkBoxSize * 0.9f, 0f, groundLayer);
            
            ToggleIndicatorVisibility(true); 
            UpdateIndicator(spawnPosition, canSummonHere);
        }
        else
        {
            canSummonHere = false;
            ToggleIndicatorVisibility(false); 
        }
    }

    private void ToggleIndicatorVisibility(bool isVisible)
    {
        indicatorRenderer.enabled = isVisible;
        cursorCoolDownBarBg.enabled = isVisible;
        cursorCooldownBar.enabled = isVisible;
    }

    private void UpdateIndicator(Vector2 position, bool isValid)
    {
        indicator.position = position;
        bool readyToSpawn = (cooldownTimer <= 0f) && (dummiesRemaining > 0);

        for (int i = 0; i < dummyIcons.Count; i++)
        {
            
            dummyIcons[i].color = (i < dummiesRemaining) ? Color.white : new Color(1f, 1f, 1f, 0.38f);   
        }

        indicatorRenderer.color = (isValid && readyToSpawn) ? validColor : invalidColor;
    }

    private void HandleInput()
    {
        
        if (Mouse.current.leftButton.wasPressedThisFrame && canSummonHere && cooldownTimer <= 0f && dummiesRemaining > 0)
        {
            SummonSkeleton();
            cooldownTimer = summonCooldown;
        }

        if (Keyboard.current.rKey.wasPressedThisFrame) {
            ClearAllDummies();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame) {
            RemoveLastDummy();
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void SummonSkeleton()
    {
        audioSource.PlayOneShot(ariseSound);
        
        GameObject newDummy = Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity);
        activeDummies.Add(newDummy);
        dummiesRemaining--;

    }

    public void RemoveLastDummy()
    {
        if (activeDummies.Count > 0)
        {
            int lastIndex = activeDummies.Count - 1;
            Destroy(activeDummies[lastIndex]);
            activeDummies.RemoveAt(lastIndex);
        }
    }
    public void ClearAllDummies()
    {
        foreach (GameObject dummy in activeDummies)
        {
            Destroy(dummy);
        }
        activeDummies.Clear();
    }
    private void UpdateCursorUI()
    {
        
        cursorCooldownBar.fillAmount = 1f - (cooldownTimer / summonCooldown);
    }
}