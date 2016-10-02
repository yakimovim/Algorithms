using System.Diagnostics;

namespace EdlinSoftware.DataStructures.Graphs.Trees
{
    public class SplayTreeNode<TValue> : BinaryTreeNodeBase<SplayTreeNode<TValue>, TValue>, IParented<SplayTreeNode<TValue>>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SplayTreeNode<TValue> _leftChild;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SplayTreeNode<TValue> _rightChild;

        public SplayTreeNode(
            TValue value,
            SplayTreeNode<TValue> parent)
        {
            Parent = parent;
            Value = value;
        }

        public SplayTreeNode<TValue> Parent { get; internal set; }

        public override SplayTreeNode<TValue> LeftChild
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

        public override SplayTreeNode<TValue> RightChild
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
    }
}