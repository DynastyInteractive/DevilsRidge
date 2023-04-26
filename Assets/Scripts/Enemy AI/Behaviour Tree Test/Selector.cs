using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //acts like an "or" gate - same as sequence but returns early when a child is running or succeeds, instead of going through all of them
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        //overriding the Node Evaluate method
        public override NodeState Evaluate()
        {
            foreach (Node node in _children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        _state = NodeState.Success;
                        return _state;
                    case NodeState.Running:
                        _state = NodeState.Success;
                        return _state;
                    default:
                        continue;
                }
            }

            _state = NodeState.Failure;
            return _state;
        }
    }
}
