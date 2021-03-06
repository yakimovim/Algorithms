﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    /// <summary>
    /// Contains methods for work with binary search tree.
    /// </summary>
    public static class BinarySearchTree
    {
        /// <summary>
        /// Merges two trees into existing root.
        /// </summary>
        /// <typeparam name="TValue">Type of node values.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="leftTreeRoot">Root of left tree.</param>
        /// <param name="rightTreeRoot">Root of right tree.</param>
        /// <param name="mergedTreeRoot">Root of merged tree.</param>
        internal static void MergeWithRoot<TValue, TNode>(TNode leftTreeRoot, TNode rightTreeRoot,
            [NotNull] TNode mergedTreeRoot)
            where TNode : BinarySearchTreeNode<TValue>
        {
            if (mergedTreeRoot == null) throw new ArgumentNullException(nameof(mergedTreeRoot));

            mergedTreeRoot.LeftChild = leftTreeRoot;
            mergedTreeRoot.RightChild = rightTreeRoot;
        }

        /// <summary>
        /// Merges two trees.
        /// </summary>
        /// <typeparam name="TValue">Type of node values.</typeparam>
        /// <param name="leftTree">Left tree.</param>
        /// <param name="rightTree">Right tree.</param>
        /// <param name="comparer">Comparer of values.</param>
        public static BinarySearchTree<TValue> Merge<TValue>(
            BinarySearchTree<TValue> leftTree,
            BinarySearchTree<TValue> rightTree,
            [NotNull] IComparer<TValue> comparer)
        {
            if (leftTree?.Root == null)
                return rightTree;
            if (rightTree?.Root == null)
                return leftTree;
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var maxNode = leftTree.Root.FindNodeWithMaximalValue();
            leftTree.Remove(maxNode.Value);

            var mergedTreeRoot = new BinarySearchTreeNode<TValue>(comparer, maxNode.Value);
            MergeWithRoot<TValue, BinarySearchTreeNode<TValue>>(leftTree.Root, rightTree.Root, mergedTreeRoot);
            return new BinarySearchTree<TValue>(mergedTreeRoot, comparer);
        }

        /// <summary>
        /// Splits binary search tree in two. In the left tree all values will be less then
        /// <paramref name="splitValue"/>, in the right tree they will be greater or equal.
        /// </summary>
        /// <typeparam name="TValue">Type of node values.</typeparam>
        /// <param name="root">Root of tree to split.</param>
        /// <param name="splitValue">Value to split by.</param>
        /// <param name="comparer">Comparer of values.</param>
        internal static Tuple<BinarySearchTreeNode<TValue>, BinarySearchTreeNode<TValue>> Split<TValue>(
            BinarySearchTreeNode<TValue> root,
            TValue splitValue,
            [NotNull] IComparer<TValue> comparer)
        {
            if(root == null)
                return new Tuple<BinarySearchTreeNode<TValue>, BinarySearchTreeNode<TValue>>(null, null);

            if (comparer.Compare(splitValue, root.Value) < 0)
            {
                var splitRoots = Split(root.LeftChild, splitValue, comparer);

                MergeWithRoot<TValue, BinarySearchTreeNode<TValue>>(splitRoots.Item2, root.RightChild, root);

                return new Tuple<BinarySearchTreeNode<TValue>, BinarySearchTreeNode<TValue>>(splitRoots.Item1, root);
            }
            else
            {
                var splitRoots = Split(root.RightChild, splitValue, comparer);

                MergeWithRoot<TValue, BinarySearchTreeNode<TValue>>(root.LeftChild, splitRoots.Item1, root);

                return new Tuple<BinarySearchTreeNode<TValue>, BinarySearchTreeNode<TValue>>(root, splitRoots.Item2);
            }
        }

        /// <summary>
        /// Splits binary search tree in two. In the left tree all values will be less then
        /// <paramref name="splitValue"/>, in the right tree they will be greater or equal.
        /// </summary>
        /// <typeparam name="TValue">Type of node values.</typeparam>
        /// <param name="tree">Tree to split.</param>
        /// <param name="splitValue">Value to split by.</param>
        /// <param name="comparer">Comparer of values.</param>
        public static Tuple<BinarySearchTree<TValue>, BinarySearchTree<TValue>> Split<TValue>(
            BinarySearchTree<TValue> tree, TValue splitValue,
            [NotNull] IComparer<TValue> comparer)
        {
            if (tree?.Root == null)
                return new Tuple<BinarySearchTree<TValue>, BinarySearchTree<TValue>>(null, null);
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var splitRoots = Split(tree.Root, splitValue, comparer);

            return new Tuple<BinarySearchTree<TValue>, BinarySearchTree<TValue>>(
                new BinarySearchTree<TValue>(splitRoots.Item1, comparer),
                new BinarySearchTree<TValue>(splitRoots.Item2, comparer)
                );
        }
    }

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

        internal BinarySearchTree(BinarySearchTreeNode<TValue> root, [NotNull] IComparer<TValue> comparer)
            : this(comparer)
        {
            _root = root;
        }

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
            return new ReplaceCommandFirstStep<BinarySearchTreeNode<TValue>>(nodeToReplace, SetRoot);
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