using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    [Header("Quiz Data")]
    [TextArea(2,4)] public string question;
    [TextArea(4,8)] public string answers;
    public char correctAnswer;

    private bool playerInRange = false;
    public bool completed = false;

    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E) && !completed)
        {
            UIManager.instance.OpenQuiz(this, question, answers, correctAnswer);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            playerInRange = false;
    }

    public void MarkComplete()
    {
        completed = true;
        GetComponent<Collider2D>().enabled = false; // prevent retake
    }
}