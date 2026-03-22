using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            UIManager.instance.OpenLevelComplete();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
