using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Paths;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    /// <summary>
    /// Represents searcher of shortest paths from any node to any other node.
    /// </summary>
    public interface IMultiSourceShortestPathSearcher
    {
        /// <summary>
        /// Returns shortest paths from any node to any other node.
        /// </summary>
        /// <param name="numberOfNodes">Number of nodes in graph.</param>
        /// <param name="edges">Directed edges of graph.</param>
        IMultiSourcePaths<long, double, long> GetShortestPaths(long numberOfNodes, params IValuedEdge<long, double>[] edges);
    }

    /// <summary>
    /// Represents searcher of shortest paths from any node to any other node without negative loops.
    /// </summary>
    public interface IMultiSourceShortestPathWithoutNegativeLoopSearcher
    {
        /// <summary>
        /// Returns shortest paths from any node to any other node without negative loops.
        /// </summary>
        /// <param name="numberOfNodes">Number of nodes in graph.</param>
        /// <param name="edges">Directed edges of graph.</param>
        IMultiSourcePathsWithoutNegativeLoop<long, double, long> GetShortestPaths(long numberOfNodes, params IValuedEdge<long, double>[] edges);
    }
}