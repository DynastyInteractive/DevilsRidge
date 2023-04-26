using System.Collections;
using System.Collections.Generic;
using BehaviourTree;

public class Enemy : Tree
{
    public UnityEngine.AI.NavMeshAgent nav;

    public UnityEngine.Vector3 _startingPos;

    public static float speed = 2f;

    private new void Start()
    {
        _startingPos = transform.position;
        UnityEngine.Debug.Log(_startingPos);
    }

    protected override Node SetupTree()
    {
        Node root = new Wandering(transform, nav, _startingPos);
        return root;
    }
}
