using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //used to build the behaviour tree
    public abstract class Tree : MonoBehaviour
    {
        [SerializeField] Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if(_root != null) _root.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}
