using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CinemachineImpulseSource))]
public class LunarStone : MonoBehaviour, IInteractable
{
    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Lunar Stone Interacted with!");
        _animator.SetTrigger("activate");
        CameraManager.Instance.CameraShake(_impulseSource);
    }
}
