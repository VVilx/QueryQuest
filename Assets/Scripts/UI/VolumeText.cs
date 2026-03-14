using UnityEngine;
using TMPro;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName; 
    [SerializeField] private string textIntro;
    private TMP_Text txt;

    private void Awake()
    {
        txt = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeName) * 100; //Get the current volume value from PlayerPrefs and convert it to percentage
        txt.text = textIntro + volumeValue.ToString(); //Update the text to show the current sound volume
    }
}
