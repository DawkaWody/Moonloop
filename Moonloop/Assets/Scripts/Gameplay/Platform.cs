using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ShadowCaster2D))]
[RequireComponent(typeof(AudioSource))]
public class Platform : MonoBehaviour
{
    public PlatformType type;
    public bool unstable;
    public bool isActive;
    [SerializeField] private float _breakingTime = 1.0f;

    private int _dissolveAmountId = Shader.PropertyToID("_Amount");
    private bool _broken = false;

    private TilemapRenderer _tilemapRenderer;
    private Collider2D _collider;
    private ShadowCaster2D _shadowCaster;
    private AudioSource _audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        _collider = GetComponent<Collider2D>();
        _shadowCaster = GetComponent<ShadowCaster2D>();
        _audioSource = GetComponent<AudioSource>();

        unstable = false;
        isActive = true;
    }

    public void Break()
    {
        if (!_broken && isActive && unstable)
            StartCoroutine(BreakCo());
    }

    IEnumerator BreakCo()
    {
        _broken = true;
        _audioSource.Play();
        StartCoroutine(DissolveCo(1f));
        yield return new WaitForSeconds(_breakingTime * 2);
        StartCoroutine(DissolveCo(0f));
        _broken = false;
    }

    IEnumerator DissolveCo(float to)
    {
        float elapsedTime = 0f;
        float baseDissolve = _tilemapRenderer.material.GetFloat(_dissolveAmountId);
        while (elapsedTime < _breakingTime)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > _breakingTime / 2)
            {
                _collider.isTrigger = to != 0f;
                _shadowCaster.enabled = to == 0f;
            }
            _tilemapRenderer.material.SetFloat(_dissolveAmountId, Mathf.Lerp(baseDissolve, to, (elapsedTime / _breakingTime)));
            yield return null;
        }
    }
}

public enum PlatformType
{
    Light,
    Dark
}
