using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CinemachineImpulseSource))]
public class LunarStone : MonoBehaviour, IInteractable
{
    private bool _activated;

    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _activated = false;

        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(Transform player)
    {
        if (_activated) return;
        player.GetComponent<PlayerDeathController>().SetCheckpoint(transform);
        _animator.SetTrigger("activate");
        CameraManager.Instance.CameraShake(_impulseSource);
        _activated = true;
    }
}
