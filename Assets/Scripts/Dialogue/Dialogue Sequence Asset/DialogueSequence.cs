using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Sequence", menuName = "Dialogue Sequence", order = 81)]
[Serializable]
public class DialogueSequence : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new();
    public List<DialogueNodeData> DialogueNodeData = new();
}
