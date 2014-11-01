using System.Collections.Generic;
using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Codes
{
    public interface IHuffmanCodeBuilder<TSymbol>
    {
        IBinaryTreeNode<TSymbol> Generate(IEnumerable<TSymbol> alphabet);
    }
    
    public static class HuffmanCodeBuilder
    {
        public static IBinaryTreeNode<TSymbol> Generate<TSymbol>(this IHuffmanCodeBuilder<TSymbol> builder, params TSymbol[] alphabet)
        {
            return builder.Generate((IEnumerable<TSymbol>)alphabet);
        }
    }
}
