using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    DialogueGraphView _graphView;
    EditorWindow _window;

    public void Init(DialogueGraphView graphView, EditorWindow window)
    {
        _graphView = graphView;
        _window = window;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> tree = new()
        {
            new SearchTreeGroupEntry(new GUIContent("Add Node"), 0),
            new SearchTreeEntry(new GUIContent("Dialogue Node"))
            {
                userData = new DialogueNode(), 
                level = 1
            }
        };
        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent, context.screenMousePosition - _window.position.position);
        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);

        switch (SearchTreeEntry.userData)
        {
            case DialogueNode dialogueNode:
                _graphView.CreateNode("Dialogue Node", localMousePosition);
                return true;
            default: 
                return false;
        }
    }
}
