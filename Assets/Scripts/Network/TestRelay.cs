using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestRelay : MonoBehaviour
{
    [SerializeField] GameObject _uICamera;
    [SerializeField] Button _createRelayButton;
    [SerializeField] Button _joinRelayButton;
    [SerializeField] TMP_InputField _joinCodeField;

    void Awake()
    {
        _createRelayButton.onClick.AddListener(() => { RelayManager.Instance.CreateRelayAsync(); _uICamera.SetActive(false); });
        _joinRelayButton.onClick.AddListener(() => { RelayManager.Instance.JoinRelayAsync(_joinCodeField.text); _uICamera.SetActive(false); });
    }
}
