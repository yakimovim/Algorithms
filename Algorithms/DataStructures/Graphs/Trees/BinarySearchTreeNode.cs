using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    /// <summary>
    /// Represents one node of binary search tree.
    /// </summary>
    /// <typeparam name="TValue">Type of node value.</typeparam>
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
}