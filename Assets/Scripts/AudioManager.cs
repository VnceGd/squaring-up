using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    const float MIN_VOLUME = 0;
    const float MAX_VOLUME = 100;
    const string PREF_KEY_VOLUME = "mainVolume";

    // Value between 0 (min) and 100 (max)
    public static float currentVolume { get; private set; } = MAX_VOLUME;

    public override void Awake()
    {
        base.Awake();
        currentVolume = LoadVolume();
    }

    // Set and save main game volume
    public void SetVolume(float newVolume)
    {
        // Constrain newVolume
        if (newVolume < MIN_VOLUME) newVolume = MIN_VOLUME;
        else if (newVolume > MAX_VOLUME) newVolume = MAX_VOLUME;

        currentVolume = newVolume;
        PlayerPrefs.SetFloat(PREF_KEY_VOLUME, currentVolume);
        PlayerPrefs.Save();
    }

    // Return volume as a value between 0 and 1
    public float GetVolume()
    {
        return currentVolume / MAX_VOLUME;
    }

    // Retrieve game volume from PlayerPrefs
    float LoadVolume()
    {
        if (PlayerPrefs.HasKey(PREF_KEY_VOLUME))
        {
            return PlayerPrefs.GetFloat(PREF_KEY_VOLUME);
        }
        else
        {
            return MAX_VOLUME;
        }
    }
}
