using System.Collections.Generic;

namespace BehaviourTree
{
    //composite that acts like an "and" gate, this node will only succeed if all child nodes succeed
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        //overriding the Node Evaluate method
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            //run through each child node to see their state. if any fail, then we stop the sequence. Then continue to check if remaining are running or a success
            foreach(Node node in _children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        _state = NodeState.Failure;
                        return _state;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        _state = NodeState.Success;
                        return _state;
                }
            }

            _state = anyChildIsRunning ? NodeState.Running : NodeState.Success;
            return _state;
        }
    }
}
