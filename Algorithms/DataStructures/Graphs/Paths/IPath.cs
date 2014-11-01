using System.Collections.Generic;

namespace EdlinSoftware.DataStructures.Graphs.Paths
{
    /// <summary>
    /// Represents single-source paths to nodes of graph.
    /// </summary>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    /// <typeparam name="TPathValue">Type of path value.</typeparam>
    /// <typeparam name="TPathElement">Type of path elements.</typeparam>
    public interface ISingleSourcePaths<TNode, TPathValue, TPathElement>
    {
        /// <summary>
        /// Returns path to some node.
        /// </summary>
        /// <param name="to">Destination node.</param>
        IPath<TNode, TPathValue, TPathElement> GetPath(TNode to);
    }

    /// <summary>
    /// Represents single-source paths to nodes of graph in absence of negative cost loop.
    /// </summary>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    /// <typeparam name="TPathValue">Type of path value.</typeparam>
    /// <typeparam name="TPathElement">Type of path elements.</typeparam>
    public interface ISingleSourcePathsWithoutNegativeLoop<TNode, TPathValue, TPathElement>
        : ISingleSourcePaths<TNode, TPathValue, TPathElement>
    {
        /// <summary>
        /// Gets if graph has negative loop.
        /// </summary>
        bool HasNegativeLoop { get; }
    }

    /// <summary>
    /// Represents path from one node to another.
    /// </summary>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    /// <typeparam name="TPathValue">Type of path value.</typeparam>
    /// <typeparam name="TPathElement">Type of path elements.</typeparam>
    public interface IPath<TNode, TPathValue, TPathElement>
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
