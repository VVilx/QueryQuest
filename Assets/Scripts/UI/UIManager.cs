using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen; 
    [SerializeField] private AudioClip gameOverSound; 

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen; 
    [SerializeField] private AudioClip pauseSound; 

    [Header("Quiz UI")]
    [SerializeField] private GameObject quizPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private Button[] answerButtons;

    [Header("Notes UI")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TextMeshProUGUI noteText;

    [Header("Level Complete")]
    [SerializeField] private GameObject levelCompletePanel;

    private QuizTrigger currentTrigger;
    private char correctLetter;
  
    private int selectedIndex = 0;

    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;

    public static bool quizOpen = false;

    private bool noteOpen = false;

    private void Awake()
    {
        instance = this;

        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        quizPanel.SetActive(false);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // needed to avoid closure problem
            answerButtons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }

        UpdateQuizNavigation();

       if(noteOpen && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            CloseNote();
        }
    }

    #region Game Over
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        if(status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion

    #region Quiz UI
    
    public void OpenQuiz(QuizTrigger trigger, string question, string answers, char correct)
    {
        quizPanel.SetActive(true);
        quizOpen = true;
        currentTrigger = trigger;
        correctLetter = correct;

        questionText.text = question;
        feedbackText.text = "";

        // Assign answers to buttons
        string[] splitAnswers = answers.Split('\n');
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI btnText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (i < splitAnswers.Length)
                btnText.text = splitAnswers[i];
            else
                btnText.text = "";
        }

        // Disable player movement and attack
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        if(playerMovement != null) playerMovement.enabled = false;
        if(playerAttack != null) playerAttack.enabled = false;

        Time.timeScale = 0f;

        // Start selection at first button
        selectedIndex = 0;
        UpdateButtonHighlight();
    }

    private void UpdateQuizNavigation()
    {
        if(!quizOpen) return;

        // Navigate Up
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex--;
            if(selectedIndex < 0) selectedIndex = answerButtons.Length - 1;
            UpdateButtonHighlight();
        }

        // Navigate Down
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex++;
            if(selectedIndex >= answerButtons.Length) selectedIndex = 0;
            UpdateButtonHighlight();
        }

        // Submit
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            SubmitAnswer();
        }
    }

    private void UpdateButtonHighlight()
    {
        for(int i = 0; i < answerButtons.Length; i++)
        {
            ColorBlock colors = answerButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.yellow : Color.white;
            answerButtons[i].colors = colors;
        }
    }

    private void OnButtonClick(int index)
    {
        // Optional: select button on click
        selectedIndex = index;
        UpdateButtonHighlight();

        // Optional: submit instantly on click
        // SubmitAnswer();
    }

    private void SubmitAnswer()
    {
        if (selectedIndex < 0 || selectedIndex >= answerButtons.Length)
        {
            feedbackText.text = "Select an answer first!";
            return;
        }

        char playerChoice = answerButtons[selectedIndex].GetComponentInChildren<TextMeshProUGUI>().text[0];

        if(playerChoice == correctLetter)
        {
            feedbackText.text = "Correct!";
            if(currentTrigger != null) currentTrigger.MarkComplete();

            CloseQuiz();
        }
        else
        {
            feedbackText.text = "Wrong! Try again.";
        }
    }

    private void CloseQuiz()
    {

        quizPanel.SetActive(false);
        quizOpen = false;

        if(playerMovement != null) playerMovement.enabled = true;
        if(playerAttack != null) playerAttack.enabled = true;

        Time.timeScale = 1f;

        currentTrigger = null;
    }
    #endregion

    #region Notes UI

    public void OpenNote(string text)
    {
        notePanel.SetActive(true);

        noteText.text = text;
        noteOpen = true;

        // Disable player movement and attack
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        if(playerMovement != null) playerMovement.enabled = false;
        if(playerAttack != null) playerAttack.enabled = false;

        Time.timeScale = 0f;
    }

     public void CloseNote()
    {
        notePanel.SetActive(false);
        noteOpen = false;

        if(playerMovement != null) playerMovement.enabled = true;
        if(playerAttack != null) playerAttack.enabled = true;

        Time.timeScale = 1f;
    }

    #endregion

    #region Level Complete UI

    public void OpenLevelComplete()
    {
        levelCompletePanel.SetActive(true);

        if(playerMovement != null) playerMovement.enabled = false;
        if(playerAttack != null) playerAttack.enabled = false;

        Time.timeScale = 0f;
    }

    // Save game option
    public void SaveProgress()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if(PlayerPrefs.GetInt("UnlockedLevel", 0) < currentLevel)
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel);

        PlayerPrefs.Save();
    }

    // Go to the next level
    public void NextLevel()
    {
        Time.timeScale = 1f;


        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);

        if(playerMovement != null) playerMovement.enabled = true;
        if(playerAttack != null) playerAttack.enabled = true;
    }

    #endregion
}