using System;
using System.Collections.Generic;

namespace EdlinSoftware.Algorithms.Graphs.Search
{
    public class BreadthFirstSearcher<TGraphNode> : IGraphSearcher<TGraphNode>
    {
        private readonly HashSet<TGraphNode> _visitedNodes = new HashSet<TGraphNode>();

        public void ClearVisitedNodes()
        {
            _visitedNodes.Clear();
        }

        public void Search(TGraphNode startNode, 
            Func<TGraphNode, IEnumerable<TGraphNode>> connectedNodesSelector, 
            Action<IGraphNodeVisitingArgs<TGraphNode>> nodeAction)
        {
            if (startNode == null) throw new ArgumentNullException("startNode");
            if (connectedNodesSelector == null) throw new ArgumentNullException("connectedNodesSelector");
            if (nodeAction == null) throw new ArgumentNullException("nodeAction");

            var args = new GraphNodeVisitingArgs<TGraphNode>();

            var processQueue = new Queue<TGraphNode>();

            if (!Enqueue(processQueue, startNode, args, nodeAction))
            { return; }

            while (processQueue.Count > 0)
            {
                var node = processQueue.Dequeue();

                args.SourceNode = node;

                foreach (var nextNode in connectedNodesSelector(node))
                {
                    if (_visitedNodes.Contains(nextNode) == false)
                    {
                        if (!Enqueue(processQueue, nextNode, args, nodeAction))
                        { return; }
                    }
                }

            }
        }

        private bool Enqueue(Queue<TGraphNode> processQueue, 
            TGraphNode node, 
            GraphNodeVisitingArgs<TGraphNode> args, 
            Action<IGraphNodeVisitingArgs<TGraphNode>> nodeAction)
        {
            processQueue.Enqueue(node);
            _visitedNodes.Add(node);
            args.TargetNode = node;
            nodeAction(args);

            return !args.IsSearchCancelled;
        }
    }
}
