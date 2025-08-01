using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private int _milisSize = 50;

    private DateTime _startTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan currentTimer = DateTime.Now - _startTime;
        String formattedTimer = string.Format("{0:D2}:{1:D2}.<size={2}>{3:D3}</size>",
            currentTimer.Minutes, currentTimer.Seconds, _milisSize, currentTimer.Milliseconds);
        _timerText.text = formattedTimer;
    }
}
