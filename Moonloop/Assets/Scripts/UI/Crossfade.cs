using System.Collections;
using UnityEngine;

public class Crossfade : MonoBehaviour
{
    public static Crossfade Instance { get; private set; }

    [SerializeField] private CanvasGroup _overlay;
    public bool initialFadein;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (initialFadein)
        {
            _overlay.alpha = 1f;
            FadeIn();
        }
        else
        {
            _overlay.alpha = 0f;
        }
    }

    public void FadeIn()
    {
        StartCoroutine(LerpAlphaCo(_overlay, 0f));
    }

    public void FadeOut()
    {
        StartCoroutine(LerpAlphaCo(_overlay, 1f));
    }

    IEnumerator LerpAlphaCo(CanvasGroup group, float to)
    {
        float elapsedTime = 0f;
        float baseAlpha = group.alpha;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(baseAlpha, to, elapsedTime);
            yield return null;
        }
    }
}
