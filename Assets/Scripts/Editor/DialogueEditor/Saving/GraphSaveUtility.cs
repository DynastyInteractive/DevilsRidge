using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    DialogueGraphView _targetGraphView;
    DialogueSequence _container;

    List<Edge> Edges => _targetGraphView.edges.ToList();
    List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    public static event Action OnGraphLoaded;
    public static event Action OnGraphSaved;

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string filePath)
    {
        var dialogueContainer = ScriptableObject.CreateInstance<DialogueSequence>();
        dialogueContainer.NodeLinks.Clear();
        dialogueContainer.DialogueNodeData.Clear();

        var connectedPorts = Edges.Where(edge => edge.input.node != null).ToArray();

        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = (DialogueNode)connectedPorts[i].output.node;
            var inputNode = (DialogueNode)connectedPorts[i].input.node;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGUID = outputNode.GUID,
                ChoiceText = connectedPorts[i].output.portName,
                TargetNodeGUID = inputNode.GUID
            });
        }

        foreach (var dialogueNode in Nodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                NodeGUID = dialogueNode.GUID,
                SpeakerName = dialogueNode.title,
                DialogueText = dialogueNode.DialogueText,
                Choices = dialogueContainer.NodeLinks.Where(link => link.BaseNodeGUID == dialogueNode.GUID).ToList(),
                Position = dialogueNode.GetPosition().position
            });
        }

        DialogueSequence outputSequence = AssetDatabase.LoadMainAssetAtPath(filePath) as DialogueSequence;
        if (outputSequence != null)
        {
            string name = outputSequence.name;
            EditorUtility.CopySerialized(dialogueContainer, outputSequence);
            outputSequence.name = name;
            AssetDatabase.SaveAssets();
        }
        else
        {
            outputSequence = new DialogueSequence();
            EditorUtility.CopySerialized(dialogueContainer, outputSequence);
            AssetDatabase.CreateAsset(outputSequence, filePath);
            AssetDatabase.SaveAssets();
        }

        OnGraphSaved?.Invoke();
    }

    public void LoadGraph(string filePath) 
    {
        _container = AssetDatabase.LoadAssetAtPath<DialogueSequence>(filePath);        
        if (_container == null)
        {
            EditorUtility.DisplayDialog("File not found!", "Dialogue graph not found.", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();

        OnGraphLoaded?.Invoke();
    }

    void ClearGraph()
    {
        if (Nodes.Count <= 0) return;
        if (_container.NodeLinks.Count <= 0) return;

        Nodes.Find(node => node.EntryPoint).GUID = _container.NodeLinks[0].BaseNodeGUID;
        foreach (var node in Nodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(
                edge => edge.input.node == node).ToList()
                .ForEach(edge => _targetGraphView.RemoveElement(edge)
                );
            _targetGraphView.RemoveElement(node);
        }
    }

    void CreateNodes()
    {
        foreach (var nodeData in _container.DialogueNodeData)
        {
            var tmpNode = _targetGraphView.CreateDialogueNode(nodeData.SpeakerName, nodeData.Position, nodeData.DialogueText);
            tmpNode.GUID = nodeData.NodeGUID;
            _targetGraphView.AddElement(tmpNode);

            var nodePorts = _container.NodeLinks.Where(node => node.BaseNodeGUID == nodeData.NodeGUID).ToList();
            nodePorts.ForEach(node => tmpNode.AddChoicePort(node.ChoiceText));
        }
    }

    void ConnectNodes()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            var connections = _container.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[i].GUID).ToList();
            for (int j = 0; j < connections.Count; j++)
            {
                string targetNodeGUID = connections[j].TargetNodeGUID;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                targetNode.SetPosition(new Rect(
                    _container.DialogueNodeData.First(x => x.NodeGUID == targetNodeGUID).Position,
                    _targetGraphView.DefaultNodeSize));
            }
        }
    }

    void LinkNodes(Port output, Port input)
    {
        Edge tmpEdge = new()
        {
            output = output,
            input = input
        };

        tmpEdge?.input.Connect(tmpEdge);
        tmpEdge?.output.Connect(tmpEdge);
        _targetGraphView.Add(tmpEdge);
    }
}
