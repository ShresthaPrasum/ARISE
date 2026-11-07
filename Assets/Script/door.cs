using UnityEngine;
using UnityEngine.SceneManagement;

public class Door: MonoBehaviour
{
    public string sceneToLoad;

    public Animator doorAnimator;

    public Animator playerAnimator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            doorAnimator.SetTrigger("Open");

            other.transform.GetChild(0).SetParent(this.transform);

            other.transform.GetChild(0).localPosition = Vector3.zero;

            playerAnimator.SetTrigger("finalExit");

            Invoke("LoadScene", 1f);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}