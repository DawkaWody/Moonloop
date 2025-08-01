using System;
using UnityEngine;
using UnityEngine.UI;

public class MoonBar : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _fill;

    [SerializeField] private Sprite[] _icons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fill.fillAmount = 0f;
        _icon.sprite = _icons[0];
    }

    public void SetIcon(int index)
    {
        if (index < 0 || index >= _icons.Length)
        {
            Debug.LogError("Icon index out of range: " + index);
            return;
        }
        _icon.sprite = _icons[index];
    }

    public void SetFill(float fillAmount)
    {
        if (fillAmount < 0f || fillAmount > 1f)
        {
            Debug.LogError("Fill amount must be between 0 and 1: " + fillAmount);
            return;
        }
        _fill.fillAmount = fillAmount;
    }
}
