using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public string Level1 ="Home";


    public void PlayLatestlevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
