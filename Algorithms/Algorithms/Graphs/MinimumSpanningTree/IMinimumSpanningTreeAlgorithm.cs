using System.Collections.Generic;
using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Graphs.MinimumSpanningTree
{
    /// <summary>
    /// Represents algorithm searching minimum spanning tree for a graph.
    /// </summary>
    public interface IMinimumSpanningTreeAlgorithm
    {
        /// <summary>
        /// Returns collection of edges in the graph.
        /// </summary>
        /// <param name="numberOfNodesInGraph">Number of nodes in the graph.</param>
        /// <param name="edgesOfGraph">Collection of edges in the graph. Nodes are numerated from 1 to <see cref="numberOfNodesInGraph"/></param>
        IEnumerable<IValuedEdge<long, long>> GetMinimumSpanningTree(long numberOfNodesInGraph, IEnumerable<IValuedEdge<long, long>> edgesOfGraph);
    }

    public static class MinimumSpanningTreeAlgorithmExtender
    {
        /// <summary>
        /// Returns collection of edges in the graph.
        /// </summary>
        /// <param name="mstSearcher">Minimum spanning tree searcher.</param>
        /// <param name="numberOfNodesInGraph">Number of nodes in the graph.</param>
        /// <param name="edgesOfGraph">Collection of edges in the graph. Nodes are numerated from 1 to <see cref="numberOfNodesInGraph"/></param>
        public static IEnumerable<IValuedEdge<long, long>> GetMinimumSpanningTree(this IMinimumSpanningTreeAlgorithm mstSearcher, long numberOfNodesInGraph, params IValuedEdge<long, long>[] edgesOfGraph)
        {
            return mstSearcher.GetMinimumSpanningTree(numberOfNodesInGraph, edgesOfGraph);
        }
    }
}
