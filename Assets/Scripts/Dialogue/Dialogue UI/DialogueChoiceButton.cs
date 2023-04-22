using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DialogueChoiceButton : MonoBehaviour
{
    [SerializeField] TMP_Text _buttonText;

    Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();    
    }

    public void Init(string choice, UnityAction onClick)
    {
        _buttonText.text = choice;
        _button.onClick.AddListener(onClick);
    }
}
