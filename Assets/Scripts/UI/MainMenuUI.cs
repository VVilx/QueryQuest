using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private const string UNLOCK_KEY = "UnlockedLevel";
    
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void NewGame()
    {
        // Reset unlocked levels
        PlayerPrefs.SetInt(UNLOCK_KEY, 3); // Unlock the first level
        PlayerPrefs.Save();

        SceneManager.LoadScene(3); // Load the first level
    }

    public void ContinueGame()
    {
        int level = PlayerPrefs.GetInt(UNLOCK_KEY, 3);
        SceneManager.LoadScene(level);
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene(1); // Load the tutorial scene
    }
}
