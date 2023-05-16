using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text _statName;
    [SerializeField] TMP_Text _statValue;

    public TMP_Text StatName { get => _statName; set => _statName = value; }
    public TMP_Text StatValue { get => _statValue; set => _statValue = value; }
}
