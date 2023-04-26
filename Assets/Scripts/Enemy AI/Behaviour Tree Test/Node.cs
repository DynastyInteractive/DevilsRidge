using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }

    //single element in the behaviour tree which can access its parent and children
    public abstract class Node
    {
        protected NodeState state;

        //the parent of this node and all of its children
        public Node parent;
        protected List<Node> children = new List<Node>();

        public Node()
        {
            parent = null;
        }

        //adds all the children of this node to the list, and the children's parent to this node
        public Node(List<Node> children)
        {
            foreach(Node child in children)
            {
                _Attach(child);
            }
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        //virtual so all derived scripts can adapt the method
        public virtual NodeState Evaluate() => NodeState.Failure;

        //Dictionary to handle the shared data
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        //Sets the data
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        //Gets the data
        public object GetData(string key)
        {
            object val = null;
            if(_dataContext.TryGetValue(key, out val)) return val;

            Node node = parent;
            if(node != null) val = node.GetData(key);
            return val;
        }

        //Clears the data
        public bool ClearData(string key)
        {
            bool cleared = false;
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            if(node != null) cleared = node.ClearData(key);
            return cleared;
        }
    }
}
