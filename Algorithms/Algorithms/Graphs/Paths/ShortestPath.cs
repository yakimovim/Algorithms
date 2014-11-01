using System.Collections.Generic;
using EdlinSoftware.DataStructures.Graphs.Paths;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    /// <summary>
    /// Represents shortest path between two nodes.
    /// </summary>
    /// <typeparam name="TNode">Type of node.</typeparam>
    /// <typeparam name="TPathValue">Type of value of path.</typeparam>
    /// <typeparam name="TPathElement">Type of elements of path.</typeparam>
    internal class ShortestPath<TNode, TPathValue, TPathElement> : IPath<TNode, TPathValue, TPathElement>
    {
        /// <summary>
        /// Source node.
        /// </summary>
        public TNode From { get; set; }

        /// <summary>
        /// Destination node.
        /// </summary>
        public TNode To { get; set; }

        /// <summary>
        /// Value of the path.
        /// </summary>
        public TPathValue Value { get; set; }

        /// <summary>
        /// Elements of the path.
        /// </summary>
        public IEnumerable<TPathElement> Path { get; set; }
    }
}
