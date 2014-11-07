using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Paths;
using EdlinSoftware.DataStructures.Heaps;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    public class DijkstraShortestPathSearcher : ISingleSourceShortestPathSearcher
    {
        private class HeapKey
        {
            public double DijkstraCriterion { get; set; }

            public long PreviousNode { get; set; }
        }

        private class HeapKeyComparer : IComparer<HeapKey>
        {
            public int Compare(HeapKey x, HeapKey y)
            {
                return x.DijkstraCriterion.CompareTo(y.DijkstraCriterion);
            }
        }

        private readonly Dictionary<long, IList<IValuedEdge<long, double>>> _forwardEdges = new Dictionary<long, IList<IValuedEdge<long, double>>>();
        private readonly Dictionary<long, IList<IValuedEdge<long, double>>> _backwardEdges = new Dictionary<long, IList<IValuedEdge<long, double>>>();
        private readonly HashSet<long> _knownSet = new HashSet<long>();

        private double[] _shortestPaths;
        private long?[] _previousNodeInShortestPath;

        public ISingleSourcePaths<long, double, long> GetShortestPaths(long numberOfNodes, long initialNode, params IValuedEdge<long, double>[] edges)
        {
            if (numberOfNodes <= 0) throw new ArgumentOutOfRangeException("numberOfNodes");
            if (initialNode < 0 || initialNode >= numberOfNodes) throw new ArgumentOutOfRangeException("initialNode");

            InitializePathsArrays(numberOfNodes, initialNode);

            PrepareEdgesStorage(edges);

            _knownSet.Clear();
            _knownSet.Add(initialNode);

            var nodesHeap = GetNodesHeap(initialNode);

            while (nodesHeap.Count > 0)
            {
                var nextNodeElement = nodesHeap.ExtractMinElement();

                var nextNode = nextNodeElement.Value;
                var dijkstraCriterion = nextNodeElement.Key.DijkstraCriterion;
                var previousNode = nextNodeElement.Key.PreviousNode;

                _shortestPaths[nextNode] = dijkstraCriterion;
                _previousNodeInShortestPath[nextNode] = previousNode;
                _knownSet.Add(nextNode);

                RecalculateHeap(nodesHeap, nextNode);
            }

            return new DijkstraShortestPaths(initialNode, _shortestPaths, _previousNodeInShortestPath);
        }

        private void InitializePathsArrays(long numberOfNodes, long initialNode)
        {
            _shortestPaths = new double[numberOfNodes];
            _previousNodeInShortestPath = new long?[numberOfNodes];

            for (long i = 0; i < numberOfNodes; i++)
            {
                if (i == initialNode)
                {
                    _shortestPaths[i] = 0.0;
                }
                else
                {
                    _shortestPaths[i] = double.PositiveInfinity;
                }
            }
        }

        private void PrepareEdgesStorage(IValuedEdge<long, double>[] edges)
        {
            _forwardEdges.Clear();
            _backwardEdges.Clear();

            foreach (var edge in edges)
            {
                if (_forwardEdges.ContainsKey(edge.End1) == false)
                { _forwardEdges[edge.End1] = new List<IValuedEdge<long, double>>(); }
                _forwardEdges[edge.End1].Add(edge);
                if (_backwardEdges.ContainsKey(edge.End2) == false)
                { _backwardEdges[edge.End2] = new List<IValuedEdge<long, double>>(); }
                _backwardEdges[edge.End2].Add(edge);
            }
        }

        private UniqueHeap<HeapKey, long> GetNodesHeap(long initialNode)
        {
            var heap = new UniqueHeap<HeapKey, long>(new HeapKeyComparer());

            if (_forwardEdges.ContainsKey(initialNode))
            {
                foreach (var node in _forwardEdges[initialNode].Select(e => e.End2))
                {
                    AddToHeap(heap, node);
                }
            }

            return heap;
        }

        private void AddToHeap(UniqueHeap<HeapKey, long> heap, long node)
        {
            if (_backwardEdges.ContainsKey(node))
            {
                var backwardEdges = _backwardEdges[node].Where(e => _knownSet.Contains(e.End1)).ToArray();

                double minDijkstraCriterion = double.PositiveInfinity;
                long previousNode = -1;

                foreach (var edge in backwardEdges)
                {
                    var dijkstraCriterion = _shortestPaths[edge.End1] + edge.Value;

                    if (dijkstraCriterion < minDijkstraCriterion)
                    {
                        minDijkstraCriterion = dijkstraCriterion;
                        previousNode = edge.End1;
                    }
                }

                heap.Add(new HeapKey { DijkstraCriterion = minDijkstraCriterion, PreviousNode = previousNode }, node);
            }
        }

        private void RecalculateHeap(UniqueHeap<HeapKey, long> heap, long node)
        {
            if (_forwardEdges.ContainsKey(node))
            {
                var forwardNodes = _forwardEdges[node]
                    .Where(e => !_knownSet.Contains(e.End2))
                    .Select(e => e.End2)
                    .ToArray();

                foreach (var forwardNode in forwardNodes)
                {
                    heap.Remove(forwardNode);
                }

                foreach (var forwardNode in forwardNodes)
                {
                    AddToHeap(heap, forwardNode);
                }
            }
        }
    }

    internal class DijkstraShortestPaths : ISingleSourcePaths<long, double, long>
    {
        private readonly long _initialNode;
        private readonly double[] _pathValues;
        private readonly long?[] _previousNodes;

        public DijkstraShortestPaths(long initialNode, double[] pathValues, long?[] previousNodes)
        {
            _initialNode = initialNode;
            _pathValues = pathValues;
            _previousNodes = previousNodes;
        }

        public virtual IPath<long, double, long> GetPath(long to)
        {
            if (to < 0 || to >= _pathValues.LongLength)
            { throw new ArgumentOutOfRangeException("to"); }

            return new ShortestPath<long, double, long>
            {
                From = _initialNode,
                To = to,
                Value = _pathValues[to],
                Path = GetPathTo(to)
            };
        }

        private IEnumerable<long> GetPathTo(long to)
        {
            if (to == _initialNode)
                return new[] { _initialNode };
            if (_previousNodes[to].HasValue == false)
                return new long[0];

            var path = new List<long> { to };

            while (true)
            {
                if (_previousNodes[to].HasValue == false)
                { throw new InvalidOperationException("Incorrect path sequence."); }
                var previousNode = _previousNodes[to].Value;

                if (previousNode == _initialNode)
                { break; }

                path.Insert(0, previousNode);
                to = previousNode;
            }

            path.Insert(0, _initialNode);

            return path;
        }
    }
}
