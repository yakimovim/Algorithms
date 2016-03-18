using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Graphs.Search;

namespace EdlinSoftware.Algorithms.Graphs
{
    /// <summary>
    /// Represents sorter of graph nodes in topological order.
    /// </summary>
    /// <typeparam name="TGraphNode">Type of graph node.</typeparam>
    public class TopologicalSorter<TGraphNode>
    {
        public TGraphNode[] Sort(IEnumerable<TGraphNode> graph, Func<TGraphNode, IEnumerable<TGraphNode>> connectedNodesSelector)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            if (connectedNodesSelector == null) throw new ArgumentNullException(nameof(connectedNodesSelector));

            var graphNodes = graph.ToArray();
            var visitedNodes = new HashSet<TGraphNode>();
            var topologicalOrder = new TGraphNode[graphNodes.Length];
            var currentPlaceInOrder = topologicalOrder.Length - 1;
            var searcher = new DepthFirstSearcher<TGraphNode>(InformationMoments.InformAfterChildren);

            foreach (var node in graphNodes)
            {
                if (visitedNodes.Contains(node) == false)
                {
                    searcher.Search(node, connectedNodesSelector, args =>
                    {
                        visitedNodes.Add(args.TargetNode);
                        topologicalOrder[currentPlaceInOrder--] = args.TargetNode;
                    });
                }
            }

            return topologicalOrder;
        }
    }
}
