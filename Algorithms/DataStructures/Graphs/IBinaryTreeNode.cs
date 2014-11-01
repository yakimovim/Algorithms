
namespace EdlinSoftware.DataStructures.Graphs
{
    /// <summary>
    /// Represents one node of binary tree.
    /// </summary>
    /// <typeparam name="TValue">Type of value stored in the node.</typeparam>
    public interface IBinaryTreeNode<TValue>
    {
        /// <summary>
        /// Gets value of the node.
        /// </summary>
        TValue Value { get; }
        /// <summary>
        /// Gets left child of the node. Can be null.
        /// </summary>
        IBinaryTreeNode<TValue> LeftChild { get; }
        /// <summary>
        /// Gets right child of the node. Can be null.
        /// </summary>
        IBinaryTreeNode<TValue> RightChild { get; }
    }

    /// <summary>
    /// Provides extension methods for binary tree nodes.
    /// </summary>
    public static class BinaryTreeNodeExtender
    {
        /// <summary>
        /// Determines if the node has no children.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <returns>True, if node has no children. False, otherwise.</returns>
        public static bool IsLeaf<TValue>(this IBinaryTreeNode<TValue> node)
        {
            return node.LeftChild == null && node.RightChild == null;
        }
    }
}
