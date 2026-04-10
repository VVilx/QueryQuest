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

        previousScene = PlayerPrefs.GetString("PreviousScene", "_MainMenu");
        Debug.Log("Previous Scene: " + previousScene);
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
        SceneManager.LoadScene(previousScene);
    }
}