using System.Collections;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _platforms;
    [SerializeField] private CanvasGroup _controls;
    [SerializeField] private CanvasGroup _persistentPlatforms;
    [SerializeField] private CanvasGroup _timer;
    [SerializeField] private CanvasGroup _moonBar;
    [SerializeField] private CanvasGroup _unstable;
    [SerializeField] private CanvasGroup _darkPlatforms;
    [SerializeField] private CanvasGroup _levelEnding;
    [SerializeField] private CanvasGroup _timerFocus;
    [SerializeField] private CanvasGroup _moonBarFocus;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _onScreenDuration = 1.8f;
    [SerializeField] private PlayerMovementController _playerMovementController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _platforms.alpha = 0f;
        _controls.alpha = 0f;
        _persistentPlatforms.alpha = 0f;
        _timer.alpha = 0f;
        _moonBar.alpha = 0f;
        _unstable.alpha = 0f;
        _darkPlatforms.alpha = 0f;
        _levelEnding.alpha = 0f;

        ActivateGroup(1);
    }

    public void UiTour()
    {
        StartCoroutine(UiTourCo());
    }

    IEnumerator UiTourCo()
    {
        _playerMovementController.Freeze();
        StartCoroutine(LerpAlphaCo(_timerFocus, 1f));
        yield return new WaitForSeconds(_fadeDuration / 2);
        ActivateGroup(3);
        yield return new WaitForSeconds(_onScreenDuration);
        StartCoroutine(LerpAlphaCo(_timerFocus, 0f));
        yield return new WaitForSeconds(_fadeDuration);
        StartCoroutine(LerpAlphaCo(_moonBarFocus, 1f));
        yield return new WaitForSeconds(_fadeDuration / 2);
        ActivateGroup(4);
        yield return new WaitForSeconds(_onScreenDuration + _fadeDuration);
        ActivateGroup(5);
        yield return new WaitForSeconds(_onScreenDuration + _fadeDuration);
        StartCoroutine(LerpAlphaCo(_moonBarFocus, 0f));
        _playerMovementController.enabled = true;
    }

    public void ActivateGroup(int groupId)
    {
        CanvasGroup group = groupId switch
        {
            0 => _platforms,
            1 => _controls,
            2 => _persistentPlatforms,
            3 => _timer,
            4 => _moonBar,
            5 => _unstable,
            6 => _darkPlatforms,
            7 => _levelEnding,
            _ => null
        };
        StartCoroutine(ActivateGroupCo(group));
    }

    IEnumerator ActivateGroupCo(CanvasGroup group)
    {
        StartCoroutine(LerpAlphaCo(group, 1f));
        yield return new WaitForSeconds(_onScreenDuration);
        StartCoroutine(LerpAlphaCo(group, 0f));
    }

    IEnumerator LerpAlphaCo(CanvasGroup group, float to)
    {
        float elapsedTime = 0f;
        float baseAlpha = group.alpha;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(baseAlpha, to, elapsedTime);
            yield return null;
        }
    }
}
