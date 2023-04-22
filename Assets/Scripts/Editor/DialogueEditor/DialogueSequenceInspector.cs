using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueSequence))]
public class DialogueSequenceInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Dialogue Editor"))
        {
            DrawDefaultInspector();
            DialogueSequence asset = target as DialogueSequence;

            if (asset == null) return;

            string assetPath = AssetDatabase.GetAssetPath(asset);
            Debug.Log(assetPath);
            DialogueGraph window = DialogueGraph.OpenDialogueGraphWindow();
            window.OpenGraph(assetPath);
        }
    }
}
