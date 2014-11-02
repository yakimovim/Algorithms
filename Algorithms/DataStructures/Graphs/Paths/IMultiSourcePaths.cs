namespace EdlinSoftware.DataStructures.Graphs.Paths
{
    /// <summary>
    /// Represents paths from any node to any node of graph.
    /// </summary>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    /// <typeparam name="TPathValue">Type of path value.</typeparam>
    /// <typeparam name="TPathElement">Type of path elements.</typeparam>
    public interface IMultiSourcePaths<TNode, out TPathValue, out TPathElement>
    {
        /// <summary>
        /// Returns path to some node.
        /// </summary>
        /// <param name="from">Source node.</param>
        /// <param name="to">Destination node.</param>
        IPath<TNode, TPathValue, TPathElement> GetPath(TNode from, TNode to);
    }

    /// <summary>
    /// Represents paths from any node to any node of graph in absence of negative cost loop.
    /// </summary>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    /// <typeparam name="TPathValue">Type of path value.</typeparam>
    /// <typeparam name="TPathElement">Type of path elements.</typeparam>
    public interface IMultiSourcePathsWithoutNegativeLoop<TNode, out TPathValue, out TPathElement>
        : IMultiSourcePaths<TNode, TPathValue, TPathElement>
    {
        /// <summary>
        /// Gets if graph has negative loop.
        /// </summary>
        bool HasNegativeLoop { get; }
    }
}