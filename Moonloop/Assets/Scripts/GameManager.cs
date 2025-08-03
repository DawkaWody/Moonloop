using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string[] _levels;
    [SerializeField] private string _mainMenu;
    [SerializeField] private MusicManager _musicManager;
    public float endLevelDelay = 2.0f;
    public TimeSpan[] levelTimes;
    public bool fadeMenu;

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
        fadeMenu = false;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public bool IsLevelUnlocked(int levelIndex)
    {
        return true;
    }

    public void UnlockLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < _levels.Length)
        {
            PlayerPrefs.SetInt("Level_" + levelIndex, 1);
            PlayerPrefs.Save();
        }
    }

    public void StartLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= _levels.Length)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }
        fadeMenu = false;
        _currentLevel = levelIndex;
        StartCoroutine(LoadLevelFade(_levels[_currentLevel]));
    }

    public void EndLevel()
    {
        TimeSpan levelTime = Timer.Instance.GetTime();
        if (levelTimes[_currentLevel] == TimeSpan.Zero || levelTime.TotalMilliseconds < levelTimes[_currentLevel].TotalMilliseconds)
            levelTimes[_currentLevel] = levelTime;
        fadeMenu = true;
        StartCoroutine(LoadLevelFade(_mainMenu));
    }

    IEnumerator LoadLevelFade(string level)
    {
        Crossfade.Instance.FadeOut();
        _musicManager.LerpVolume(Crossfade.Instance.fadeDuration, 0f);
        yield return new WaitForSeconds(Crossfade.Instance.fadeDuration);
        SceneManager.LoadScene(level);
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == _mainMenu) _musicManager.PlayMenuMusic();
        else _musicManager.PlayGameMusic();


        _musicManager.LerpVolume(Crossfade.Instance.fadeDuration, 1f);
    }
}
