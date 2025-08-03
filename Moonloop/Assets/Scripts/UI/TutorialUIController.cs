using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

    private Queue<IEnumerator> _messageQueue = new Queue<IEnumerator>();
    private bool _isDisplayingMessage = false;

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
        _timerFocus.alpha = 0f;
        _moonBarFocus.alpha = 0f;

        Enqueue(ActivateGroupCo(_controls));
        StartCoroutine(ProcessQueue());
    }

    public void UiTour()
    {
        _messageQueue.Enqueue(UiTourCo());
        if (!_isDisplayingMessage)
            StartCoroutine(ProcessQueue());
    }

    void Enqueue(IEnumerator coroutine)
    {
        _messageQueue.Enqueue(coroutine);
    }

    public void ActivateGroup(int groupId)
    {
        Enqueue(ActivateGroupById(groupId));
        if (!_isDisplayingMessage)
            StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        _isDisplayingMessage = true;
        while (_messageQueue.Count > 0)
        {
            yield return StartCoroutine(_messageQueue.Dequeue());
        }
        _isDisplayingMessage = false;
    }

    IEnumerator UiTourCo()
    {
        _playerMovementController.Freeze();

        yield return StartCoroutine(ActivateGroupWithFocus(_timerFocus, _timer));
        yield return StartCoroutine(ActivateGroupWithFocus(_moonBarFocus, _moonBar));
        yield return StartCoroutine(ActivateGroupCo(_unstable));

        _playerMovementController.enabled = true;
    }

    IEnumerator ActivateGroupById(int groupId)
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

        if (group != null)
            yield return StartCoroutine(ActivateGroupCo(group));
    }

    IEnumerator ActivateGroupCo(CanvasGroup group)
    {
        yield return StartCoroutine(LerpAlphaCo(group, 1f));
        yield return new WaitForSeconds(_onScreenDuration);
        yield return StartCoroutine(LerpAlphaCo(group, 0f));
    }

    IEnumerator ActivateGroupWithFocus(CanvasGroup focus, CanvasGroup group)
    {
        yield return StartCoroutine(LerpAlphaCo(focus, 1f));
        yield return new WaitForSeconds(_fadeDuration / 2f);
        yield return StartCoroutine(ActivateGroupCo(group));
        yield return StartCoroutine(LerpAlphaCo(focus, 0f));
        yield return new WaitForSeconds(_fadeDuration / 2f);
    }

    IEnumerator LerpAlphaCo(CanvasGroup group, float to)
    {
        float elapsedTime = 0f;
        float baseAlpha = group.alpha;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(baseAlpha, to, elapsedTime / _fadeDuration);
            yield return null;
        }
        group.alpha = to;
    }
}
