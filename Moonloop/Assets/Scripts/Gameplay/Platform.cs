using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour
{
    public PlatformType type;
    public bool unstable = false;
    [SerializeField] private float _breakingTime = 1.0f;
    private bool _broken = false;

    private int _dissolveAmountId = Shader.PropertyToID("_Amount");

    private TilemapRenderer _tilemapRenderer;
    private Collider2D _collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public void Break()
    {
        if (!_broken)
            StartCoroutine(BreakCo());
    }

    IEnumerator BreakCo()
    {
        _broken = true;
        StartCoroutine(DissolveCo(1f));
        yield return new WaitForSeconds(_breakingTime);
        StartCoroutine(DissolveCo(0f));
        _broken = false;
    }

    IEnumerator DissolveCo(float to)
    {
        float elapsedTime = 0f;
        float baseDissolve = _tilemapRenderer.material.GetFloat(_dissolveAmountId);
        while (elapsedTime < _breakingTime)
        {
            if (elapsedTime > _breakingTime / 2) _collider.isTrigger = !_collider.isTrigger;
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
