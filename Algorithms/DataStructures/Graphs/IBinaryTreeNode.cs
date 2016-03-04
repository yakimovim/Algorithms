using System;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs
{
    /// <summary>
    /// Represents one node of binary tree.
    /// </summary>
    /// <typeparam name="TValue">Type of value stored in the node.</typeparam>
    public interface IBinaryTreeNode<out TValue>
    {
        /// <summary>
        /// Gets value of the node.
        /// </summary>
        TValue Value { get; }
        /// <summary>
        /// Gets left child of the node. Can be null.
        /// </summary>
        [CanBeNull]
        IBinaryTreeNode<TValue> LeftChild { get; }
        /// <summary>
        /// Gets right child of the node. Can be null.
        /// </summary>
        [CanBeNull]
        IBinaryTreeNode<TValue> RightChild { get; }
    }

    /// <summary>
    /// Base class for implementation of binary tree nodes.
    /// </summary>
    /// <typeparam name="TNode">Type of binary tree node.</typeparam>
    /// <typeparam name="TValue">Type of value stored in the node.</typeparam>
    public abstract class BinaryTreeNodeBase<TNode, TValue> : IBinaryTreeNode<TValue>
        where TNode : BinaryTreeNodeBase<TNode, TValue>
    {
        /// <summary>
        /// Gets or sets value of the node.
        /// </summary>
        public TValue Value { get; protected set; }

        /// <summary>
        /// Gets or sets left child of the node. Can be null.
        /// </summary>
        [CanBeNull]
        public virtual TNode LeftChild { get; set; }
        /// <summary>
        /// Gets or sets right child of the node. Can be null.
        /// </summary>
        [CanBeNull]
        public virtual TNode RightChild { get; set; }

        IBinaryTreeNode<TValue> IBinaryTreeNode<TValue>.LeftChild => LeftChild;

        IBinaryTreeNode<TValue> IBinaryTreeNode<TValue>.RightChild => RightChild;
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

        /// <summary>
        /// Returns height of binary tree node.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <returns>Height of binary tree node. 0 if node is null.</returns>
        public static int GetHeight<TValue>(this IBinaryTreeNode<TValue> node)
        {
            if (node == null)
                return 0;

            return 1 + Math.Max(GetHeight(node.LeftChild), GetHeight(node.RightChild));
        }

        /// <summary>
        /// Visits first left children, then itself, then right children.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <param name="action">Action for each node.</param>
        public static void VisitInOrder<TValue>(this IBinaryTreeNode<TValue> node, Action<TValue> action)
        {
            if(node == null)
                return;

            node.LeftChild.VisitInOrder(action);
            action(node.Value);
            node.RightChild.VisitInOrder(action);
        }

        /// <summary>
        /// Visits first itself, then left children, then right children.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <param name="action">Action for each node.</param>
        public static void VisitPreOrder<TValue>(this IBinaryTreeNode<TValue> node, Action<TValue> action)
        {
            if (node == null)
                return;

            action(node.Value);
            node.LeftChild.VisitInOrder(action);
            node.RightChild.VisitInOrder(action);
        }

        /// <summary>
        /// Visits first left children, then right children, then itself.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <param name="action">Action for each node.</param>
        public static void VisitPostOrder<TValue>(this IBinaryTreeNode<TValue> node, Action<TValue> action)
        {
            if (node == null)
                return;

            node.LeftChild.VisitInOrder(action);
            node.RightChild.VisitInOrder(action);
            action(node.Value);
        }
    }
}
