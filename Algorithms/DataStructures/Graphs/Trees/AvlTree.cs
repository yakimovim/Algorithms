using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    /// <summary>
    /// Contains methods for work with AVL tree.
    /// </summary>
    public static class AvlTree
    {
        /// <summary>
        /// Rebalances node of AVL tree.
        /// </summary>
        /// <typeparam name="TValue">Type of node values.</typeparam>
        /// <param name="node">Node of AVL tree.</param>
        internal static void Rebalance<TValue>(this AvlTreeNode<TValue> node)
        {
            while (node != null)
            {
                var parent = node.Parent;

                node.Balance();

                node.AdjustHeight();

                node = parent;
            }
        }

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
            where TNode : AvlTreeNode<TValue>
        {
            if (mergedTreeRoot == null) throw new ArgumentNullException(nameof(mergedTreeRoot));

            mergedTreeRoot.LeftChild = leftTreeRoot;
            mergedTreeRoot.RightChild = rightTreeRoot;
        }

        /// <summary>
        /// Merges two trees into existing root.
        /// </summary>
        /// <typeparam name="TValue">Type of node values.</typeparam>
        /// <param name="leftTree">Left tree.</param>
        /// <param name="rightTree">Right tree.</param>
        /// <param name="comparer">Comparer of values.</param>
        public static AvlTree<TValue> Merge<TValue>(
            AvlTree<TValue> leftTree,
            AvlTree<TValue> rightTree,
            [NotNull] IComparer<TValue> comparer)
        {
            if (leftTree?.Root == null)
                return rightTree;
            if (rightTree?.Root == null)
                return leftTree;
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var maxNode = leftTree.Root.FindNodeWithMaximalValue();
            leftTree.Remove(maxNode.Value);

            var mergedTreeRoot = new AvlTreeNode<TValue>(comparer, maxNode.Value, null);

            var newRoot = MergeWithRoot(leftTree.Root, rightTree.Root, mergedTreeRoot);

            return new AvlTree<TValue>(newRoot, comparer);
        }

        private static AvlTreeNode<TValue> MergeWithRoot<TValue>(
            AvlTreeNode<TValue> leftTreeRoot,
            AvlTreeNode<TValue> rightTreeRoot,
            [NotNull] AvlTreeNode<TValue> mergedTreeRoot)
        {
            if (leftTreeRoot == null && rightTreeRoot == null)
            { return mergedTreeRoot; }
            if (leftTreeRoot == null)
            {
                var subRoot = rightTreeRoot;
                while (subRoot.LeftChild != null)
                {
                    subRoot = subRoot.LeftChild;
                }

                subRoot.LeftChild = mergedTreeRoot;
                mergedTreeRoot.Parent = subRoot;
                mergedTreeRoot.Rebalance();
                return mergedTreeRoot.GetTreeRoot();
            }
            if (rightTreeRoot == null)
            {
                var subRoot = leftTreeRoot;
                while (subRoot.RightChild != null)
                {
                    subRoot = subRoot.RightChild;
                }

                subRoot.RightChild = mergedTreeRoot;
                mergedTreeRoot.Parent = subRoot;
                mergedTreeRoot.Rebalance();
                return mergedTreeRoot.GetTreeRoot();
            }

            if (HeightDistance(leftTreeRoot, rightTreeRoot) <= 1)
            {
                MergeWithRoot<TValue, AvlTreeNode<TValue>>(leftTreeRoot, rightTreeRoot, mergedTreeRoot);
                mergedTreeRoot.Height = 1 + Math.Max(leftTreeRoot.Height, rightTreeRoot.Height);
                return mergedTreeRoot;
            }

            if (leftTreeRoot.Height > rightTreeRoot.Height)
            {
                var subRoot = leftTreeRoot;
                while (HeightDistance(subRoot, rightTreeRoot) > 1)
                {
                    subRoot = subRoot.RightChild;
                }

                var subRootParent = subRoot.Parent;

                MergeWithRoot<TValue, AvlTreeNode<TValue>>(subRoot, rightTreeRoot, mergedTreeRoot);
                mergedTreeRoot.Height = 1 + Math.Max(subRoot.Height, rightTreeRoot.Height);

                if(subRootParent != null)
                { subRootParent.RightChild = mergedTreeRoot;}
                mergedTreeRoot.Parent = subRootParent;

                mergedTreeRoot.Rebalance();

                return mergedTreeRoot.GetTreeRoot();
            }

            if (leftTreeRoot.Height < rightTreeRoot.Height)
            {
                var subRoot = rightTreeRoot;
                while (HeightDistance(leftTreeRoot, subRoot) > 1)
                {
                    subRoot = subRoot.LeftChild;
                }

                var subRootParent = subRoot.Parent;

                MergeWithRoot<TValue, AvlTreeNode<TValue>>(leftTreeRoot, subRoot, mergedTreeRoot);
                mergedTreeRoot.Height = 1 + Math.Max(leftTreeRoot.Height, subRoot.Height);

                if (subRootParent != null)
                { subRootParent.LeftChild = mergedTreeRoot; }
                mergedTreeRoot.Parent = subRootParent;

                mergedTreeRoot.Rebalance();

                return mergedTreeRoot.GetTreeRoot();
            }

            throw new InvalidOperationException("This situation should never occur.");
        }

        private static long HeightDistance<TValue>(AvlTreeNode<TValue> node1,
            AvlTreeNode<TValue> node2)
        {
            return Math.Abs((long) node1.Height - (long) node2.Height);
        }
    }

    /// <summary>
    /// Self-balancing binary tree invented by Adelson-Velsky and Landis (1962).
    /// </summary>
    public class AvlTree<TValue> : BinarySearchTreeBase<TValue>
    {
        [CanBeNull]
        private AvlTreeNode<TValue> _root;

        internal AvlTreeNode<TValue> Root => _root;

        public AvlTree([NotNull] IComparer<TValue> comparer)
            : base(comparer)
        { }

        public AvlTree()
        { }

        internal AvlTree(AvlTreeNode<TValue> root, [NotNull] IComparer<TValue> comparer)
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
                _root = new AvlTreeNode<TValue>(Comparer, value, null);
            else
            {
                var nodeToAddTo = _root.FindNodeToAddTo(value, Comparer);

                var newNode = new AvlTreeNode<TValue>(Comparer, value, nodeToAddTo);

                if (Comparer.Compare(value, nodeToAddTo.Value) < 0)
                {
                    nodeToAddTo.LeftChild = newNode;
                }
                else
                {
                    nodeToAddTo.RightChild = newNode;
                }

                newNode.Rebalance();

                _root = newNode.GetTreeRoot();
            }
        }

        
        /// <summary>
        /// Checks if the tree contains the value;
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True, if the tree contains the value. Otherwise, false.</returns>
        public override bool Contains(TValue value)
        {
            var node = _root.FindNodeByValue(value, Comparer);

            return node != null && Comparer.Compare(node.Value, value) == 0;
        }

        /// <summary>
        /// Removes the value from the tree.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        /// <returns>True, if value was removed. Otherwise, false.</returns>
        public override bool Remove(TValue value)
        {
            var nodeToRemove = _root.FindNodeByValue(value, Comparer);
            if (nodeToRemove == null || Comparer.Compare(nodeToRemove.Value, value) != 0)
                return false;

            var parentOfNodeToRemove = nodeToRemove.Parent;
            AvlTreeNode<TValue> balancingStartNode;

            if (nodeToRemove.IsLeaf())
            {
                Replace(nodeToRemove).At(parentOfNodeToRemove).With(null);
                balancingStartNode = parentOfNodeToRemove;
            }
            else
            {
                if (nodeToRemove.RightChild == null)
                {
                    Replace(nodeToRemove).At(parentOfNodeToRemove).With(nodeToRemove.LeftChild);
                    balancingStartNode = nodeToRemove.LeftChild;
                }
                else if (nodeToRemove.RightChild.LeftChild == null)
                {
                    Replace(nodeToRemove).At(parentOfNodeToRemove).With(nodeToRemove.RightChild);
                    nodeToRemove.RightChild.LeftChild = nodeToRemove.LeftChild;
                    balancingStartNode = nodeToRemove.RightChild;
                }
                else
                {
                    var leftmostNode = FindLeftmostChildOf(nodeToRemove.RightChild);
                    balancingStartNode = leftmostNode.Parent;
                    var parentOfLeftmostNode = leftmostNode.Parent;

                    Replace(leftmostNode).At(parentOfLeftmostNode).With(leftmostNode.RightChild);
                    Replace(nodeToRemove).At(parentOfNodeToRemove).With(leftmostNode);
                    leftmostNode.LeftChild = nodeToRemove.LeftChild;
                    leftmostNode.RightChild = nodeToRemove.RightChild;
                }
            }

            balancingStartNode.Rebalance();

            UpdateRootReference(nodeToRemove, balancingStartNode);

            return true;
        }

        private void UpdateRootReference(AvlTreeNode<TValue> nodeToRemove, AvlTreeNode<TValue> balancingStartNode)
        {
            if (ReferenceEquals(nodeToRemove, _root))
            {
                _root = balancingStartNode?.GetTreeRoot();
            }
        }

        private AvlTreeNode<TValue> FindLeftmostChildOf(AvlTreeNode<TValue> node)
        {
            while (node.LeftChild != null)
            {
                node = node.LeftChild;
            }

            return node;
        }

        private ReplaceCommandFirstStep<AvlTreeNode<TValue>> Replace(AvlTreeNode<TValue> nodeToReplace)
        {
            return new ReplaceCommandFirstStep<AvlTreeNode<TValue>>(nodeToReplace, node =>
            {
                if (node != null)
                    node.Parent = null;
            });
        }

        public override IEnumerator<TValue> GetEnumerator()
        {
            var values = new LinkedList<TValue>();

            _root.VisitInOrder<TValue, AvlTreeNode<TValue>>(value => values.AddLast(value));

            return values.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join("\n", _root.GetStringPresentation<TValue, AvlTreeNode<TValue>>());
        }
    }
}