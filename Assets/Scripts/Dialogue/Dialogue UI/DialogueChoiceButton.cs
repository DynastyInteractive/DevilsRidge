using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DialogueChoiceButton : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _buttonText;

    public void Init(string choice, UnityAction onClick)
    {
        _buttonText.text = choice;
        _button.onClick.AddListener(onClick);
    }
}
