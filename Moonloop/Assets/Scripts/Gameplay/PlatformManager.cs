using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private Platform[] _platforms;
    [SerializeField] private float _alphaOnInactive = 0.2f;
    [SerializeField] private float _alphaLerpTime = 0.3f;

    private Transform[] _platformTransforms;
    private ArrayList _darkPlatforms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _platformTransforms = new Transform[_platforms.Length];
        _darkPlatforms = new ArrayList();
        for (int i = 0; i < _platforms.Length; i++)
        {
            _platformTransforms[i] = _platforms[i].transform;
            Deactivate(_platformTransforms[i]);
            if (_platforms[i].type == PlatformType.Dark)
                _darkPlatforms.Add(_platforms[i]);
        }
    }

    public void Activate(Transform platform)
    {
        StartCoroutine(LerpAlphaCo(platform.GetComponent<Tilemap>(), 1.0f));
        platform.GetComponent<Collider2D>().isTrigger = false;
        platform.GetComponent<Platform>().isActive = true;
    }

    public void Deactivate(Transform platform)
    {

        StartCoroutine(LerpAlphaCo(platform.GetComponent<Tilemap>(), _alphaOnInactive));
        platform.GetComponent<Collider2D>().isTrigger = true;
        platform.GetComponent<Platform>().isActive = false;
    }

    public void ActivateDarkPlatforms()
    {
        foreach (Platform platform in _darkPlatforms)
        {
            if (!platform.isActive)
                Activate(platform.transform);
        }
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
