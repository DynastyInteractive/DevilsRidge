using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] string _action;

    public string Action => _action;

    public abstract void Interact(GameObject player);
}
