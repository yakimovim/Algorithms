using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
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

        private void SetRoot(AvlTreeNode<TValue> node)
        {
            _root = node;
            if (node != null)
            { node.Parent = null; }
        }

        /// <summary>
        /// Adds value to the tree.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public override void Add(TValue value)
        {
            if (_root == null)
                _root = new AvlTreeNode<TValue>(Comparer, value, null, SetRoot);
            else
            {
                var nodeToAddTo = _root.FindNodeToAddTo(value, Comparer);

                var newNode = new AvlTreeNode<TValue>(Comparer, value, nodeToAddTo, SetRoot);

                if (Comparer.Compare(value, nodeToAddTo.Value) < 0)
                {
                    nodeToAddTo.LeftChild = newNode;
                }
                else
                {
                    nodeToAddTo.RightChild = newNode;
                }

                Rebalance(newNode);
            }
        }

        private void Rebalance(AvlTreeNode<TValue> node)
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

            Rebalance(balancingStartNode);

            return true;
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
            return new ReplaceCommandFirstStep<AvlTreeNode<TValue>>(SetRoot, nodeToReplace);
        }

        public override IEnumerator<TValue> GetEnumerator()
        {
            var values = new LinkedList<TValue>();

            _root.VisitInOrder(value => values.AddLast(value));

            return values.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join("\n", _root.GetStringPresentation());
        }
    }
}