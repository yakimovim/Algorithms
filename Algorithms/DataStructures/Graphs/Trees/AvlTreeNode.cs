using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    internal class AvlTreeNode<TValue> : BinaryTreeNodeBase<AvlTreeNode<TValue>, TValue>, IParented<AvlTreeNode<TValue>>
    {
        private readonly IComparer<TValue> _comparer;
        private readonly Action<AvlTreeNode<TValue>> _setTreeRoot;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AvlTreeNode<TValue> _leftChild;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AvlTreeNode<TValue> _rightChild;

        public AvlTreeNode(
            [NotNull] IComparer<TValue> comparer, 
            TValue value, 
            AvlTreeNode<TValue> parent,
            [NotNull] Action<AvlTreeNode<TValue>> setTreeRoot)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            if (setTreeRoot == null) throw new ArgumentNullException(nameof(setTreeRoot));
            _comparer = comparer;
            _setTreeRoot = setTreeRoot;
            Parent = parent;
            Value = value;
        }

        public AvlTreeNode<TValue> Parent { get; internal set; }

        public ulong Height { get; set; }

        public void AdjustHeight()
        {
            Height = 1 + Math.Max(LeftChild?.Height ?? 0UL, RightChild?.Height ?? 0UL);
        }

        public override AvlTreeNode<TValue> LeftChild
        {
            [DebuggerStepThrough]
            get { return _leftChild; }
            [DebuggerStepThrough]
            set
            {
                _leftChild = value;
                if (_leftChild != null)
                {
                    _leftChild.Parent = this;
                }
            }
        }

        public override AvlTreeNode<TValue> RightChild
        {
            [DebuggerStepThrough]
            get { return _rightChild; }
            [DebuggerStepThrough]
            set
            {
                _rightChild = value;
                if (_rightChild != null)
                {
                    _rightChild.Parent = this;
                }
            }
        }

        /// <summary>
        /// Balances the tree starting from this node.
        /// </summary>
        internal void Balance()
        {
            if (State == TreeState.RightHeavy)
            {
                Contract.Assert(RightChild != null);

                if (RightChild.BalanceFactor < 0)
                {
                    LeftRightRotation();
                }
                else
                {
                    LeftRotation();
                }
            }
            else if (State == TreeState.LeftHeavy)
            {
                Contract.Assert(LeftChild != null);

                if (LeftChild.BalanceFactor > 0)
                {
                    RightLeftRotation();
                }
                else
                {
                    RightRotation();
                }
            }
        }

        private void LeftRotation()
        {
            Contract.Assume(RightChild != null);

            var rightChild = RightChild;
            var leftChildOfRightChild = RightChild.LeftChild;

            if (Parent == null)
                _setTreeRoot(rightChild);
            else if (Parent.LeftChild == this)
                Parent.LeftChild = rightChild;
            else
                Parent.RightChild = rightChild;

            RightChild = leftChildOfRightChild;
            rightChild.LeftChild = this;

            leftChildOfRightChild?.AdjustHeight();
            AdjustHeight();
            rightChild.AdjustHeight();
            Parent?.AdjustHeight();
        }

        private void RightRotation()
        {
            Contract.Assume(LeftChild != null);

            var leftChild = LeftChild;
            var rightChildOfleftChild = LeftChild.RightChild;

            if (Parent == null)
                _setTreeRoot(leftChild);
            else if (Parent.LeftChild == this)
                Parent.LeftChild = leftChild;
            else
                Parent.RightChild = leftChild;

            LeftChild = rightChildOfleftChild;
            leftChild.RightChild = this;

            rightChildOfleftChild?.AdjustHeight();
            AdjustHeight();
            leftChild.AdjustHeight();
            Parent?.AdjustHeight();
        }

        private void LeftRightRotation()
        {
            Contract.Assume(RightChild != null);

            RightChild.RightRotation();

            LeftRotation();
        }

        private void RightLeftRotation()
        {
            Contract.Assume(LeftChild != null);

            LeftChild.LeftRotation();

            RightRotation();
        }

        /// <summary>
        /// Gets height of the left child.
        /// </summary>
        private ulong LeftHeight => LeftChild?.Height ?? 0UL;
        /// <summary>
        /// Gets height of the right child.
        /// </summary>
        private ulong RightHeight => RightChild?.Height ?? 0UL;
        /// <summary>
        /// Gets balance factor.
        /// </summary>
        private long BalanceFactor => (long) RightHeight - (long) LeftHeight;
        /// <summary>
        /// Gets state of the tree starting from this node.
        /// </summary>
        internal TreeState State
        {
            get
            {
                if(BalanceFactor > 1)
                    return TreeState.RightHeavy;
                if(BalanceFactor < -1)
                    return TreeState.LeftHeavy;
                return TreeState.Balanced;
            }
        }

        AvlTreeNode<TValue> IParented<AvlTreeNode<TValue>>.Parent => Parent;


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
                    LeftChild = new AvlTreeNode<TValue>(_comparer, value, this, _setTreeRoot);
                }
                else
                {
                    LeftChild.Add(value);
                    LeftChild.Balance();
                }
            }
            else
            {
                if (RightChild == null)
                {
                    RightChild = new AvlTreeNode<TValue>(_comparer, value, this, _setTreeRoot);
                }
                else
                {
                    RightChild.Add(value);
                    RightChild.Balance();
                }
            }
        }
    }


    /// <summary>
    /// States of AVL tree.
    /// </summary>
    internal enum TreeState
    {
        LeftHeavy,
        RightHeavy,
        Balanced
    }
}