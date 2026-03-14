using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
   [SerializeField] private AudioClip changeSound;
   [SerializeField] private AudioClip interactSound;

    private RectTransform rect;
    private int currentPosition;
    
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        //Change position of the selection arrow
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
            ChangePosition(-1); //Move up the menu
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
            ChangePosition(1); //Move down the menu
        
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E)) 
            Interact();
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change; //Change the current position by the change value

        if(_change !=0)
            SoundManager.instance.PlaySound(changeSound); 
        if (currentPosition < 0) 
            currentPosition = options.Length - 1; //If we go below the first option, loop back to the last option
        else if (currentPosition >= options.Length) 
            currentPosition = 0; //If we go above the last option, loop back to the first option
        
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0); //Move the arrow to the new position
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound); //Play interact sound when selecting an option

        //Access the button component of the current option and invoke its onClick event
        options[currentPosition].GetComponent<Button>().onClick.Invoke();

    }
}
