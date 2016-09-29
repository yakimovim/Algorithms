using System.Collections.Generic;
using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Codes
{
    public interface IHuffmanCodeBuilder<TSymbol>
    {
        IValuedBinaryTreeNode<TSymbol> Generate(IEnumerable<TSymbol> alphabet);
    }
    
    public static class HuffmanCodeBuilder
    {
        public static IValuedBinaryTreeNode<TSymbol> Generate<TSymbol>(this IHuffmanCodeBuilder<TSymbol> builder, params TSymbol[] alphabet)
        {
            return builder.Generate(alphabet);
        }
    }
}
