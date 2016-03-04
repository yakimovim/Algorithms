using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    /// <summary>
    /// Represents base class for binary search trees.
    /// </summary>
    public abstract class BinarySearchTreeBase<TValue> : IEnumerable<TValue>, ICanAdd<TValue>
    {
        protected class ReplaceCommandFirstStep<TNode>
            where TNode : BinaryTreeNodeBase<TNode, TValue>
        {
            private readonly Action<TNode> _setTreeRoot;
            private readonly TNode _nodeToReplace;

            public ReplaceCommandFirstStep([NotNull] Action<TNode> setTreeRoot,
                [NotNull] TNode nodeToReplace)
            {
                if (setTreeRoot == null) throw new ArgumentNullException(nameof(setTreeRoot));
                if (nodeToReplace == null) throw new ArgumentNullException(nameof(nodeToReplace));
                _setTreeRoot = setTreeRoot;
                _nodeToReplace = nodeToReplace;
            }

            public ReplaceCommandLastStep<TNode> At([CanBeNull] TNode parentOfNodeToReplace)
            {
                return new ReplaceCommandLastStep<TNode>(_setTreeRoot, _nodeToReplace, parentOfNodeToReplace);
            }
        }

        protected class ReplaceCommandLastStep<TNode>
            where TNode : BinaryTreeNodeBase<TNode, TValue>
        {
            private readonly Action<TNode> _setTreeRoot;
            private readonly TNode _nodeToReplace;
            private readonly TNode _parentOfNodeToReplace;

            public ReplaceCommandLastStep([NotNull] Action<TNode> setTreeRoot,
                [NotNull] TNode nodeToReplace, 
                TNode parentOfNodeToReplace)
            {
                if (setTreeRoot == null) throw new ArgumentNullException(nameof(setTreeRoot));
                if (nodeToReplace == null) throw new ArgumentNullException(nameof(nodeToReplace));
                _setTreeRoot = setTreeRoot;
                _nodeToReplace = nodeToReplace;
                _parentOfNodeToReplace = parentOfNodeToReplace;
            }

            public void With([CanBeNull] TNode node)
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
                    _setTreeRoot(node);
                }
            }
        }

        [NotNull] protected readonly IComparer<TValue> Comparer;

        protected BinarySearchTreeBase([NotNull] IComparer<TValue> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            Comparer = comparer;
        }

        protected BinarySearchTreeBase()
        {
            if (typeof(IComparable<TValue>).IsAssignableFrom(typeof(TValue)))
                Comparer = Comparer<TValue>.Default;
            else
                throw new InvalidOperationException($"Type {typeof(TValue).Name} is not comparable.");
        }

        /// <summary>
        /// Adds value to the tree.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public abstract void Add(TValue value);

        /// <summary>
        /// Checks if the tree contains the value;
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True, if the tree contains the value. Otherwise, false.</returns>
        public abstract bool Contains(TValue value);

        /// <summary>
        /// Removes the value from the tree.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        /// <returns>True, if value was removed. Otherwise, false.</returns>
        public abstract bool Remove(TValue value);

        public abstract IEnumerator<TValue> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}