using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DialogueNode : Node
{
    public GraphView Parent { get; set; }
    public string GUID { get; set; }
    public string DialogueText { get; set; }
    public bool EntryPoint { get; set; } = false;

    public Port GeneratePort(Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float)); // Arbitrary type
    }

    public void AddChoicePort(string overriddenPortName = "", Action onValueChangedCallback = null)
    {
        Port generatedPort = GeneratePort(Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        int outputPortCount = outputContainer.Query("connector").ToList().Count;

        string choicePortName = string.IsNullOrWhiteSpace(overriddenPortName) ? 
            $"Choice {outputPortCount + 1}" : overriddenPortName;

        generatedPort.portName = choicePortName;

        TextField textField = new()
        {
            value = choicePortName,
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        textField.RegisterValueChangedCallback(evt => onValueChangedCallback?.Invoke());
        textField.style.maxWidth = 125;
        textField.style.overflow = Overflow.Visible;
        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);

        Button deleteButton = new(() => RemoveChoicePort(generatedPort))
        {
            text = "✘"
        };
        generatedPort.contentContainer.Add(deleteButton);

        outputContainer.Add(generatedPort);
        RefreshPorts();
        RefreshExpandedState();
    }

    public void RemoveChoicePort(Port portToRemove)
    {
        var edges = Parent.edges.ToList();
        
        edges.Where(edge => edge.output == portToRemove).ToList()
            .ForEach(edge => Parent.RemoveElement(edge));

        outputContainer.Remove(portToRemove);

        RefreshExpandedState();
        RefreshPorts(); 
    }
}
