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
            if (_comparer.Compare(value, Value) < 0)
            {
                if (LeftChild == null)
                {
                    LeftChild = new BinarySearchTreeNode<TValue>(_comparer, value);
                }
                else
                {
                    LeftChild.Add(value);
                }
            }
            else
            {
                if (RightChild == null)
                {
                    RightChild = new BinarySearchTreeNode<TValue>(_comparer, value);
                }
                else
                {
                    RightChild.Add(value);
                }
            }
        }
    }
}