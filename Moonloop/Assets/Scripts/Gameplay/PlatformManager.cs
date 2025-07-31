using System;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private Transform[] platformGroups;
    private int _currentGroup = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = _currentGroup + 1; i < platformGroups.Length; i++)
        {
            platformGroups[i].gameObject.SetActive(false);
        }
    }

    public void NextGroup()
    {
        platformGroups[_currentGroup].gameObject.SetActive(false);
        _currentGroup++;
        if (_currentGroup >= platformGroups.Length) _currentGroup = 0;
        platformGroups[_currentGroup].gameObject.SetActive(true);
    }

    public void PreviousGroup()
    {
        platformGroups[_currentGroup].gameObject.SetActive(false);
        _currentGroup--;
        if (_currentGroup < 0) _currentGroup = platformGroups.Length - 1;
        platformGroups[_currentGroup].gameObject.SetActive(true);
    }
}
