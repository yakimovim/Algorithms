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
                _root.Add(value);
                _root.Balance();
            }
        }

        /// <summary>
        /// Checks if the tree contains the value;
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True, if the tree contains the value. Otherwise, false.</returns>
        public override bool Contains(TValue value)
        {
            return FindNodeByValue(value) != null;
        }

        /// <summary>
        /// Finds node by its value.
        /// </summary>
        /// <param name="value">Value of node.</param>
        [CanBeNull]
        private AvlTreeNode<TValue> FindNodeByValue(TValue value)
        {
            AvlTreeNode<TValue> node = _root;

            while (node != null)
            {
                var comparison = Comparer.Compare(value, node.Value);

                if (comparison == 0)
                    break;

                node = comparison < 0 ? node.LeftChild : node.RightChild;
            }

            return node;
        }

        /// <summary>
        /// Removes the value from the tree.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        /// <returns>True, if value was removed. Otherwise, false.</returns>
        public override bool Remove(TValue value)
        {
            var nodeToRemove = FindNodeByValue(value);
            if (nodeToRemove == null)
                return false;

            var parentOfNodeToRemove = nodeToRemove.Parent;
            AvlTreeNode<TValue> balancingStartNode = null;

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

            BalanceFrom(balancingStartNode);

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

        private void BalanceFrom([CanBeNull] AvlTreeNode<TValue> node)
        {
            if(node == null)
                return;

            var parent = node.Parent;
            node.Balance();

            BalanceFrom(parent);
        }

        private ReplaceCommandFirstStep<AvlTreeNode<TValue>> Replace(AvlTreeNode<TValue> nodeToReplace)
        {
            return new ReplaceCommandFirstStep<AvlTreeNode<TValue>>(SetRoot, nodeToReplace);
        }

        /// <summary>
        /// Checks if every node of the tree is balanced.
        /// </summary>
        /// <returns>True, if all nodes of the tree are balanced. False, otherwise.</returns>
        internal bool IsBalanced()
        {
            return IsBalanced(_root);
        }

        /// <summary>
        /// Checks if given node is balanced with its children.
        /// </summary>
        /// <returns>True, if given node is balanced with its children. False, otherwise.</returns>
        private bool IsBalanced(AvlTreeNode<TValue> node)
        {
            if (node == null)
                return true;

            return node.State == TreeState.Balanced
                   && IsBalanced(node.LeftChild)
                   && IsBalanced(node.RightChild);
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