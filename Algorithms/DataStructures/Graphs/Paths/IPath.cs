using System.Collections.Generic;

namespace EdlinSoftware.DataStructures.Graphs.Paths
{
    /// <summary>
    /// Represents path from one node to another.
    /// </summary>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    /// <typeparam name="TPathValue">Type of path value.</typeparam>
    /// <typeparam name="TPathElement">Type of path elements.</typeparam>
    public interface IPath<out TNode, out TPathValue, out TPathElement>
    {
        /// <summary>
        /// Source node.
        /// </summary>
        TNode From { get; }
        /// <summary>
        /// Destination node.
        /// </summary>
        TNode To { get; }
        /// <summary>
        /// Value of the path.
        /// </summary>
        TPathValue Value { get; }
        /// <summary>
        /// Elements of the path.
        /// </summary>
        IEnumerable<TPathElement> Path { get; }
    }
}
