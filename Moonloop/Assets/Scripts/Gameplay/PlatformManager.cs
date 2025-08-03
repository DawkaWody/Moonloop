using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private Platform[] _platforms;
    [SerializeField] private float _alphaOnInactive = 0.2f;
    [SerializeField] private float _alphaLerpTime = 0.3f;

    private Transform[] _platformTransforms;
    private int _activePlatformLayer;
    private int _inactivePlatformLayer;

    public Platform[] AllPlatforms => _platforms;

    void Awake()
    {
        _activePlatformLayer = LayerMask.NameToLayer("Ground");
        _inactivePlatformLayer = LayerMask.NameToLayer("PlatformInactive");
    }

    void Start()
    {
        _platformTransforms = new Transform[_platforms.Length];

        for (int i = 0; i < _platforms.Length; i++)
        {
            _platformTransforms[i] = _platforms[i].transform;
            Deactivate(_platformTransforms[i]);
        }
    }

    public void Activate(Transform platform)
    {
        platform.gameObject.layer = _activePlatformLayer;

        var shadow = platform.GetComponent<ShadowCaster2D>();
        if (shadow != null) shadow.enabled = true;

        var platformComponent = platform.GetComponent<Platform>();
        if (platformComponent != null)
        {
            platformComponent.isActive = true;

            if (platformComponent.type == PlatformType.Dark)
            {
                var light = platform.GetComponentInChildren<Light2D>();
                if (light != null) light.enabled = true;
            }
        }

        StartCoroutine(LerpAlphaCo(platform.GetComponent<Tilemap>(), 1.0f));
    }

    public void Deactivate(Transform platform)
    {
        platform.gameObject.layer = _inactivePlatformLayer;

        var shadow = platform.GetComponent<ShadowCaster2D>();
        if (shadow != null) shadow.enabled = false;

        var platformComponent = platform.GetComponent<Platform>();
        if (platformComponent != null)
        {
            platformComponent.isActive = false;

            if (platformComponent.type == PlatformType.Dark)
            {
                var light = platform.GetComponentInChildren<Light2D>();
                if (light != null) light.enabled = false;
            }
        }
        StartCoroutine(LerpAlphaCo(platform.GetComponent<Tilemap>(), _alphaOnInactive));
    }

    public void SetPlatformState(Platform platform, bool active)
    {
        if (active && !platform.isActive)
            Activate(platform.transform);
        else if (!active && platform.isActive)
            Deactivate(platform.transform);
    }

    IEnumerator LerpAlphaCo(Tilemap tilemap, float to)
    {
        float lerpAlphaTimer = 0.0f;
        Color baseColor = tilemap.color;
        while (lerpAlphaTimer < _alphaLerpTime)
        {
            yield return new WaitForEndOfFrame();
            lerpAlphaTimer += Time.deltaTime;
            tilemap.color = new Color(baseColor.r, baseColor.g, baseColor.b,
                Mathf.Lerp(baseColor.a, to, lerpAlphaTimer / _alphaLerpTime));
        }
    }
}

