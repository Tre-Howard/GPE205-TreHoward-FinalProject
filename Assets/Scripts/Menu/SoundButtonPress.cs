using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundButtonPress : MonoBehaviour
{
    public AudioMixer mainAudioMixer;
    public Slider masterVolumeSlider;
    public Slider soundVolumeSlider;
    public Slider musicVolumeSlider;

    public void Start()
    {
        SetVolumeOnStart();
    }

    public void SetVolumeOnStart()
    {
        // start with slider (0-1)
        float newVolume = masterVolumeSlider.value;
        if (newVolume <= 0)
        {
            // if zero, set volume to lowest value
            newVolume = -80;
        }
        else
        {
            // if > 0, find log10 value
            newVolume = Mathf.Log10(newVolume);

            // put newVolume in a 0-20 instead of a 0-1 range
            newVolume = newVolume * 20;
        }

        // set volume
        mainAudioMixer.SetFloat("masterVolume", newVolume);
    }

    public void UpdateMasterVolume()
    {
        // convert slider value from 0-1 to -80 to 0 using log10 - this also checks if its 0 and automatically changes it to -80f
        float newVolume = masterVolumeSlider.value > 0 ? Mathf.Log10(masterVolumeSlider.value) * 20 : -80f;
        // i was having issues with 0 being a "negative infinity" so when i put the slider to 0 it would go back to max volume
        // this fixes it

        // set volume based on slider value
        mainAudioMixer.SetFloat("masterVolume", newVolume);

    }

    public void UpdateSoundVolume()
    {
        // convert slider value from 0-1 to -80 to 0 using log10 - this also checks if it's 0 and automatically changes it to -80f
        float soundVolume = soundVolumeSlider.value > 0 ? Mathf.Log10(soundVolumeSlider.value) * 20 : -80f;

        // convert master volume slider value from 0-1 to -80 to 0 using log10
        float masterVolume = Mathf.Log10(masterVolumeSlider.value) * 20;

        // calculate the final music volume by adding the music volume to the master volume
        float finalMusicVolume = soundVolume + masterVolume;

        // set volume based on slider value
        mainAudioMixer.SetFloat("soundVolume", finalMusicVolume);
    }

    public void UpdateMusicVolume()
    {
        // convert slider value from 0-1 to -80 to 0 using log10 - this also checks if it's 0 and automatically changes it to -80f
        float musicVolume = musicVolumeSlider.value > 0 ? Mathf.Log10(musicVolumeSlider.value) * 20 : -80f;

        // convert master volume slider value from 0-1 to -80 to 0 using log10
        float masterVolume = Mathf.Log10(masterVolumeSlider.value) * 20;

        // calculate the final music volume by adding the music volume to the master volume
        float finalMusicVolume = musicVolume + masterVolume;

        // set volume based on slider value
        mainAudioMixer.SetFloat("musicVolume", finalMusicVolume);
    }

}
