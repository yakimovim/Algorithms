using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    /// <summary>
    /// Represents binary search tree.
    /// </summary>
    /// <typeparam name="TValue">Type of node values.</typeparam>
    public class BinarySearchTree<TValue> : IEnumerable<TValue>
    {
        private class ReplaceCommandFirstStep
        {
            private readonly BinarySearchTree<TValue> _tree;
            private readonly BinarySearchTreeNode<TValue> _nodeToReplace;

            public ReplaceCommandFirstStep([NotNull] BinarySearchTree<TValue> tree,
                [NotNull] BinarySearchTreeNode<TValue> nodeToReplace)
            {
                if (tree == null) throw new ArgumentNullException(nameof(tree));
                if (nodeToReplace == null) throw new ArgumentNullException(nameof(nodeToReplace));
                _tree = tree;
                _nodeToReplace = nodeToReplace;
            }

            public ReplaceCommandLastStep At([CanBeNull] BinarySearchTreeNode<TValue> parentOfNodeToReplace)
            {
                return new ReplaceCommandLastStep(_tree, _nodeToReplace, parentOfNodeToReplace);
            }
        }

        private class ReplaceCommandLastStep
        {
            private readonly BinarySearchTree<TValue> _tree;
            private readonly BinarySearchTreeNode<TValue> _nodeToReplace;
            private readonly BinarySearchTreeNode<TValue> _parentOfNodeToReplace;

            public ReplaceCommandLastStep([NotNull] BinarySearchTree<TValue> tree, [CanBeNull] BinarySearchTreeNode<TValue> nodeToReplace, BinarySearchTreeNode<TValue> parentOfNodeToReplace)
            {
                if (tree == null) throw new ArgumentNullException(nameof(tree));
                _tree = tree;
                _nodeToReplace = nodeToReplace;
                _parentOfNodeToReplace = parentOfNodeToReplace;
            }

            public void With([CanBeNull] BinarySearchTreeNode<TValue> node)
            {
                if (_parentOfNodeToReplace != null)
                {
                    if (_parentOfNodeToReplace.LeftChild == _nodeToReplace)
                        _parentOfNodeToReplace.LeftChild = node;
                    else
                        _parentOfNodeToReplace.RightChild = node;
                }
                else
                {
                    _tree._root = node;
                }
            }
        }

        [NotNull]
        private readonly IComparer<TValue> _comparer;
        [CanBeNull]
        private BinarySearchTreeNode<TValue> _root;

        public BinarySearchTree([NotNull] IComparer<TValue> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            _comparer = comparer;
        }

        public BinarySearchTree()
        {
            if(typeof(IComparable<TValue>).IsAssignableFrom(typeof(TValue)))
                _comparer = Comparer<TValue>.Default;
            else
                throw new InvalidOperationException($"Type {typeof(TValue).Name} is not comparable.");
        }

        /// <summary>
        /// Adds value to the tree.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(TValue value)
        {
            if(_root == null)
                _root = new BinarySearchTreeNode<TValue>(_comparer, value);
            else
                _root.Add(value);
        }

        /// <summary>
        /// Checks if the tree contains the value;
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True, if the tree contains the value. Otherwise, false.</returns>
        public bool Contains(TValue value)
        {
            var searchResult = FindWithParent(value);
            return searchResult.Item1 != null;
        }

        /// <summary>
        /// Finds node and its parent by value.
        /// </summary>
        /// <param name="value">Value of node.</param>
        /// <returns>Tuple of node and its parent node.</returns>
        private Tuple<BinarySearchTreeNode<TValue>, BinarySearchTreeNode<TValue>> FindWithParent(TValue value)
        {
            BinarySearchTreeNode<TValue> parent = null;
            BinarySearchTreeNode<TValue> node = _root;

            while (node != null)
            {
                var comparison = _comparer.Compare(value, node.Value);

                if(comparison == 0)
                    break;

                if (comparison < 0)
                {
                    parent = node;
                    node = node.LeftChild;
                }
                else
                {
                    parent = node;
                    node = node.RightChild;
                }
            }

            return Tuple.Create(node, parent);
        }
        
        /// <summary>
        /// Removes the value from the tree.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        /// <returns>True, if value was removed. Otherwise, false.</returns>
        public bool Remove(TValue value)
        {
            var searchResult = FindWithParent(value);

            var nodeToRemove = searchResult.Item1;
            var parentOfNodeToRemove = searchResult.Item2;

            if(nodeToRemove == null)
                return false;

            if (nodeToRemove.IsLeaf())
            {
                Replace(nodeToRemove).At(parentOfNodeToRemove).With(null);
            }
            else
            {
                if (nodeToRemove.RightChild == null)
                {
                    Replace(nodeToRemove).At(parentOfNodeToRemove).With(nodeToRemove.LeftChild);
                }
                else if (nodeToRemove.RightChild.LeftChild == null)
                {
                    Replace(nodeToRemove).At(parentOfNodeToRemove).With(nodeToRemove.RightChild);
                    nodeToRemove.RightChild.LeftChild = nodeToRemove.LeftChild;
                }
                else
                {
                    var leftmostNodeSearchresult = FindLeftmostChild(nodeToRemove.RightChild, nodeToRemove);
                    var leftmostNode = leftmostNodeSearchresult.Item1;
                    var parentOfLeftmostNode = leftmostNodeSearchresult.Item2;

                    Replace(leftmostNode).At(parentOfLeftmostNode).With(leftmostNode.RightChild);
                    Replace(nodeToRemove).At(parentOfNodeToRemove).With(leftmostNode);
                    leftmostNode.LeftChild = nodeToRemove.LeftChild;
                    leftmostNode.RightChild = nodeToRemove.RightChild;
                }
            }

            return true;
        }

        private Tuple<BinarySearchTreeNode<TValue>, BinarySearchTreeNode<TValue>> FindLeftmostChild(BinarySearchTreeNode<TValue> node, BinarySearchTreeNode<TValue> parent)
        {
            while (node.LeftChild != null)
            {
                parent = node;
                node = node.LeftChild;
            }

            return Tuple.Create(node, parent);
        }

        private ReplaceCommandFirstStep Replace(BinarySearchTreeNode<TValue> nodeToReplace)
        {
            return new ReplaceCommandFirstStep(this, nodeToReplace);
        }

        private void Visit(Action<TValue> action, BinarySearchTreeNode<TValue> node)
        {
            if(node == null)
                return;

            Visit(action, node.LeftChild);
            action(node.Value);
            Visit(action, node.RightChild);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            var values = new LinkedList<TValue>();

            Visit(value => values.AddLast(value), _root);

            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// Represents one node of binary search tree.
    /// </summary>
    /// <typeparam name="TValue">Type of node value.</typeparam>
    internal class BinarySearchTreeNode<TValue> : IBinaryTreeNode<TValue>
    {
        private readonly IComparer<TValue> _comparer;

        /// <summary>
        /// Gets value of the node.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Gets or sets left child of this node.
        /// </summary>
        [CanBeNull]
        public BinarySearchTreeNode<TValue> LeftChild { get; set; }

        /// <summary>
        /// Gets or sets right child of this node.
        /// </summary>
        [CanBeNull]
        public BinarySearchTreeNode<TValue> RightChild { get; set; }

        IBinaryTreeNode<TValue> IBinaryTreeNode<TValue>.LeftChild => LeftChild;

        IBinaryTreeNode<TValue> IBinaryTreeNode<TValue>.RightChild => RightChild;

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

    /// <summary>
    /// Contains extension methods for Binary Search Tree.
    /// </summary>
    public static class BinarySearchTreeExtensions
    {
        /// <summary>
        /// Adds several values into the binary search tree.
        /// </summary>
        /// <typeparam name="TValue">Type of values</typeparam>
        /// <param name="tree">Binary search tree.</param>
        /// <param name="values">Values to add.</param>
        public static void AddRange<TValue>(this BinarySearchTree<TValue> tree, params TValue[] values)
        {
            AddRange(tree, (IEnumerable<TValue>) values);
        }

        /// <summary>
        /// Adds several values into the binary search tree.
        /// </summary>
        /// <typeparam name="TValue">Type of values</typeparam>
        /// <param name="tree">Binary search tree.</param>
        /// <param name="values">Values to add.</param>
        public static void AddRange<TValue>(this BinarySearchTree<TValue> tree, IEnumerable<TValue> values)
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    tree.Add(value);
                }
            }
        }
    }
}