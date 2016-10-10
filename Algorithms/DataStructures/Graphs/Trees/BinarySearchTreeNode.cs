using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using EdlinSoftware.Algorithms.Visualizers;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    /// <summary>
    /// Represents one node of binary search tree.
    /// </summary>
    /// <typeparam name="TValue">Type of node value.</typeparam>
    [DebuggerTypeProxy(typeof(BinaryTreeNodeDebuggerProxy))]
    internal class BinarySearchTreeNode<TValue> : BinaryTreeNodeBase<BinarySearchTreeNode<TValue> ,TValue>
    {
        private readonly IComparer<TValue> _comparer;

        public BinarySearchTreeNode([NotNull] IComparer<TValue> comparer, TValue value)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            _comparer = comparer;
            Value = value;
        }

        /// <summary>
        /// Adds value to the tree with this node as a root.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(TValue value)
        {
            var node = this;

            while (true)
            {
                var nodeValue = node.Value;

                if (_comparer.Compare(value, nodeValue) < 0)
                {
                    if (node.LeftChild == null)
                    {
                        node.LeftChild = new BinarySearchTreeNode<TValue>(_comparer, value);
                        return;
                    }

                    node = node.LeftChild;
                }
                else
                {
                    if (node.RightChild == null)
                    {
                        node.RightChild = new BinarySearchTreeNode<TValue>(_comparer, value);
                        return;
                    }

                    node = node.RightChild;
                }
            }

        }
    }


    public class BinaryTreeNodeDebuggerProxy
    {
        public readonly GraphNode Node;

        public BinaryTreeNodeDebuggerProxy([NotNull] object node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            Node = BuildNode(node);
        }

        private GraphNode BuildNode(object node)
        {
            if (node == null)
                return null;

            var leftChild = node.GetType().GetProperty("LeftChild", BindingFlags.Instance | BindingFlags.Public).GetValue(node);
            var rightChild = node.GetType().GetProperty("RightChild", BindingFlags.Instance | BindingFlags.Public).GetValue(node);

            var result = new GraphNode
            {
                Content = node.GetType()
                        .GetProperty("Value", BindingFlags.Instance | BindingFlags.Public)
                        .GetValue(node)?.ToString()
            };

            var children = new List<GraphEdge>();
            result.Edges = children;

            if (rightChild != null)
                children.Add(new GraphEdge
                {
                    IsDirected = true,
                    From = result,
                    To = BuildNode(rightChild)
                });
            if (leftChild != null)
                children.Add(new GraphEdge
                {
                    IsDirected = true,
                    From = result,
                    To = BuildNode(leftChild)
                });

            return result;
        }
    }
}