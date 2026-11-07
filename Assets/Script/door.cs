using UnityEngine;
using UnityEngine.SceneManagement;

public class Door: MonoBehaviour
{
    public string sceneToLoad;

    public Animator doorAnimator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            doorAnimator.SetTrigger("Open");
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}