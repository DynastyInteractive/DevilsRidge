using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    NodeSearchWindow _searchWindow;
    EditorWindow _window;

    public event Action OnGraphChanged;

    public Vector2 DefaultNodeSize => new(150, 200);
    public bool HasEntryNode
    {
        get
        {
            bool hasEntryNode = false;
            nodes.ForEach((node) =>
            {
                var dialogueNode = (DialogueNode)node;
                if (dialogueNode.EntryPoint) hasEntryNode = true;
            });

            return hasEntryNode;
        }
    }

    public DialogueGraphView(EditorWindow window)
    {
        _window = window;

        graphViewChanged = OnGraphModified;

        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        GridBackground grid = new();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
        AddSearchWindow();
    }

    GraphViewChange OnGraphModified(GraphViewChange change)
    {
        OnGraphChanged?.Invoke();

        return change;
    }

    void AddSearchWindow()
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Init(this, _window);
        nodeCreationRequest += context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    } 

    DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode
        {            
            title = "START",
            Parent = this,
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            EntryPoint = true
        };

        Port generatedPort = node.GeneratePort(Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.capabilities &= ~Capabilities.Selectable;
        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }

    public DialogueNode CreateDialogueNode(string speakerName, Vector2 mousePosition, string dialogueText = null)
    {
        DialogueNode dialogueNode = new()
        {
            title = string.IsNullOrWhiteSpace(speakerName) ? "Dialogue Node" : speakerName,
            Parent = this,
            DialogueText = (dialogueText == null) ? "Dialogue" : dialogueText,
            GUID = Guid.NewGuid().ToString()
        };
        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

        var oldLabel = dialogueNode.titleContainer.Q<Label>("title-label");
        dialogueNode.titleContainer.Remove(oldLabel);

        TextField speakerNameField = new()
        {
            value = speakerName,
        };
        speakerNameField.RegisterValueChangedCallback(evt => dialogueNode.title = evt.newValue);
        speakerNameField.RegisterValueChangedCallback(evt => OnGraphChanged?.Invoke());
        speakerNameField.style.maxWidth = 125;
        speakerNameField.style.overflow = Overflow.Visible;
        dialogueNode.titleContainer.Add(speakerNameField);
        speakerNameField.SendToBack();

        Port inputPort = dialogueNode.GeneratePort(Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        Button addChoiceButton = new(() => dialogueNode.AddChoicePort(onValueChangedCallback: () => OnGraphChanged?.Invoke()))
        {
            text = "Add Choice"
        };
        dialogueNode.titleButtonContainer.Add(addChoiceButton);

        TextField dialogueField = new TextField(string.Empty);
        dialogueField.SetValueWithoutNotify(dialogueNode.DialogueText);
        dialogueField.RegisterValueChangedCallback(dialogue => 
        {
            dialogueNode.DialogueText = dialogue.newValue;
            OnGraphChanged?.Invoke(); 
        });
        dialogueField.style.maxWidth = 250;
        dialogueField.multiline = true;
        dialogueField.style.whiteSpace = WhiteSpace.Normal;
        dialogueNode.mainContainer.Add(dialogueField);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(mousePosition, DefaultNodeSize));

        OnGraphChanged?.Invoke();

        return dialogueNode;
    }

    public void CreateNode(string nodeName, Vector2 mousePosition)
    {
        DialogueNode node = CreateDialogueNode(nodeName, mousePosition);
        node.AddChoicePort();
        AddElement(node);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new();

        ports.ForEach((port) =>
        {
            if (startPort == port || startPort.node == port.node) return;
            if (startPort.direction == port.direction) return;

            compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

}
