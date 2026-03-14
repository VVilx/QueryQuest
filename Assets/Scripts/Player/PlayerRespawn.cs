using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound; //Sound that we'll play when picking up a new checkpoint
    private Transform currentCheckpoint; //The point where the player will respawn
    private Health playerHealth; //Reference to the player's health script
    private UIManager uiManager; //Reference to the UI manager

    private void Awake()
    {
        playerHealth = GetComponent<Health>(); //Get the player's health component
        uiManager = FindObjectOfType<UIManager>(); //Find the UI manager in the scene
    }

    public void CheckRespawn()
    {
        //Check if checkpoint is available
        if (currentCheckpoint == null)
        {
            //Show game over screen
            uiManager.GameOver();

            return; //Do nothing
        }
        transform.position = currentCheckpoint.position; //Move the player to the current checkpoint's position
        playerHealth.Respawn(); //Restore player health and reset animation
        
        //Move camera to checkpoint
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //Activate checkpoints
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform; //Set the current checkpoint to the one we just collided with
            SoundManager.instance.PlaySound(checkpointSound); //Play checkpoint sound
            collision.GetComponent<Collider2D>().enabled = false; //Disable the checkpoint's collider
            collision.GetComponent<Animator>().SetTrigger("appear"); //Play checkpoint activation animation
        }
    }
}
