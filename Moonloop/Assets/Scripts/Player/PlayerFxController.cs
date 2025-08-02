using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CinemachineImpulseSource))]
public class PlayerFxController : MonoBehaviour
{
    [SerializeField] private float _dissolveTime = 0.75f;
    [SerializeField] private CameraShakeProfileSO _deathShakeProfile;

    private int _dissolveAmountId = Shader.PropertyToID("_Amount");

    private SpriteRenderer _spriteRenderer;
    private CinemachineImpulseSource _impulseSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();

        _spriteRenderer.material.SetFloat(_dissolveAmountId, 1f);
        Respawn();
    }

    public void Shake()
    {
        CameraManager.Instance.CameraShake(_impulseSource, _deathShakeProfile);
    }

    public void Respawn()
    {
        StartCoroutine(DissolveCo(0f));
    }

    public void Dissolve()
    {
        StartCoroutine(DissolveCo(1f));
    }

    IEnumerator DissolveCo(float to)
    {
        float elapsedTime = 0f;
        float baseDissolve = _spriteRenderer.material.GetFloat(_dissolveAmountId);
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;
            _spriteRenderer.material.SetFloat(_dissolveAmountId, Mathf.Lerp(baseDissolve, to, (elapsedTime / _dissolveTime)));
            yield return null;
        }
    }
}
