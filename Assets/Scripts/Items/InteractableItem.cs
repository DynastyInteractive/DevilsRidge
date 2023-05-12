using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InteractableItem : Interactable
{
    [SerializeField] Item _item;

    public override void Interact()
    {
        Inventory.Instance.Add(_item);
        Destroy(gameObject);
    }
}

#if UNITY_EDITOR
    [CustomEditor(typeof(InteractableItem))]
    public class InteractableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!Application.isPlaying) return;

            InteractableItem interactable = (InteractableItem)target;
            if (GUILayout.Button("Interact"))
            {
                interactable.Interact();
            }
        }
    }
#endif

