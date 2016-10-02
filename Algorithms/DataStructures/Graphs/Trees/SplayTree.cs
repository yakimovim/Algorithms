using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    /// <summary>
    /// Contains methods for work with splay tree.
    /// </summary>
    public static class SplayTree
    {
        private static bool ShouldZigZig<TValue>(this SplayTreeNode<TValue> node)
        {
            return (node.IsLeftChild() && (node?.Parent.IsLeftChild() ?? false))
                   || (node.IsRigthChild() && (node?.Parent.IsRigthChild() ?? false));
        }

        private static void ZigZig<TValue>(this SplayTreeNode<TValue> node)
        {
            Contract.Requires(node != null);
            Contract.Requires(node.Parent != null);
            Contract.Requires(node.Parent.Parent != null);

            var parent = node.Parent;
            Contract.Assume(parent != null);
            var grandParent = parent.Parent;
            Contract.Assume(grandParent != null);
            var grandGrandParent = grandParent.Parent;

            var grandParentIsLeftChild = grandParent.IsLeftChild();

            if (node.IsLeftChild() && parent.IsLeftChild())
            {
                grandParent.LeftChild = parent.RightChild;
                parent.RightChild = grandParent;
                parent.LeftChild = node.RightChild;
                node.RightChild = parent;
            }
            else
            {
                grandParent.RightChild = parent.LeftChild;
                parent.LeftChild = grandParent;
                parent.RightChild = node.LeftChild;
                node.LeftChild = parent;
            }

            if (grandGrandParent != null)
            {
                if (grandParentIsLeftChild)
                    grandGrandParent.LeftChild = node;
                else
                    grandGrandParent.RightChild = node;
            }
            else
            {
                node.Parent = null;
            }
        }

        private static bool ShouldZigZag<TValue>(this SplayTreeNode<TValue> node)
        {
            return (node.IsLeftChild() && (node?.Parent.IsRigthChild() ?? false))
                   || (node.IsRigthChild() && (node?.Parent.IsLeftChild() ?? false));
        }

        private static void ZigZag<TValue>(this SplayTreeNode<TValue> node)
        {
            Contract.Requires(node != null);
            Contract.Requires(node.Parent != null);
            Contract.Requires(node.Parent.Parent != null);

            var parent = node.Parent;
            Contract.Assume(parent != null);
            var grandParent = parent.Parent;
            Contract.Assume(grandParent != null);
            var grandGrandParent = grandParent.Parent;

            var grandParentIsLeftChild = grandParent.IsLeftChild();

            if (node.IsRigthChild() && parent.IsLeftChild())
            {
                grandParent.LeftChild = node.RightChild;
                parent.RightChild = node.LeftChild;
                node.LeftChild = parent;
                node.RightChild = grandParent;
            }
            else
            {
                grandParent.RightChild = node.LeftChild;
                parent.LeftChild = node.RightChild;
                node.LeftChild = grandParent;
                node.RightChild = parent;
            }

            if (grandGrandParent != null)
            {
                if (grandParentIsLeftChild)
                    grandGrandParent.LeftChild = node;
                else
                    grandGrandParent.RightChild = node;
            }
            else
            {
                node.Parent = null;
            }
        }

        private static bool ShouldZig<TValue>(this SplayTreeNode<TValue> node)
        {
            return node?.Parent != null && node.Parent.Parent == null;
        }

        private static void Zig<TValue>(this SplayTreeNode<TValue> node)
        {
            Contract.Requires(node != null);
            Contract.Requires(node.Parent != null);

            var parent = node.Parent;
            Contract.Assume(parent != null);

            if (node.IsRigthChild())
            {
                parent.RightChild = node.LeftChild;
                node.LeftChild = parent;
            }
            else
            {
                parent.LeftChild = node.RightChild;
                node.RightChild = parent;
            }

            node.Parent = null;
        }

        public static void ApplyZigZagging<TValue>(this SplayTreeNode<TValue> node)
        {
            if (node == null)
                return;

            if (ShouldZig(node))
                Zig(node);
            else if (ShouldZigZag(node))
                ZigZag(node);
            else if (ShouldZigZig(node))
                ZigZig(node);
        }

        /// <summary>
        /// Splits binary search tree in two. In the left tree all values will be less 
        /// or equal then <paramref name="splitValue"/>, in the right tree they will 
        /// be greater or equal.
        /// </summary>
        /// <typeparam name="TValue">Type of node values.</typeparam>
        /// <param name="tree">Tree to split.</param>
        /// <param name="splitValue">Value to split by.</param>
        /// <param name="comparer">Comparer of values.</param>
        public static Tuple<SplayTree<TValue>, SplayTree<TValue>> Split<TValue>(
            SplayTree<TValue> tree, TValue splitValue,
            [NotNull] IComparer<TValue> comparer)
        {
            if (tree?.Root == null)
                return new Tuple<SplayTree<TValue>, SplayTree<TValue>>(
                    new SplayTree<TValue>(comparer),
                    new SplayTree<TValue>(comparer));

            var node = tree.Find(splitValue);
            if (comparer.Compare(splitValue, node.Value) < 0)
            {
                var leftTreeRoot = node.LeftChild;
                var rightTreeRoot = node;
                rightTreeRoot.LeftChild = null;
                if (leftTreeRoot != null)
                { leftTreeRoot.Parent = null; }

                return new Tuple<SplayTree<TValue>, SplayTree<TValue>>(
                    new SplayTree<TValue>(leftTreeRoot, comparer),
                    new SplayTree<TValue>(rightTreeRoot, comparer));
            }
            else
            {
                var leftTreeRoot = node;
                var rightTreeRoot = node.RightChild;
                leftTreeRoot.RightChild = null;
                if (rightTreeRoot != null)
                { rightTreeRoot.Parent = null; }

                return new Tuple<SplayTree<TValue>, SplayTree<TValue>>(
                    new SplayTree<TValue>(leftTreeRoot, comparer),
                    new SplayTree<TValue>(rightTreeRoot, comparer));
            }
        }
    }


    public class SplayTree<TValue> : BinarySearchTreeBase<TValue>
    {
        [CanBeNull]
        private SplayTreeNode<TValue> _root;

        internal SplayTreeNode<TValue> Root => _root;

        public SplayTree([NotNull] IComparer<TValue> comparer)
            : base(comparer)
        { }

        public SplayTree()
        { }

        internal SplayTree(SplayTreeNode<TValue> root, [NotNull] IComparer<TValue> comparer)
            : this(comparer)
        {
            _root = root;
        }

        private void Splay(SplayTreeNode<TValue> node)
        {
            if (node == null)
                return;

            while (node.Parent != null)
            {
                node.ApplyZigZagging();
            }

            _root = node;
        }

        public override void Add(TValue value)
        {
            if (_root == null)
                _root = new SplayTreeNode<TValue>(value, null);
            else
            {
                var node = _root;

                SplayTreeNode<TValue> parent = null;
                bool shouldAddToLeft;

                while (true)
                {
                    var nodeValue = node.Value;

                    if (Comparer.Compare(value, nodeValue) < 0)
                    {
                        if (node.LeftChild == null)
                        {
                            parent = node;
                            shouldAddToLeft = true;
                            break;
                        }

                        node = node.LeftChild;
                    }
                    else
                    {
                        if (node.RightChild == null)
                        {
                            parent = node;
                            shouldAddToLeft = false;
                            break;
                        }

                        node = node.RightChild;
                    }
                }

                var addedNode = new SplayTreeNode<TValue>(value, parent);

                if (shouldAddToLeft)
                {
                    parent.LeftChild = addedNode;
                }
                else
                {
                    parent.RightChild = addedNode;
                }

                Splay(addedNode);
            }
        }

        public override bool Contains(TValue value)
        {
            var node = Find(value);

            return node != null && Comparer.Compare(node.Value, value) == 0;
        }

        public SplayTreeNode<TValue> Find(TValue value)
        {
            var node = _root.FindNodeByValue(value, Comparer);
            Splay(node);
            return node;
        }

        public override bool Remove(TValue value)
        {
            var nodeToRemove = Find(value);
            if (nodeToRemove == null)
                return false;
            if (Comparer.Compare(nodeToRemove.Value, value) != 0)
                return false;

            Splay(nodeToRemove.Next());
            Splay(nodeToRemove);

            var nodeToRemoveLeftChild = nodeToRemove.LeftChild;
            var nodeToRemoveRightChild = nodeToRemove.RightChild;

            if (nodeToRemove.IsLeaf())
            {
                Replace(nodeToRemove).At(nodeToRemove.Parent).With(null);
            }
            else
            {
                if (nodeToRemove.RightChild == null)
                {
                    Replace(nodeToRemove).At(nodeToRemove.Parent).With(nodeToRemove.LeftChild);
                }
                else if (nodeToRemove.RightChild.LeftChild == null)
                {
                    Replace(nodeToRemove).At(nodeToRemove.Parent).With(nodeToRemove.RightChild);
                    nodeToRemove.RightChild.LeftChild = nodeToRemove.LeftChild;
                }
                else
                {
                    var leftmostNode = FindLeftmostChildOf(nodeToRemove.RightChild);
                    var parentOfLeftmostNode = leftmostNode.Parent;

                    Replace(leftmostNode).At(parentOfLeftmostNode).With(leftmostNode.RightChild);
                    Replace(nodeToRemove).At(nodeToRemove.Parent).With(leftmostNode);
                    leftmostNode.LeftChild = nodeToRemove.LeftChild;
                    leftmostNode.RightChild = nodeToRemove.RightChild;
                }
            }

            if (nodeToRemoveLeftChild != null)
            {
                _root = nodeToRemoveLeftChild.GetTreeRoot();
            }
            else
            {
                _root = nodeToRemoveRightChild?.GetTreeRoot();
            }

            return true;
        }

        private ReplaceCommandFirstStep<SplayTreeNode<TValue>> Replace(SplayTreeNode<TValue> nodeToReplace)
        {
            return new ReplaceCommandFirstStep<SplayTreeNode<TValue>>(nodeToReplace, node =>
            {
                if (node != null)
                    node.Parent = null;
            });
        }

        private SplayTreeNode<TValue> FindLeftmostChildOf(SplayTreeNode<TValue> node)
        {
            while (node.LeftChild != null)
            {
                node = node.LeftChild;
            }

            return node;
        }

        public override IEnumerator<TValue> GetEnumerator()
        {
            var values = new LinkedList<TValue>();

            _root.VisitInOrder<TValue, SplayTreeNode<TValue>>(value => values.AddLast(value));

            return values.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join("\n", _root.GetStringPresentation<TValue, SplayTreeNode<TValue>>());
        }
    }
}