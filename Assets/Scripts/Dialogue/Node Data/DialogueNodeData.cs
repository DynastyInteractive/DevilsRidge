using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNodeData
{
    [SerializeField] string _nodeGUID;
    [SerializeField] string _speakerName;
    [SerializeField] string _dialogueText;
    [SerializeField] List<NodeLinkData> _choices;
    [SerializeField] Vector2 _position;

    public string NodeGUID { get => _nodeGUID; set => _nodeGUID = value; }
    public string SpeakerName { get => _speakerName; set => _speakerName = value; }
    public string DialogueText { get => _dialogueText; set => _dialogueText = value; }
    public List<NodeLinkData> Choices { get => _choices; set => _choices = value; }
    public Vector2 Position { get => _position; set => _position = value; }
}
