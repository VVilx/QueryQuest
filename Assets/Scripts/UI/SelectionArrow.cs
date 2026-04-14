using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
   [SerializeField] private AudioClip changeSound;
   [SerializeField] private AudioClip interactSound;

    private RectTransform rect;
    private int currentPosition;
    private List<RectTransform> activeOptions;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        activeOptions = new List<RectTransform>();
    }

    private void Start()
    {
        UpdateActiveOptions();
    }

    private void Update()
    {
        UpdateActiveOptions();

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

        if(activeOptions.Count == 0) return; //If there are no active options, do nothing

        currentPosition += _change; //Change the current position by the change value

        if(_change !=0)
            SoundManager.instance.PlaySound(changeSound); 

        if (currentPosition < 0) 
            currentPosition = activeOptions.Count - 1; //If we go below the first option, loop back to the last option

        else if (currentPosition >= activeOptions.Count) 
            currentPosition = 0; //If we go above the last option, loop back to the first option
        
        rect.position = new Vector3(rect.position.x, activeOptions[currentPosition].position.y, 0); //Move the arrow to the new position
    }

    private void UpdateActiveOptions()
    {
        if(options == null) return;

        activeOptions.Clear(); //Clear the list of active options

        foreach (RectTransform option in options)
        {
            if (option != null && option.gameObject.activeInHierarchy)
                activeOptions.Add(option);
        }

        if (currentPosition >= activeOptions.Count)
        {
            currentPosition = activeOptions.Count > 0 ? 0 : -1; //Reset position if it exceeds the number of active options
        }

        if(activeOptions.Count > 0 && currentPosition >= 0 && currentPosition < activeOptions.Count)
        {
            rect.position = new Vector3(rect.position.x, activeOptions[currentPosition].position.y, 0); //Move the arrow to the new position
        }
    }

    private void Interact()
    {
        if(activeOptions == null || activeOptions.Count == 0 || currentPosition < 0 || currentPosition >= activeOptions.Count)
        { 
            return; //If there are no active options, do nothing
        }

        SoundManager.instance.PlaySound(interactSound); //Play interact sound when selecting an option

        //Access the button component of the current option and invoke its onClick event
        activeOptions[currentPosition].GetComponent<Button>().onClick.Invoke();

    }
}
