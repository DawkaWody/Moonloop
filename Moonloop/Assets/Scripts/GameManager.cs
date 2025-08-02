using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string[] _levels;
    [SerializeField] private string _mainMenu;
    public float endLevelDelay = 2.0f;
    public TimeSpan[] levelTimes;

    private int _currentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
            Destroy(gameObject);
    }

    private void Init()
    {
        levelTimes = new TimeSpan[_levels.Length];
        _currentLevel = 0;
    }

    public void StartLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= _levels.Length)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }

        _currentLevel = levelIndex;
        SceneManager.LoadScene(_levels[levelIndex]);
    }

    public void EndLevel()
    {
        TimeSpan levelTime = Timer.Instance.GetTime();
        if (levelTimes[_currentLevel] == TimeSpan.Zero || levelTime.TotalMilliseconds < levelTimes[_currentLevel].TotalMilliseconds)
            levelTimes[_currentLevel] = levelTime;
        Debug.Log($"Level finished in {levelTimes[_currentLevel]}. Back to menu...");
    }
}
