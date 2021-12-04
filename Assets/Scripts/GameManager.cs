using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    static TimeKeeper timeKeeper;

    public static int currentLevel { get; private set; }

    public override void Awake()
    {
        base.Awake();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        StartLevel();
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
        currentLevel = level;
    }

    void StartLevel()
    {
        if (currentLevel > 0)
        {
            timeKeeper = FindObjectOfType<TimeKeeper>();
            timeKeeper.StartTimer();
            timeKeeper.SetPB(GetRecord(currentLevel));
        }
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevel);
    }

    public void CompleteLevel()
    {
        if (currentLevel > 0)
        {
            timeKeeper.StopTimer();
            timeKeeper.SetPB(SetRecord(currentLevel, timeKeeper.levelTimer));
        }
    }

    float SetRecord(int level, float time)
    {
        float pb = GetRecord(level);
        if (pb > time || pb <= 0)
        {
            PlayerPrefs.SetFloat("pb_" + level.ToString(), time);
            PlayerPrefs.Save();
            return time;
        }
        return pb;
    }

    public float GetRecord(int level)
    {
        string levelKey = "pb_" + level.ToString();
        if (PlayerPrefs.HasKey(levelKey))
        {
            return PlayerPrefs.GetFloat(levelKey);
        }
        else
        {
            return 0;
        }
    }

    void EraseRecord(int level)
    {
        string levelKey = "pb_" + level.ToString();
        if (PlayerPrefs.HasKey(levelKey))
        {
            PlayerPrefs.DeleteKey(levelKey);
        }
    }

    public void EraseAll() {
        for (int s = 1; s < SceneManager.sceneCountInBuildSettings; s++)
        {
            EraseRecord(s);
        }
    }
}
