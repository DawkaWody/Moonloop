using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlanetController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int levelIndex;

    public Image planetImage;
    public Sprite nonInteractiveSprite;
    public Sprite unlockedSprite;
    public Sprite lockedSprite;
    public Sprite hoverSprite;

    private bool isInteractable;

    public void SetInteractive(bool interactive)
    {
        isInteractable = interactive;

        bool unlocked = GameManager.Instance.IsLevelUnlocked(levelIndex);

        if (!interactive)
        {
            planetImage.sprite = nonInteractiveSprite;
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
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isInteractable && GameManager.Instance.IsLevelUnlocked(levelIndex))
        {
            planetImage.sprite = unlockedSprite;
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
