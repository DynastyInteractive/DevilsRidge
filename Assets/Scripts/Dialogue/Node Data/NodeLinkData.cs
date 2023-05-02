using System;
using UnityEngine;

[Serializable]
public class NodeLinkData
{
    [SerializeField] string _baseNodeGUID;
    [SerializeField] string _portName;
    [SerializeField] string _targetNodeGUID;

    public string BaseNodeGUID { get => _baseNodeGUID; set => _baseNodeGUID = value; }
    public string ChoiceText { get => _portName; set => _portName = value; }
    public string TargetNodeGUID { get => _targetNodeGUID; set => _targetNodeGUID = value; }

}
