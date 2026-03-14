using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake() {
        
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        //Keep this object even when we go to new scene
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Destroy duplicate
        else if(instance != null && instance != this) {
            Destroy(gameObject);
        }

        //Assign initial volumes
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }
    public void PlaySound(AudioClip _sound) {
        soundSource.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(1, "soundVolume", _change, soundSource);
    }

    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(1, "musicVolume", _change, musicSource);
    }

    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        //Clamp the volume between 0 and 1
        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        //Assign the new volume to the audio source
        float finalVolume = currentVolume *= baseVolume;
        source.volume = finalVolume;

        //Save the new volume level to PlayerPrefs
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }

}
