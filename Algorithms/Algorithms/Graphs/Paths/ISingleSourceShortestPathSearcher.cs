using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Paths;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    /// <summary>
    /// Represents searcher of shortest paths from single node.
    /// </summary>
    public interface ISingleSourceShortestPathSearcher
    {
        /// <summary>
        /// Returns shortest paths from single node.
        /// </summary>
        /// <param name="numberOfNodes">Number of nodes in graph.</param>
        /// <param name="initialNode">Source node.</param>
        /// <param name="edges">Directed edges of graph.</param>
        ISingleSourcePaths<long, double, long> GetShortestPaths(long numberOfNodes, long initialNode, params IValuedEdge<long, double>[] edges);
    }
}
