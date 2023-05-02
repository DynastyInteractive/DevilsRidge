using System;
using Unity.Tutorials.Core.Editor;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    DialogueGraphView _graphView;
    string _filePath = string.Empty;

    Button _saveAsButton;
    Button _saveButton;

    [MenuItem("Window/Dialogue/Dialogue Graph")]
    public static DialogueGraph OpenDialogueGraphWindow()
    {
        DialogueGraph window = OpenDialogueGraphWindow("Dialogue Graph");
        return window;
    }
    
    public static DialogueGraph OpenDialogueGraphWindow(string windowName)
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent(windowName);
        return window;
    }

    [UnityEditor.Callbacks.OnOpenAsset()]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        UnityEngine.Object asset = EditorUtility.InstanceIDToObject(instanceID);
        if (asset is DialogueSequence)
        {
            string assetPath = AssetDatabase.GetAssetPath(instanceID);
            DialogueGraph window = OpenDialogueGraphWindow(asset.name);
            window.OpenGraph(assetPath);
            return true;
        }
        return false; //let unity open it.
    }

    public override void SaveChanges()
    {
        base.SaveChanges();
        if (_filePath.IsNullOrWhiteSpace()) SaveGraphAs();
        else SaveGraph();
    }

    void OnEnable()
    {
        GraphSaveUtility.OnGraphLoaded += EnableSaveButton;
        GraphSaveUtility.OnGraphSaved += EnableSaveButton;

        ConstructGraphView();
        GenerateToolbar();

        _graphView.OnGraphChanged += () =>
        {
            hasUnsavedChanges = true;
        };
    }

    void EnableSaveButton()
    {
        hasUnsavedChanges = false;
        _saveButton.SetEnabled(true);
    }

    void OnDisable()
    {
        rootVisualElement.Remove( _graphView );
    }

    void ConstructGraphView()
    {
        _graphView = new DialogueGraphView(this)
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add( _graphView );
    }

    void GenerateToolbar()
    {
        Toolbar toolbar = new();

        _saveAsButton = new(() =>
        {
            SaveGraphAs();
        });
        _saveAsButton.text = "Save As";
        toolbar.Add(_saveAsButton);

        _saveButton = new(() =>
        {
            SaveGraph();
        });
        _saveButton.text = "Save";
        toolbar.Add(_saveButton);
        _saveButton.SetEnabled(_filePath.IsNotNullOrWhiteSpace());

        Button openButton = new(() =>
        {
            OpenGraph();
        });
        openButton.text = "Open";
        toolbar.Add(openButton);

        rootVisualElement.Add(toolbar);
    }

    private void SaveGraphAs()
    {
        _filePath = EditorUtility.SaveFilePanelInProject(
            "Save Dialogue Graph",
            "New Dialogue Sequence.asset",
            "asset",
            "");

        if (_filePath.Length > 0)
            GraphSaveUtility.GetInstance(_graphView).SaveGraph(_filePath);

    }

    private void SaveGraph()
    {
        if (string.IsNullOrWhiteSpace(_filePath))
        {
            DisplayInvalidFileName();
            return;
        }

        GraphSaveUtility.GetInstance(_graphView).SaveGraph(_filePath);
    }

    private void OpenGraph()
    {
        _filePath = EditorUtility.OpenFilePanel(
            "Open Dialogue Graph",
            $"", 
            "asset");

        _filePath = _filePath.Replace(Application.dataPath, "Assets/");

        if (_filePath.Length > 0)
            GraphSaveUtility.GetInstance(_graphView).LoadGraph(_filePath);
    }

    public void OpenGraph(string filePath)
    {
        _filePath = filePath;

        if (_filePath.Length > 0)
            GraphSaveUtility.GetInstance(_graphView).LoadGraph(_filePath);
    }

    void DisplayInvalidFileName()
    {
        EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
    }
}
    