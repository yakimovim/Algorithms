using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Paths;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    /// <summary>
    /// Represents searcher of shortest paths from any node to any other node without negative loops using Johnson algorithm.
    /// </summary>
    public class JohnsonShortestPathSearcher : IMultiSourceShortestPathWithoutNegativeLoopSearcher
    {
        public IMultiSourcePathsWithoutNegativeLoop<long, double, long> GetShortestPaths(long numberOfNodes, params IValuedEdge<long, double>[] edges)
        {
            var singleSourceShortestPathSearcher = new YenBellmanFordShortestPathSearcher();

            var singleSourcePaths = singleSourceShortestPathSearcher.GetShortestPaths(numberOfNodes + 1, numberOfNodes,
                edges.Concat(GetEdgesFromAdditionalNode(numberOfNodes)).ToArray());

            var nodeWeights = GetNodeWeights(singleSourcePaths, numberOfNodes);

            return new JohnsonShortestPaths(numberOfNodes, edges, nodeWeights)
            {
                HasNegativeLoop = singleSourcePaths.HasNegativeLoop
            };
        }

        private IEnumerable<IValuedEdge<long, double>> GetEdgesFromAdditionalNode(long additionalNode)
        {
            for (long node = 0; node < additionalNode; node++)
            {
                var currentNode = node;

                yield return ValuedEdge<long, double>.Directed(EqualityComparer<long>.Default, edge =>
                {
                    edge.End1 = additionalNode;
                    edge.End2 = currentNode;
                    edge.Value = 0.0;
                });
            }
        }

        private IDictionary<long, double> GetNodeWeights(ISingleSourcePathsWithoutNegativeLoop<long, double, long> singleSourcePaths, long numberOfNodes)
        {
            var nodeWeights = new Dictionary<long, double>();

            if (singleSourcePaths.HasNegativeLoop == false)
            {
                for (long node = 0; node < numberOfNodes; node++)
                {
                    nodeWeights[node] = singleSourcePaths.GetPath(node).Value;
                }
            }

            return nodeWeights;
        }
    }

    internal class JohnsonShortestPaths : IMultiSourcePathsWithoutNegativeLoop<long, double, long>
    {
        private readonly IDictionary<long, ISingleSourcePaths<long, double, long>> _dijkstraPathsCache = new Dictionary<long, ISingleSourcePaths<long, double, long>>();
        private readonly DijkstraShortestPathSearcher _dijkstraShortestPathSearcher = new DijkstraShortestPathSearcher();

        private readonly long _numberOfNodes;
        private readonly IValuedEdge<long, double>[] _edges;
        private readonly IDictionary<long, double> _nodeWeights;

        private IValuedEdge<long, double>[] _reweightedEdges;

        private IValuedEdge<long, double>[] ReweightedEdges => _reweightedEdges ?? (_reweightedEdges = GetReweightedEdges());

        public JohnsonShortestPaths(long numberOfNodes, IValuedEdge<long, double>[] edges, IDictionary<long, double> nodeWeights)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            if (nodeWeights == null) throw new ArgumentNullException(nameof(nodeWeights));
            _numberOfNodes = numberOfNodes;
            _edges = edges;
            _nodeWeights = nodeWeights;
        }

        public IPath<long, double, long> GetPath(long @from, long to)
        {
            if (HasNegativeLoop)
                throw new InvalidOperationException("Graph has negative loop.");

            ISingleSourcePaths<long, double, long> dijkstraPath;
            if (_dijkstraPathsCache.TryGetValue(@from, out dijkstraPath) == false)
            {
                dijkstraPath = _dijkstraShortestPathSearcher.GetShortestPaths(_numberOfNodes, @from, ReweightedEdges);
                _dijkstraPathsCache[@from] = dijkstraPath;
            }

            return new JohnsonPath(dijkstraPath.GetPath(to), _nodeWeights);
        }

        private IValuedEdge<long, double>[] GetReweightedEdges()
        {
            return _edges
                .Select(e => ValuedEdge<long, double>.Directed(EqualityComparer<long>.Default, edge =>
                {
                    edge.End1 = e.End1;
                    edge.End2 = e.End2;
                    edge.Value = e.Value + _nodeWeights[e.End1] - _nodeWeights[e.End2];
                }))
                .Cast<IValuedEdge<long, double>>()
                .ToArray();
        }

        public bool HasNegativeLoop { get; set; }
    }

    internal class JohnsonPath : IPath<long, double, long>
    {
        public JohnsonPath(IPath<long, double, long> reweightedPath, IDictionary<long, double> nodeWeights)
        {
            if (reweightedPath == null) throw new ArgumentNullException("reweightedPath");
            if (nodeWeights == null) throw new ArgumentNullException("nodeWeights");
            From = reweightedPath.From;
            To = reweightedPath.To;
            Value = reweightedPath.Value - nodeWeights[From] + nodeWeights[To];
            Path = reweightedPath.Path;
        }

        public long From { get; private set; }
        public long To { get; private set; }
        public double Value { get; private set; }
        public IEnumerable<long> Path { get; private set; }
    }
}