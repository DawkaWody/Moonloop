using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private int _milisSize = 50;

    private bool _run;
    private DateTime _startTime;
    private TimeSpan _currentTimer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startTime = DateTime.Now;
        _run = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_run) return;
        _currentTimer = DateTime.Now - _startTime;
        String formattedTimer = string.Format("{0:D2}:{1:D2}.<size={2}>{3:D3}</size>",
            _currentTimer.Minutes, _currentTimer.Seconds, _milisSize, _currentTimer.Milliseconds);
        _timerText.text = formattedTimer;
    }

    public TimeSpan GetTime()
    {
        return _currentTimer;
    }

    public void Stop()
    {
        _run = false;
    }
}
