using System;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public static Moon Instance { get; private set; }

    [SerializeField] private float _phaseDuration = 4.0f;
    [SerializeField] private int _maxPhases;
    [SerializeField] private PlatformManager _platformManager;
    [SerializeField] private bool cycle;

    private int _currentPhase;
    private float _phaseTimer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentPhase = 0;
        _phaseTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cycle) return;

        _phaseTimer += Time.deltaTime;
        if (_phaseTimer >= _phaseDuration)
        {
            _phaseTimer = 0.0f;
            _currentPhase = (_currentPhase + 1) % _maxPhases;
            NextPhase();
        }
    }

    public void NextPhase()
    {
        _platformManager.NextGroup();
        _currentPhase++;
    }
}
