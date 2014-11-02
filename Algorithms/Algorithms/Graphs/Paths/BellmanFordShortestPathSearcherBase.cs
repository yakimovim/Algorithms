using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Paths;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    public abstract class BellmanFordShortestPathSearcherBase : ISingleSourceShortestPathSearcher
    {
        protected class CorrectPathElement
        {
            public double CorrectPathValue { get; set; }

            public long? LastCorrectPathNode { get; set; }
        }

        protected ILookup<long, IValuedEdge<long, double>> InputEdges;

        protected double[,] ShortestPaths;
        protected long?[,] PreviousNodeInShortestPath;

        ISingleSourcePaths<long, double, long> ISingleSourceShortestPathSearcher.GetShortestPaths(long numberOfNodes, long initialNode, params IValuedEdge<long, double>[] edges)
        {
            return GetShortestPaths(numberOfNodes, initialNode, edges);
        }

        public virtual ISingleSourcePathsWithoutNegativeLoop<long, double, long> GetShortestPaths(long numberOfNodes, long initialNode, params IValuedEdge<long, double>[] edges)
        {
            if (numberOfNodes <= 0) throw new ArgumentOutOfRangeException("numberOfNodes");
            if (initialNode < 0 || initialNode >= numberOfNodes) throw new ArgumentOutOfRangeException("initialNode");

            InitializePathArrays(numberOfNodes, initialNode);

            PrepareEdgesStorage(edges);

            var sliceIndex = FindAllPaths();

            var hasNegativeCircle = CheckForNegativeCircle(sliceIndex);

            return new BellmanFordShortestPaths(initialNode, ShortestPaths, PreviousNodeInShortestPath, sliceIndex)
            {
                HasNegativeLoop = hasNegativeCircle
            };
        }

        private void InitializePathArrays(long numberOfNodes, long initialNode)
        {
            ShortestPaths = new double[numberOfNodes, 2];
            PreviousNodeInShortestPath = new long?[numberOfNodes, 2];

            for (long i = 0; i < numberOfNodes; i++)
            {
                if (i == initialNode)
                    ShortestPaths[i, 0] = 0;
                else
                    ShortestPaths[i, 0] = double.PositiveInfinity;
            }
        }

        private void PrepareEdgesStorage(IEnumerable<IValuedEdge<long, double>> edges)
        {
            InputEdges = edges.ToLookup(e => e.End2);
        }

        protected abstract int FindAllPaths();

        protected abstract bool CheckForNegativeCircle(int sliceIndex);

        protected void Swap(ref int previousStepIndex, ref int currentStepIndex)
        {
            var temp = currentStepIndex;
            currentStepIndex = previousStepIndex;
            previousStepIndex = temp;
        }
    }

    internal class BellmanFordShortestPaths : ISingleSourcePathsWithoutNegativeLoop<long, double, long>
    {
        private readonly double[,] _pathMatrix;
        private readonly long?[,] _previousNodes;
        private readonly int _sliceIndex;
        private readonly long _initialNode;

        public bool HasNegativeLoop { get; set; }

        public BellmanFordShortestPaths(long initialNode, double[,] pathMatrix, long?[,] previousNodes, int sliceIndex)
        {
            if (pathMatrix == null) throw new ArgumentNullException("pathMatrix");
            if (previousNodes == null) throw new ArgumentNullException("previousNodes");
            _pathMatrix = pathMatrix;
            _previousNodes = previousNodes;
            _sliceIndex = sliceIndex;
            _initialNode = initialNode;
        }

        public IPath<long, double, long> GetPath(long to)
        {
            if (HasNegativeLoop) throw new InvalidOperationException();

            return new ShortestPath<long, double, long>
            {
                From = _initialNode,
                To = to,
                Value = _pathMatrix[to, _sliceIndex],
                Path = GetPathTo(to)
            };
        }

        private IEnumerable<long> GetPathTo(long to)
        {
            if (to == _initialNode)
                return new long[] { _initialNode };
            if (_previousNodes[to, _sliceIndex].HasValue == false)
                return new long[0];

            var path = new List<long> { to };

            while (_previousNodes[to, _sliceIndex].Value != _initialNode)
            {
                path.Insert(0, _previousNodes[to, _sliceIndex].Value);
                to = _previousNodes[to, _sliceIndex].Value;
            }

            path.Insert(0, _initialNode);

            return path;
        }
    }
}
