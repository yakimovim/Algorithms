using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Codes
{
    internal class HuffmanTreeNode<TSymbol> : BinaryTreeNodeBase<HuffmanTreeNode<TSymbol>, TSymbol>, IBinaryTreeNode<TSymbol>
    {
        public HuffmanTreeNode(TSymbol symbol, double frequency)
        {
            Value = symbol;
            Frequency = frequency;
        }

        public double Frequency { get; }

        IBinaryTreeNode<TSymbol> IBinaryTreeNode<TSymbol, IBinaryTreeNode<TSymbol>>.LeftChild => LeftChild;

        IBinaryTreeNode<TSymbol> IBinaryTreeNode<TSymbol, IBinaryTreeNode<TSymbol>>.RightChild => RightChild;
    }
}
