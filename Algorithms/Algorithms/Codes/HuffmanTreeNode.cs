using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Codes
{
    internal class HuffmanTreeNode<TSymbol> : IBinaryTreeNode<TSymbol>
    {
        public HuffmanTreeNode(TSymbol symbol, double frequency)
        {
            Value = symbol;
            Frequency = frequency;
        }

        public double Frequency { get; }

        public TSymbol Value { get; }

        public IBinaryTreeNode<TSymbol> LeftChild { get; set; }

        public IBinaryTreeNode<TSymbol> RightChild { get; set; }
    }
}
