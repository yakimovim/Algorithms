using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    /// <summary>
    /// Represents binary search tree.
    /// </summary>
    /// <typeparam name="TValue">Type of node values.</typeparam>
    public class BinarySearchTree<TValue> : BinarySearchTreeBase<TValue>
    {
        [CanBeNull]
        private BinarySearchTreeNode<TValue> _root;

        internal BinarySearchTreeNode<TValue> Root => _root;

        public BinarySearchTree([NotNull] IComparer<TValue> comparer)
            : base(comparer)
        { }

        public BinarySearchTree()
        { }

        /// <summary>
        /// Adds value to the tree.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public override void Add(TValue value)
        {
            if (_root == null)
                _root = new BinarySearchTreeNode<TValue>(Comparer, value);
            else
                _root.Add(value);
        }

        /// <summary>
        /// Checks if the tree contains the value;
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True, if the tree contains the value. Otherwise, false.</returns>
        public override bool Contains(TValue value)
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
                var comparison = Comparer.Compare(value, node.Value);

                if (comparison == 0)
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
        public override bool Remove(TValue value)
        {
            var searchResult = FindWithParent(value);

            var nodeToRemove = searchResult.Item1;
            var parentOfNodeToRemove = searchResult.Item2;

            if (nodeToRemove == null)
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
                    var leftmostNodeSearchresult = FindLeftmostChildOf(nodeToRemove.RightChild, nodeToRemove);
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

        private Tuple<BinarySearchTreeNode<TValue>, BinarySearchTreeNode<TValue>> FindLeftmostChildOf(BinarySearchTreeNode<TValue> node, BinarySearchTreeNode<TValue> parent)
        {
            while (node.LeftChild != null)
            {
                parent = node;
                node = node.LeftChild;
            }

            return Tuple.Create(node, parent);
        }

        private ReplaceCommandFirstStep<BinarySearchTreeNode<TValue>> Replace(BinarySearchTreeNode<TValue> nodeToReplace)
        {
            return new ReplaceCommandFirstStep<BinarySearchTreeNode<TValue>>(SetRoot, nodeToReplace);
        }

        private void SetRoot(BinarySearchTreeNode<TValue> node)
        {
            _root = node;
        }

        public override IEnumerator<TValue> GetEnumerator()
        {
            var values = new LinkedList<TValue>();

            _root.VisitInOrder<TValue, BinarySearchTreeNode<TValue>>(value => values.AddLast(value));

            return values.GetEnumerator();
        }

        public override string ToString()
        {
            return _root?.ToString() ?? "";
        }
    }
}