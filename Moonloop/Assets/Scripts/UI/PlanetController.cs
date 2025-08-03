using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class PlanetController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int levelIndex;

    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private Image planetImage;
    [SerializeField] private Sprite nonInteractiveSprite;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite hoverSprite;

    private bool isInteractable;

    public void SetInteractive(bool interactive)
    {
        isInteractable = interactive;

        bool unlocked = GameManager.Instance.IsLevelUnlocked(levelIndex);

        if (!interactive)
        {
            planetImage.sprite = nonInteractiveSprite;
            bestTimeText.gameObject.SetActive(false);
        }
        else
        {
            planetImage.sprite = unlocked ? unlockedSprite : lockedSprite;
        }

        GetComponent<Button>().interactable = unlocked && interactive;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isInteractable && GameManager.Instance.IsLevelUnlocked(levelIndex))
        {
            planetImage.sprite = hoverSprite;

            TimeSpan bestTime = GameManager.Instance.levelTimes[levelIndex];
            if (bestTime != TimeSpan.Zero)
                bestTimeText.text = $"Best: {bestTime:mm\\:ss\\.fff}";
            else
                bestTimeText.text = "--:--.---";

            bestTimeText.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isInteractable && GameManager.Instance.IsLevelUnlocked(levelIndex))
        {
            planetImage.sprite = unlockedSprite;

            bestTimeText.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isInteractable && GameManager.Instance.IsLevelUnlocked(levelIndex))
        {
            GameManager.Instance.StartLevel(levelIndex);
        }
    }
}
