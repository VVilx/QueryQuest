using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [SerializeField] private QuizTrigger quiz;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (quiz != null && !quiz.completed)
            {
                if (collision.transform.position.x < transform.position.x)
                    collision.transform.position = new Vector3(transform.position.x - 1f, collision.transform.position.y, collision.transform.position.z);
                else
                    collision.transform.position = new Vector3(transform.position.x + 1f, collision.transform.position.y, collision.transform.position.z);
                    
                Debug.Log("Finish the quiz before entering!");
                return;    
            }

            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().ActivateRoom(false);
            } else
            {
                cam.MoveToNewRoom(previousRoom);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
            }
        }
    }

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }
}
