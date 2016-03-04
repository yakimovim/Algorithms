using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Codes
{
    internal class HuffmanTreeNode<TSymbol> : BinaryTreeNodeBase<HuffmanTreeNode<TSymbol>, TSymbol>
    {
        public HuffmanTreeNode(TSymbol symbol, double frequency)
        {
            Value = symbol;
            Frequency = frequency;
        }

        public double Frequency { get; }
    }
}
