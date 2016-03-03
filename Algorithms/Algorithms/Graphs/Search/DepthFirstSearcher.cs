using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Graphs.Search
{
    public enum InformationMoments
    {
        InformWhenFirstMet,
        InformAfterChilden,
        Both
    }

    public class DepthFirstSearcher<TGraphNode> : IGraphSearcher<TGraphNode>
    {
        private class NodeDescription
        {
            public NodeDescription(TGraphNode sourceNode, TGraphNode node, IEnumerable<TGraphNode> nodeChildren)
            {
                SourceNode = sourceNode;
                Node = node;
                NodeChildren = nodeChildren?.ToArray() ?? new TGraphNode[0];
                CurrentChildIndex = 0;
            }

            public TGraphNode SourceNode { get; }

            public TGraphNode Node { get; }

            public TGraphNode[] NodeChildren { get; }

            public long CurrentChildIndex { get; set; }
        }

        private readonly HashSet<TGraphNode> _visitedNodes = new HashSet<TGraphNode>();
        private readonly InformationMoments _informationMoment;

        public void ClearVisitedNodes()
        {
            _visitedNodes.Clear();
        }

        public DepthFirstSearcher(InformationMoments informationMoment = InformationMoments.InformWhenFirstMet)
        {
            _informationMoment = informationMoment;
        }

        public void Search(TGraphNode startNode,
            Func<TGraphNode, IEnumerable<TGraphNode>> connectedNodesSelector,
            Action<IGraphNodeVisitingArgs<TGraphNode>> nodeAction)
        {
            if (startNode == null) throw new ArgumentNullException(nameof(startNode));
            if (connectedNodesSelector == null) throw new ArgumentNullException(nameof(connectedNodesSelector));
            if (nodeAction == null) throw new ArgumentNullException(nameof(nodeAction));

            var args = new GraphNodeVisitingArgs<TGraphNode>();

            var processStack = new Stack<NodeDescription>();

            if (!Push(processStack, startNode, default(TGraphNode), connectedNodesSelector, args, nodeAction))
            { return; }

            while (processStack.Count > 0)
            {
                var nodeDescription = processStack.Peek();

                TGraphNode nextChildNode;

                if (TryGetNextUnvisitedChild(nodeDescription, out nextChildNode) == false)
                {
                    processStack.Pop();

                    if (_informationMoment != InformationMoments.InformWhenFirstMet)
                    {
                        args.SourceNode = nodeDescription.SourceNode;
                        args.TargetNode = nodeDescription.Node;
                        nodeAction(args);
                        if (args.IsSearchCancelled)
                        { return; }
                    }
                }
                else
                {
                    if (!Push(processStack, nextChildNode, nodeDescription.Node, connectedNodesSelector, args, nodeAction))
                    { return; }
                }
            }
        }

        private bool TryGetNextUnvisitedChild(NodeDescription nodeDescription,out TGraphNode nextChildNode)
        {
            nextChildNode = default(TGraphNode);

            while (nodeDescription.CurrentChildIndex < nodeDescription.NodeChildren.Length)
            {
                var childNode = nodeDescription.NodeChildren[nodeDescription.CurrentChildIndex++];
                if (_visitedNodes.Contains(childNode) == false)
                {
                    nextChildNode = childNode;
                    return true;
                }
            }

            return false;
        }

        private bool Push(Stack<NodeDescription> processStack,
            TGraphNode node,
            TGraphNode sourceNode,
            Func<TGraphNode, IEnumerable<TGraphNode>> connectedNodesSelector,
            GraphNodeVisitingArgs<TGraphNode> args,
            Action<IGraphNodeVisitingArgs<TGraphNode>> nodeAction)
        {
            _visitedNodes.Add(node);
            processStack.Push(new NodeDescription(sourceNode, node, connectedNodesSelector(node)));

            if (_informationMoment != InformationMoments.InformAfterChilden)
            {
                args.SourceNode = sourceNode;
                args.TargetNode = node;
                nodeAction(args);
                return !args.IsSearchCancelled;
            }

            return true;
        }
    }
}
