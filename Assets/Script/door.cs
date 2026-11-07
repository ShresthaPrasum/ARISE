using UnityEngine;
using UnityEngine.SceneManagement;

public class Door: MonoBehaviour
{
    public string sceneToLoad;

    public Animator doorAnimator;

    public GameObject playerSprite;

    public Animator playerAnimator;

    public Vector3 playerPositionInDoor;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            doorAnimator.SetTrigger("Open");


            playerSprite.transform.SetParent(this.transform);

            playerSprite.transform.localPosition = playerPositionInDoor;

            playerAnimator.SetTrigger("finalExit");

            Invoke("LoadScene", 1f);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}