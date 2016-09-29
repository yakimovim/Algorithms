using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs
{
    /// <summary>
    /// Represents one node of binary tree.
    /// </summary>
    /// <typeparam name="TValue">Type of value stored in the node.</typeparam>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    public interface IBinaryTreeNode<out TValue, out TNode>
        where TNode : IBinaryTreeNode<TValue, TNode>
    {
        /// <summary>
        /// Gets value of the node.
        /// </summary>
        TValue Value { get; }
        /// <summary>
        /// Gets left child of the node. Can be null.
        /// </summary>
        [CanBeNull]
        TNode LeftChild { get; }
        /// <summary>
        /// Gets right child of the node. Can be null.
        /// </summary>
        [CanBeNull]
        TNode RightChild { get; }

        /// <summary>
        /// This property may be used by algorithms to store specific information.
        /// One should provide only empty dictionary. Lazy initialization is welcomed.
        /// </summary>
        [NotNull]
        IDictionary<string, object> Properties { get; }
    }

    /// <summary>
    /// Represents one node of binary tree.
    /// </summary>
    /// <typeparam name="TValue">Type of value stored in the node.</typeparam>
    public interface IBinaryTreeNode<out TValue> : IBinaryTreeNode<TValue, IBinaryTreeNode<TValue>>
    { }

    /// <summary>
    /// Represents some item with parent.
    /// </summary>
    /// <typeparam name="TParent">Type of parent.</typeparam>
    public interface IParented<out TParent>
    {
        /// <summary>
        /// Gets parent of the node. Can be null.
        /// </summary>
        [CanBeNull]
        TParent Parent { get; }
    }

    /// <summary>
    /// Base class for implementation of binary tree nodes.
    /// </summary>
    /// <typeparam name="TNode">Type of binary tree node.</typeparam>
    /// <typeparam name="TValue">Type of value stored in the node.</typeparam>
    public abstract class BinaryTreeNodeBase<TNode, TValue> : IBinaryTreeNode<TValue, TNode>
        where TNode : BinaryTreeNodeBase<TNode, TValue>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDictionary<string, object>> _properties = new Lazy<IDictionary<string, object>>(() => new Dictionary<string, object>(1));

        /// <summary>
        /// Gets or sets value of the node.
        /// </summary>
        public TValue Value { get; protected set; }

        /// <summary>
        /// Gets or sets left child of the node. Can be null.
        /// </summary>
        [CanBeNull]
        public virtual TNode LeftChild { get; set; }
        /// <summary>
        /// Gets or sets right child of the node. Can be null.
        /// </summary>
        [CanBeNull]
        public virtual TNode RightChild { get; set; }

        public IDictionary<string, object> Properties => _properties.Value;

        public override string ToString()
        {
            return string.Join("\n", this.GetStringPresentation());
        }
    }

    /// <summary>
    /// Provides extension methods for binary tree nodes.
    /// </summary>
    public static class BinaryTreeNodeExtender
    {
        /// <summary>
        /// Determines if the node has no children.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <returns>True, if node has no children. False, otherwise.</returns>
        public static bool IsLeaf<TValue, TNode>(this IBinaryTreeNode<TValue, TNode> node) 
            where TNode : IBinaryTreeNode<TValue, TNode>
        {
            return node.LeftChild == null && node.RightChild == null;
        }

        /// <summary>
        /// Returns node which contains given value. Or returns node to which node with this value should be attached.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Root node.</param>
        /// <param name="value">Value to find.</param>
        /// <param name="comparer">Comparer of values.</param>
        /// <remarks>Applicable only to binary search trees.</remarks>
        public static TNode FindNodeByValue<TValue, TNode>(
            this TNode node, 
            TValue value, 
            [NotNull] IComparer<TValue> comparer) 
            where TNode : IBinaryTreeNode<TValue, TNode>
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            if (node == null)
                return default(TNode);

            while(true)
            {
                var nodeValue = node.Value;
                var comparison = comparer.Compare(value, nodeValue);

                if (comparison < 0)
                {
                    if (node.LeftChild == null)
                        return node;
                    node = node.LeftChild;
                }
                else if (comparison > 0)
                {
                    if (node.RightChild == null)
                        return node;
                    node = node.RightChild;
                }
                else
                    return node;
            }
        }

        /// <summary>
        /// Returns height of binary tree node.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <returns>Height of binary tree node. 0 if node is null.</returns>
        public static ulong GetHeight<TValue, TNode>(this TNode node) 
            where TNode : IBinaryTreeNode<TValue, TNode>
        {
            if (node == null)
                return 0;

            var key = Guid.NewGuid().ToString();

            var queue = new Queue<IBinaryTreeNode<TValue, TNode>>();
            queue.Enqueue(node);
            node.Properties[key] = 1UL;
            while (true)
            {
                var currentNode = queue.Dequeue();

                var currentHeight = (ulong) currentNode.Properties[key];

                foreach (var child in new [] { currentNode.LeftChild, currentNode.RightChild }.Where(c => c != null))
                {
                    child.Properties[key] = currentHeight + 1;
                    queue.Enqueue(child);
                }

                currentNode.Properties.Remove(key);


                if (queue.Count == 0)
                    return currentHeight;
            }
        }

        /// <summary>
        /// Returns node with minimal value in the sub-tree of this node.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <remarks>Applicable only to binary search trees.</remarks>
        public static TNode FindNodeWithMinimalValue<TValue, TNode>(this TNode node) 
            where TNode : IBinaryTreeNode<TValue, TNode>
        {
            if (node == null)
                return default(TNode);

            while (true)
            {
                if (node.LeftChild == null)
                    return node;

                node = node.LeftChild;
            }
        }

        /// <summary>
        /// Returns node with maximal value in the sub-tree of this node.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <remarks>Applicable only to binary search trees.</remarks>
        public static TNode FindNodeWithMaximalValue<TValue, TNode>(this TNode node) 
            where TNode : IBinaryTreeNode<TValue, TNode>
        {
            if (node == null)
                return default(TNode);

            while (true)
            {
                if (node.RightChild == null)
                    return node;

                node = node.RightChild;
            }
        }

        /// <summary>
        /// Returns node with next value greater or equal to the value of this node.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <remarks>Applicable only to binary search trees.</remarks>
        public static TNode Next<TValue, TNode>(this TNode node) 
            where TNode : IBinaryTreeNode<TValue, TNode>, IParented<TNode>
        {
            if (node == null)
                return default(TNode);

            if (node.RightChild != null)
            {
                node = node.RightChild;

                while (true)
                {
                    if (node.LeftChild == null)
                        return node;

                    node = node.LeftChild;
                }
            }
            else
            {
                while (true)
                {
                    if (node.Parent == null)
                        return default(TNode);

                    if (object.ReferenceEquals(node.Parent.RightChild, node))
                        node = node.Parent;
                    else
                    {
                        return node.Parent;
                    }
                }
            }
        }

        /// <summary>
        /// Returns node with next value less or equal to the value of this node.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <remarks>Applicable only to binary search trees.</remarks>
        public static TNode Previous<TValue, TNode>(this TNode node) 
            where TNode : IBinaryTreeNode<TValue, TNode>, IParented<TNode>
        {
            if (node == null)
                return default(TNode);

            if (node.LeftChild != null)
            {
                node = node.LeftChild;

                while (true)
                {
                    if (node.RightChild == null)
                        return node;

                    node = node.RightChild;
                }
            }
            else
            {
                while (true)
                {
                    if (node.Parent == null)
                        return default(TNode);

                    if (object.ReferenceEquals(node.Parent.LeftChild, node))
                        node = node.Parent;
                    else
                    {
                        return node.Parent;
                    }
                }
            }
        }

        /// <summary>
        /// Returns from tree with root <paramref name="node"/> all values between <paramref name="from"/> and <paramref name="to"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <param name="from">Lower border of range.</param>
        /// <param name="to">Upper border of range.</param>
        /// <param name="comparer">Comparer of values.</param>
        /// <param name="includeFrom">Include values equal to <paramref name="from"/> to the result.</param>
        /// <param name="includeTo">Include values equal to <paramref name="to"/> to the result.</param>
        public static TValue[] RangeSearch<TValue, TNode>(this TNode node, 
            TValue from,
            TValue to,
            [NotNull] IComparer<TValue> comparer,
            bool includeFrom = false,
            bool includeTo = false) 
            where TNode : IBinaryTreeNode<TValue, TNode>, IParented<TNode>
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            if(node == null)
                return new TValue[0];

            var range = new LinkedList<TValue>();

            var rangeNode = node.FindNodeByValue(from, comparer);

            while (true)
            {
                var comparisonTo = comparer.Compare(to, rangeNode.Value);
                if(comparisonTo < 0)
                    break;


                if (comparer.Compare(from, rangeNode.Value) < 0)
                    range.AddLast(rangeNode.Value);

                rangeNode = rangeNode.Next<TValue, TNode>();
            }

            return range.ToArray();
        }

        /// <summary>
        /// Visits first left children, then itself, then right children.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <param name="action">Action for each node.</param>
        public static void VisitInOrder<TValue, TNode>(this IBinaryTreeNode<TValue, TNode> node, Action<TValue> action) 
            where TNode : IBinaryTreeNode<TValue, TNode>
        {
            if (node == null)
                return;

            var key = Guid.NewGuid().ToString();

            var stack = new Stack<IBinaryTreeNode<TValue, TNode>>();
            stack.Push(node);
            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                if (currentNode.Properties.ContainsKey(key))
                {
                    currentNode.Properties.Remove(key);
                    action(currentNode.Value);
                }
                else
                {
                    if (currentNode.RightChild != null)
                    {
                        stack.Push(currentNode.RightChild);
                    }
                    currentNode.Properties[key] = true;
                    stack.Push(currentNode);
                    if (currentNode.LeftChild != null)
                    {
                        stack.Push(currentNode.LeftChild);
                    }
                }
            }
        }

        /// <summary>
        /// Visits first itself, then left children, then right children.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <param name="action">Action for each node.</param>
        public static void VisitPreOrder<TValue, TNode>(this IBinaryTreeNode<TValue, TNode> node, Action<TValue> action) 
            where TNode : IBinaryTreeNode<TValue, TNode>
        {
            if (node == null)
                return;

            var key = Guid.NewGuid().ToString();

            var stack = new Stack<IBinaryTreeNode<TValue, TNode>>();
            stack.Push(node);
            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                if (currentNode.Properties.ContainsKey(key))
                {
                    currentNode.Properties.Remove(key);
                    action(currentNode.Value);
                }
                else
                {
                    if (currentNode.RightChild != null)
                    {
                        stack.Push(currentNode.RightChild);
                    }
                    if (currentNode.LeftChild != null)
                    {
                        stack.Push(currentNode.LeftChild);
                    }
                    currentNode.Properties[key] = true;
                    stack.Push(currentNode);
                }
            }
        }

        /// <summary>
        /// Visits first left children, then right children, then itself.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <param name="action">Action for each node.</param>
        public static void VisitPostOrder<TValue, TNode>(this IBinaryTreeNode<TValue, TNode> node, Action<TValue> action) 
            where TNode : IBinaryTreeNode<TValue, TNode>
        {
            if (node == null)
                return;

            var key = Guid.NewGuid().ToString();

            var stack = new Stack<IBinaryTreeNode<TValue, TNode>>();
            stack.Push(node);
            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                if (currentNode.Properties.ContainsKey(key))
                {
                    currentNode.Properties.Remove(key);
                    action(currentNode.Value);
                }
                else
                {
                    currentNode.Properties[key] = true;
                    stack.Push(currentNode);
                    if (currentNode.RightChild != null)
                    {
                        stack.Push(currentNode.RightChild);
                    }
                    if (currentNode.LeftChild != null)
                    {
                        stack.Push(currentNode.LeftChild);
                    }
                }
            }
        }

        /// <summary>
        /// Gets string presentation of node.
        /// </summary>
        /// <typeparam name="TValue">Type of node value.</typeparam>
        /// <typeparam name="TNode">Type of node.</typeparam>
        /// <param name="node">Tree node.</param>
        /// <returns>Array of strings of equal length with string presentation of sub-tree.</returns>
        public static string[] GetStringPresentation<TValue, TNode>(this IBinaryTreeNode<TValue, TNode> node) 
            where TNode : IBinaryTreeNode<TValue, TNode>
        {
            if (node == null)
                return new string[0];

            var leftPresentation = GetStringPresentation(node.LeftChild);
            var rightPresentation = GetStringPresentation(node.RightChild);

            var leftWidth = leftPresentation.Length > 0 ? leftPresentation[0].Length : 0;
            var rightWidth = rightPresentation.Length > 0 ? rightPresentation[0].Length : 0;

            var myPresentation = node.Value?.ToString() ?? "";
            var myWidth = myPresentation.Length;

            if (rightWidth == 0 && leftWidth == 0)
                return new[] {myPresentation};

            var middlePadding = Math.Max(3, myWidth);
            var totalWidth = Math.Max(leftWidth + middlePadding + rightWidth, myWidth);
            middlePadding = totalWidth - leftWidth - rightWidth;

            int leftPadding, rightPadding;
            if (leftWidth == 0)
            {
                leftPadding = 0;
                rightPadding = totalWidth - myWidth;
            }
            else if (rightWidth == 0)
            {
                leftPadding = totalWidth - myWidth;
                rightPadding = 0;
            }
            else
            {
                leftPadding = (totalWidth - myWidth) / 2;
                rightPadding = totalWidth - myWidth - leftPadding;
            }


            var presentation = new LinkedList<string>();
            presentation.AddLast(new string(' ', leftPadding) + myPresentation + new string(' ', rightPadding));

            var size = Math.Max(leftPresentation.Length, rightPresentation.Length);
            for (int i = 0; i < size; i++)
            {
                var value = (i < leftPresentation.Length)
                    ? leftPresentation[i]
                    : new string(' ', leftWidth);
                value += new string(' ', middlePadding);
                value += (i < rightPresentation.Length)
                    ? rightPresentation[i]
                    : new string(' ', rightWidth);
                presentation.AddLast(value);
            }

            return presentation.ToArray();
        }
    }
}
