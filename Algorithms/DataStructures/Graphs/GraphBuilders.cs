using System;
using System.Linq;

namespace EdlinSoftware.DataStructures.Graphs
{
    /// <summary>
    /// Contains builders of different graphs.
    /// </summary>
    public static class GraphBuilders
    {
        /// <summary>
        /// Creates complete graph.
        /// </summary>
        /// <typeparam name="TEdge">Type of graph edges.</typeparam>
        /// <param name="numberOfNodes">Number of nodes in graph.</param>
        /// <param name="edgeProvider">Generator of edges.</param>
        public static Graph<TEdge> GetCompleteGraph<TEdge>(long numberOfNodes, Func<long, long, TEdge> edgeProvider)
            where TEdge : IEdge<long>
        {
            if (edgeProvider == null) throw new ArgumentNullException(nameof(edgeProvider));

            var graph = new Graph<TEdge>(numberOfNodes);

            for (long sourseNode = 0; sourseNode < numberOfNodes; sourseNode++)
            {
                for (long targetNode = 0; targetNode < numberOfNodes; targetNode++)
                {
                    if (sourseNode == targetNode) continue;

                    if (graph.GetEdgesBetween(sourseNode, targetNode).Any()) continue;

                    var edge = edgeProvider(sourseNode, targetNode);

                    graph.AddEdge(edge);
                }
            }

            return graph;
        }

        /// <summary>
        /// Creates complete graph with undirected edges.
        /// </summary>
        /// <param name="numberOfNodes">Number of nodes in graph.</param>
        public static Graph<UndirectedEdge<long>> GetCompleteGraph(long numberOfNodes)
        {
            return GetCompleteGraph(numberOfNodes, (node1, node2) => new UndirectedEdge<long> { End1 = node1, End2 = node2 });
        }

    }
}
