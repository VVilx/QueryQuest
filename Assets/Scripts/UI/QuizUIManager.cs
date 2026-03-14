using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class QuizUIManager : MonoBehaviour
{
    public static QuizUIManager instance;

    [Header("UI")]
    public GameObject panel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answersText;
    public TMP_InputField answerInput;
    public TextMeshProUGUI feedbackText;

    private QuizTrigger currentTrigger;
    private char correctLetter;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    // Method to open the quiz UI with the given question and answer
    public void OpenQuiz(string question, string[] answers, int correctIndex, QuizTrigger trigger)
    {
        currentTrigger = trigger;

        questionText.text = question;

        // Format answers with letters
        answersText.text =
        "A) " + answers[0] + "\n" +
        "B) " + answers[1] + "\n" +
        "C) " + answers[2] + "\n" +
        "D) " + answers[3];

        correctLetter = (char)('A' + correctIndex); 

        answerInput.text = "";
        feedbackText.text = "";

        panel.SetActive(true);

        answerInput.text = "";
        answerInput.Select();
        answerInput.ActivateInputField();

        // Disable player movement
        playerMovement = FindObjectOfType<PlayerMovement>();
        if(playerMovement != null)
            playerMovement.enabled = false;

        // Disable player attack
        if(playerAttack != null)
            playerAttack.enabled = false;

        Time.timeScale = 0f; // Pause the game
    }

    public void SubmitAnswer()
    {
        if(string.IsNullOrEmpty(answerInput.text))
            return;

        char playerAnswer = char.ToUpper(answerInput.text[0]);

        if(playerAnswer == correctLetter)
        {
            feedbackText.text = "Correct!";
            currentTrigger.MarkComplete();
            CloseQuiz();
        }
        else
        {
            feedbackText.text = "Wrong! Try again.";
        }
    }

    private void CloseQuiz()
    {
        panel.SetActive(false);
    
        if(playerMovement != null)
            playerMovement.enabled = true;

        if(playerAttack != null)
            playerAttack.enabled = true;

        Time.timeScale = 1f; // Resume the game
        
    }
}
