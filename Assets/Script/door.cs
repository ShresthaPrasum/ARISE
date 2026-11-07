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
            
            var grandFather = other.transform.parent.parent;

            other.transform.parent.SetParent(this.transform);

            other.transform.parent.localPosition = new Vector3(-5.24f, -2.35f, -2f);

            playerAnimator.SetTrigger("finalExit");

            Invoke("LoadScene", 1f);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}