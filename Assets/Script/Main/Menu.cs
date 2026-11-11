using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public string homeSceneName ="Home";

    public bool GameIsPaused = false;


    private void Start()
    {
        if (pauseMenuUI == null)
        {
            return;
        }
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Update()
    {
        if(Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame && pauseMenuUI != null)
        {
            if (GameIsPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }

        else
        {
            return;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;

        GameIsPaused=  false;
    }

    private void Pause()
    {
       pauseMenuUI.SetActive(true);

       Time.timeScale = 0f;

       GameIsPaused = true; 
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        GameIsPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadHome()
    {
        Time.timeScale =1f;

        GameIsPaused = false;

        SceneManager.LoadScene(homeSceneName);
    }

}
