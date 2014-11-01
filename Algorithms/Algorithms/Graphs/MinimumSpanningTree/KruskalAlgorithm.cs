using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.UnionFind;

namespace EdlinSoftware.Algorithms.Graphs.MinimumSpanningTree
{
    /// <summary>
    /// Represents Kruskal's algorithm of minimum spanning tree search.
    /// </summary>
    public class KruskalAlgorithm : IMinimumSpanningTreeAlgorithm
    {
        private UnionFind<long> _unionFind;
        private IUnionFindElement<long>[] _nodes;
        private IList<IValuedEdge<long, long>> _orderedEdges;

        public IEnumerable<IValuedEdge<long, long>> GetMinimumSpanningTree(long numberOfNodesInGraph, IEnumerable<IValuedEdge<long, long>> edgesOfGraph)
        {
            _unionFind = new UnionFind<long>();

            for (long i = 0; i < numberOfNodesInGraph; i++)
            { _unionFind.Add(i); }

            _nodes = _unionFind.Elements.ToArray();

            _orderedEdges = edgesOfGraph == null
                ? new List<IValuedEdge<long, long>>()
                : edgesOfGraph.OrderBy(e => e.Value).ToList();

            return GetMinimumSpanningTree();
        }

        private IEnumerable<IValuedEdge<long, long>> GetMinimumSpanningTree()
        {
            var minimumSpanningTree = new List<IValuedEdge<long, long>>();

            foreach (var edge in _orderedEdges)
            {
                if (!CreatesLoopInSpanningTree(edge))
                {
                    minimumSpanningTree.Add(edge);
                    MergeClusters(edge);
                }
            }

            return minimumSpanningTree;
        }

        private bool CreatesLoopInSpanningTree(IValuedEdge<long, long> edge)
        {
            return ReferenceEquals(_nodes[edge.End1 - 1].Group, _nodes[edge.End2 - 1].Group);
        }

        private void MergeClusters(IValuedEdge<long, long> edge)
        {
            _unionFind.Union(_nodes[edge.End1 - 1], _nodes[edge.End2 - 1]);
        }
    }
}
