using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    [TextArea(4,8)] public string noteMessage;

    private bool playerInRange = false;

    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            UIManager.instance.OpenNote(noteMessage);
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
}
