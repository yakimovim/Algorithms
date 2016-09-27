using System;
using System.Collections.Generic;
using System.Linq;
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
            if (node == null)
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

        /// <summary>
        /// Gets string presentation of node.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <returns>Array of strings of equal length with string presentation of sub-tree.</returns>
        public static string[] GetStringPresentation<TValue>(this IBinaryTreeNode<TValue> node)
        {
            if (node == null)
                return new string[0];

            var leftPresentation = GetStringPresentation(node.LeftChild);
            var rightPresentation = GetStringPresentation(node.RightChild);

            var leftWidth = leftPresentation.Length > 0 ? leftPresentation[0].Length : 0;
            var rightWidth = rightPresentation.Length > 0 ? rightPresentation[0].Length : 0;

            var myPresentation = node.Value?.ToString() ?? "";
            var myWidth = myPresentation.Length;

            if (rightWidth == 0 && leftWidth == 0)
                return new[] {myPresentation};

            var middlePadding = Math.Max(3, myWidth);
            var totalWidth = Math.Max(leftWidth + middlePadding + rightWidth, myWidth);
            middlePadding = totalWidth - leftWidth - rightWidth;

            int leftPadding, rightPadding;
            if (leftWidth == 0)
            {
                leftPadding = 0;
                rightPadding = totalWidth - myWidth;
            }
            else if (rightWidth == 0)
            {
                leftPadding = totalWidth - myWidth;
                rightPadding = 0;
            }
            else
            {
                leftPadding = (totalWidth - myWidth) / 2;
                rightPadding = totalWidth - myWidth - leftPadding;
            }


            var presentation = new LinkedList<string>();
            presentation.AddLast(new string(' ', leftPadding) + myPresentation + new string(' ', rightPadding));

            var size = Math.Max(leftPresentation.Length, rightPresentation.Length);
            for (int i = 0; i < size; i++)
            {
                var value = (i < leftPresentation.Length)
                    ? leftPresentation[i]
                    : new string(' ', leftWidth);
                value += new string(' ', middlePadding);
                value += (i < rightPresentation.Length)
                    ? rightPresentation[i]
                    : new string(' ', rightWidth);
                presentation.AddLast(value);
            }

            return presentation.ToArray();
        }
    }
}
