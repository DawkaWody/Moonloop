using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Light2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CinemachineImpulseSource))]
[RequireComponent(typeof(AudioSource))]
public class LunarStone : MonoBehaviour, IInteractable
{
    [SerializeField] private float _activatedLightIntensity = 2f;
    [SerializeField] private float _lightIntensityLerpTime = 0.5f;
    [ColorUsage(true, true)]
    [SerializeField] private Color _activatedGlow;
    [SerializeField] private CameraShakeProfileSO _shakeProfile;
    [SerializeField] private MoonLight _moonLight;

    private bool _activated;

    private Animator _animator;
    private Light2D _light;
    private SpriteRenderer _spriteRenderer;
    private CinemachineImpulseSource _impulseSource;
    private AudioSource _audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _activated = false;

        _animator = GetComponent<Animator>();
        _light = GetComponent<Light2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Interact(Transform player)
    {
        if (_activated) return;
        player.GetComponent<PlayerMovementController>().Freeze();
        Timer.Instance.Stop();
        _animator.SetTrigger("activate");
        StartCoroutine(LerpLightIntensityCo());
        _spriteRenderer.material.SetColor("_Color", _activatedGlow);
        _audioSource.Play();
        CameraManager.Instance.CameraShake(_impulseSource, _shakeProfile);
        _activated = true;
        player.GetComponent<PlayerFxController>().Dissolve();
        _moonLight.cycle = false;
        StartCoroutine(EndLevelCo());
    }

    IEnumerator LerpLightIntensityCo()
    {
        float baseIntensity = _light.intensity;
        float lerpTimer = 0.0f;
        while (lerpTimer < _lightIntensityLerpTime)
        {
            yield return new WaitForEndOfFrame();
            lerpTimer += Time.deltaTime;
            _light.intensity = Mathf.Lerp(baseIntensity, _activatedLightIntensity, lerpTimer / _lightIntensityLerpTime);
        }
    }

    IEnumerator EndLevelCo()
    {
        yield return new WaitForSeconds(GameManager.Instance.endLevelDelay);
        GameManager.Instance.EndLevel();
    }
}
