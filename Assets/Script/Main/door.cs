using UnityEngine;
using UnityEngine.SceneManagement;

public class Door: MonoBehaviour
{
    public string sceneToLoad;

    private int currentLevelNumber;

    public Animator doorAnimator;

    public GameObject playerSprite;

    public Animator playerAnimator;

    public float delayBeforeLoad = 1f;

    public Vector3 playerPositionInDoor;

    public AudioSource audioSource;

    public AudioClip doorOpenSound;

    public void Start()
    {
        currentLevelNumber = int.Parse(sceneToLoad) -1;
        
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.bodyType = RigidbodyType2D.Static;
            }

            doorAnimator.SetTrigger("Open");


            playerSprite.transform.SetParent(this.transform);

            playerSprite.transform.localPosition = playerPositionInDoor;

            playerAnimator.SetTrigger("finalExit");

            audioSource.PlayOneShot(doorOpenSound);

            Invoke("LoadScene", delayBeforeLoad);

            int currentUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

    
            if (currentLevelNumber >= currentUnlocked)
            {
                PlayerPrefs.SetInt("UnlockedLevel", currentLevelNumber + 1);
                PlayerPrefs.Save(); 
            }
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void PlayDoorOpenSound()
    {
            audioSource.PlayOneShot(doorOpenSound);
    
    }
}