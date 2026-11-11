using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class LevelScene : MonoBehaviour
{
    public int levelNumber;

    private Button myButton;
    


    private void Start()
    {
        myButton = GetComponent<Button>();

        int maxUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();

        if(levelNumber > maxUnlocked)
        {
            myButton.interactable = false;
            
            buttonText.color = Color.gray * 0.8f;
        }

        else
        {
            myButton.interactable = true;
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelNumber.ToString());
        
    }
}