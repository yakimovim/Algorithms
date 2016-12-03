using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Graphs.Search;

namespace EdlinSoftware.Algorithms.Graphs
{
    /// <summary>
    /// Represents searcher of strongly connected components in directed graph.
    /// </summary>
    public class StronglyConnectedComponentsSearcher
    {
        private Dictionary<long, HashSet<long>> _directEdges;
        private Dictionary<long, HashSet<long>> _reverseEdges;

        public HashSet<long>[] Search(long numberOfNodes, Func<long, IEnumerable<long>> connectedNodesSelector)
        {
            if (numberOfNodes < 0) throw new ArgumentOutOfRangeException(nameof(numberOfNodes));
            if (connectedNodesSelector == null) throw new ArgumentNullException(nameof(connectedNodesSelector));

            BuildEdges(numberOfNodes, connectedNodesSelector);

            var orderOfNodes = MakeOrderOfNodes(numberOfNodes);

            var listOfComponents = CalculateComponents(orderOfNodes);

            return listOfComponents.ToArray();
        }

        private void BuildEdges(long numberOfNodes, Func<long, IEnumerable<long>> connectedNodesSelector)
        {
            _directEdges = new Dictionary<long, HashSet<long>>();
            _reverseEdges = new Dictionary<long, HashSet<long>>();

            for (long node = 0; node < numberOfNodes; node++)
            {
                foreach (var connectedNode in connectedNodesSelector(node))
                {
                    PutEdge(_directEdges, node, connectedNode);
                    PutEdge(_reverseEdges, connectedNode, node);
                }
            }
        }

        private void PutEdge(Dictionary<long, HashSet<long>> edges, long fromNode, long toNode)
        {
            if (edges.ContainsKey(fromNode) == false)
                edges[fromNode] = new HashSet<long>();
            edges[fromNode].Add(toNode);
        }

        private List<long> MakeOrderOfNodes(long numberOfNodes)
        {
            var visitedNodes = new HashSet<long>();
            var orderOfNodes = new List<long>();

            Func<long, IEnumerable<long>> connectedNodesProvider = node => _reverseEdges.ContainsKey(node) ? (IEnumerable<long>) _reverseEdges[node] : new long[0];

            var searcher = new DepthFirstSearcher<long>(InformationMoments.InformAfterChildren);

            for (long node = numberOfNodes - 1; node >= 0; node--)
            {
                if (visitedNodes.Contains(node) == false)
                {
                    searcher.Search(node, connectedNodesProvider, args =>
                    {
                        visitedNodes.Add(args.TargetNode);
                        orderOfNodes.Add(args.TargetNode);
                    });
                }
            }

            return orderOfNodes;
        }

        private List<HashSet<long>> CalculateComponents(List<long> orderOfNodes)
        {
            var visitedNodes = new HashSet<long>();
            var listOfComponents = new List<HashSet<long>>();
            HashSet<long> currentComponent = null;
            
            Func<long, IEnumerable<long>> connectedNodesProvider = node => _directEdges.ContainsKey(node) ? (IEnumerable<long>)_directEdges[node] : new long[0];

            var searcher = new DepthFirstSearcher<long>();

            foreach (long node in orderOfNodes.Reverse<long>())
            {
                if (visitedNodes.Contains(node) == false)
                {
                    if (currentComponent != null)
                    {
                        listOfComponents.Add(currentComponent);
                    }

                    currentComponent = new HashSet<long>();

                    var component = currentComponent;
                    searcher.Search(node, connectedNodesProvider, args =>
                    {
                        visitedNodes.Add(args.TargetNode);
                        component.Add(args.TargetNode);
                    });
                }
            }

            if (currentComponent != null)
            {
                listOfComponents.Add(currentComponent);
            }

            return listOfComponents;
        }
    }
}
