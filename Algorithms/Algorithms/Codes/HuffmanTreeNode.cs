using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Codes
{
    internal class HuffmanTreeNode<TSymbol> : BinaryTreeNodeBase<HuffmanTreeNode<TSymbol>, TSymbol>, IValuedBinaryTreeNode<TSymbol>
    {
        public HuffmanTreeNode(TSymbol symbol, double frequency)
        {
            Value = symbol;
            Frequency = frequency;
        }

        public double Frequency { get; }

        IValuedBinaryTreeNode<TSymbol> IBinaryTreeNode<IValuedBinaryTreeNode<TSymbol>>.LeftChild => LeftChild;

        IValuedBinaryTreeNode<TSymbol> IBinaryTreeNode<IValuedBinaryTreeNode<TSymbol>>.RightChild => RightChild;
    }
}
