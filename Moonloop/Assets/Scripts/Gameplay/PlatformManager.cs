using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private Transform[] _platformGroups;
    [SerializeField] private float _alphaOnInactive = 0.4f;
    [SerializeField] private float _alphaLerpTime = 0.3f;
    private int _currentGroup = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = _currentGroup + 1; i < _platformGroups.Length; i++)
            Deactivate(_platformGroups[i]);
    }

    public void NextGroup()
    {
        Deactivate(_platformGroups[_currentGroup]);
        _currentGroup++;
        if (_currentGroup >= _platformGroups.Length) _currentGroup = 0;
        Activate(_platformGroups[_currentGroup]);
    }

    public void PreviousGroup()
    {
        Deactivate(_platformGroups[_currentGroup]);
        _currentGroup--;
        if (_currentGroup < 0) _currentGroup = _platformGroups.Length - 1;
        Activate(_platformGroups[_currentGroup]);
    }

    private void Activate(Transform platformGroup)
    {
        Color baseColor = platformGroup.GetComponent<Tilemap>().color;
        StartCoroutine(LerpAlphaCo(platformGroup.GetComponent<Tilemap>(), 1.0f));
        platformGroup.GetComponent<TilemapCollider2D>().enabled = true;
    }

    private void Deactivate(Transform platformGroup)
    {
        Color baseColor = platformGroup.GetComponent<Tilemap>().color;
        StartCoroutine(LerpAlphaCo(platformGroup.GetComponent<Tilemap>(), _alphaOnInactive));
        platformGroup.GetComponent<TilemapCollider2D>().enabled = false;
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
