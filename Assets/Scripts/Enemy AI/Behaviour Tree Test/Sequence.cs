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
            bool isAnyChildRunning = false;

            //run through each child node to see their state. if any fail, then we stop the sequence. Then continue to check if remaining are running or a success
            foreach(Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        state = NodeState.Failure;
                        return state;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        isAnyChildRunning = true;
                        continue;
                    default:
                        state = NodeState.Success;
                        return state;
                }
            }

            state = isAnyChildRunning ? NodeState.Running : NodeState.Success;
            return state;
        }
    }
}
