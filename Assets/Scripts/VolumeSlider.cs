using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    // Slider component
    Slider volumeSlider;
    
    // UI text for displaying slider value
    public TextMeshProUGUI volumeValue;

    void Awake()
    {
        // Assign slider component on Awake
        volumeSlider = GetComponent<Slider>();
    }

    void Start()
    {
        // Retrieve volume from AudioManager
        volumeSlider.value = AudioManager.currentVolume;
        UpdateText();
    }

    public void UpdateVolume()
    {
        // Set 
        AudioManager.Instance.SetVolume(volumeSlider.value);
        UpdateText();
    }

    void UpdateText()
    {
        volumeValue.text = volumeSlider.value.ToString();
    }
}
