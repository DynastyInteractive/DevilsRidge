using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image), typeof(Button))]
public class TabButton : MonoBehaviour
{
    Button _button;

    public event Action<TabButton> OnTabSelected;

    void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnButtonPressed);
    }

    void OnButtonPressed()
    {
        OnTabSelected?.Invoke(this);
    }
}
