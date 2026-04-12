using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUIManager : MonoBehaviour
{
    [Header("Tutorial Scenes")]
    [SerializeField] private string tutorialPage1 = "Tutorial";
    [SerializeField] private string tutorialPage2 = "Tutorial 2";

    private string currentSceneName;
    private string previousScene;
    
    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        
        // Get the saved return scene
        previousScene = PlayerPrefs.GetString("TutorialReturnScene", "_MainMenu");
        
        Debug.Log($"Tutorial page: {currentSceneName}");
        Debug.Log($"Will return to: {previousScene}");
        Debug.Log($"TutorialReturnScene value in PlayerPrefs: {PlayerPrefs.GetString("TutorialReturnScene", "NOT SET")}");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToPreviousScene();
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            NextPage();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            PreviousPage();
        }
    }
    
    public void NextPage()
    {
        Time.timeScale = 1f;
        
        if (currentSceneName == tutorialPage1)
        {
            SceneManager.LoadScene(tutorialPage2);
        }
    }
    
    public void PreviousPage()
    {
        Time.timeScale = 1f;
        
        if (currentSceneName == tutorialPage2)
        {
            SceneManager.LoadScene(tutorialPage1);
        }
        else if (currentSceneName == tutorialPage1)
        {
            ReturnToPreviousScene();
        }
    }
    
    public void ReturnToPreviousScene()
    {
        Time.timeScale = 1f;
        
        // Get the return scene from PlayerPrefs
        string returnScene = PlayerPrefs.GetString("TutorialReturnScene", "_MainMenu");
        
        // Clear it so it doesn't affect next time
        PlayerPrefs.DeleteKey("TutorialReturnScene");
        PlayerPrefs.Save();
        
        Debug.Log($"Returning to: {returnScene}");
        
        SceneManager.LoadScene(returnScene);
    }
}