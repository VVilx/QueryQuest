using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUIManager : MonoBehaviour
{
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f; // VERY important (prevents freezing)
        SceneManager.LoadScene(0); // Main Menu
    }
}
