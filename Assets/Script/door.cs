using UnityEngine;
using UnityEngine.SceneManagement;

public class Door: MonoBehaviour
{
    public string sceneToLoad;

    public Animator doorAnimator;

    public GameObject playerSprite;

    public Animator playerAnimator;

    public float delayBeforeLoad = 1f;

    public Vector3 playerPositionInDoor;

    public AudioSource audioSource;

    public AudioClip doorCloseSound;

    public AudioClip doorOpenSound;

    public void Start()
    {
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            doorAnimator.SetTrigger("Open");


            playerSprite.transform.SetParent(this.transform);

            playerSprite.transform.localPosition = playerPositionInDoor;

            playerAnimator.SetTrigger("finalExit");

            audioSource.PlayOneShot(doorOpenSound);

            Invoke("LoadScene", delayBeforeLoad);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void PlayDoorCloseSound()
    {
            audioSource.PlayOneShot(doorCloseSound);
    
    }
    public void PlayDoorOpenSound()
    {
            audioSource.PlayOneShot(doorOpenSound);
    
    }
}