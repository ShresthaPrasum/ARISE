using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public string levelScene = "Levels";
    

    public void PlayLatestlevel()
    {
        int latestLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        SceneManager.LoadScene(latestLevel.ToString());
    }


    public void LevelScene()
    {
        SceneManager.LoadScene(levelScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
