using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeKeeper : MonoBehaviour
{
    bool isPlaying;
    public float levelTimer { get; private set; }
    
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI pbTimeText;
    public GameObject nextLevelPanel;

    void Update()
    {
        if (isPlaying)
        {
            levelTimer += Time.deltaTime;
            timerText.text = DisplayTime(levelTimer);
        }
    }

    public void StartTimer()
    {
        isPlaying = true;
    }

    public void StopTimer()
    {
        isPlaying = false;
        timerText.color = Color.green;
        nextLevelPanel.gameObject.SetActive(true);
    }

    public void SetPB(float pb)
    {
        if (pb > 0)
        {
            pbTimeText.text = DisplayTime(pb);
        }
        else
        {
            pbTimeText.text = "--:--";
        }
    }

    // Assumes time is given in seconds and formats it as mm:ss
    public string DisplayTime(float time)
    {
        string display = "";
        int minutes = Mathf.FloorToInt(time / 60);
        if (minutes > 0)
        {
            display += minutes.ToString("D2") + ":";
        }
        float seconds = time - (minutes * 60);
        if (seconds < 10)
        {
            display += "0";
        }
        display += seconds.ToString("F2");
        return display;
    }
}
