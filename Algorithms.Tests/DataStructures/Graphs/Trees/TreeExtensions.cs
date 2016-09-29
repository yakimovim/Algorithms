using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Trees;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.DataStructures.Graphs.Trees
{
    /// <summary>
    /// Contains extension methods for trees to be used in tests.
    /// </summary>
    internal static class TreeExtensions
    {
        /// <summary>
        /// Checks if given node is balanced with its children.
        /// </summary>
        /// <returns>True, if given node is balanced with its children. False, otherwise.</returns>
        public static bool IsBalanced<TValue>(this AvlTreeNode<TValue> node)
        {
            if (node == null)
                return true;

            return node.State == TreeState.Balanced
                   && IsBalanced(node.LeftChild)
                   && IsBalanced(node.RightChild);
        }

        public static TValue[] GetValues<TValue, TNode>(this TNode node)
            where TNode : IBinaryTreeNode<TNode>, IValued<TValue>
        {
            var values = new LinkedList<TValue>();

            node.VisitInOrder<TValue, TNode>(v => values.AddLast(v));

            return values.ToArray();
        }

        public static void CheckBinarySearchTree<TValue, TNode>(this TNode node, IComparer<TValue> comparer)
            where TNode : IBinaryTreeNode<TNode>, IValued<TValue>
        {
            if(node == null)
                return;

            var leftValues = node.LeftChild.GetValues<TValue, TNode>();
            var rightValues = node.RightChild.GetValues<TValue, TNode>();

            foreach (var value in leftValues)
            {
                Assert.IsTrue(comparer.Compare(value, node.Value) <= 0);
            }
            foreach (var value in rightValues)
            {
                Assert.IsTrue(comparer.Compare(value, node.Value) >= 0);
            }

            CheckBinarySearchTree(node.LeftChild, comparer);
            CheckBinarySearchTree(node.RightChild, comparer);
        }
    }
}