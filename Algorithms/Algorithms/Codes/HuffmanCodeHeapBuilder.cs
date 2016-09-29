using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Heaps;

namespace EdlinSoftware.Algorithms.Codes
{
    /// <summary>
    /// Represents builder of Huffman's code tree using heap.
    /// </summary>
    public class HuffmanCodeHeapBuilder<TSymbol> : IHuffmanCodeBuilder<TSymbol>
    {
        private readonly Func<TSymbol, double> _frequencyProvider;

        [DebuggerStepThrough]
        public HuffmanCodeHeapBuilder(Func<TSymbol, double> frequencyProvider)
        {
            if (frequencyProvider == null) throw new ArgumentNullException(nameof(frequencyProvider));
            _frequencyProvider = frequencyProvider;
        }

        public IBinaryTreeNode<TSymbol> Generate(IEnumerable<TSymbol> alphabet)
        {
            if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));

            var heap = new Heap<double, HuffmanTreeNode<TSymbol>>(Comparer<double>.Default,
                (from a in alphabet
                 let node = new HuffmanTreeNode<TSymbol>(a, _frequencyProvider(a))
                 select new HeapElement<double, HuffmanTreeNode<TSymbol>>(node.Frequency, node)).ToArray());
            if (heap.Count <= 1) throw new ArgumentNullException(nameof(alphabet), "Alphabet should contain at least two symbols.");

            while (heap.Count > 1)
            {
                var node1 = heap.ExtractMinElement().Value;
                var node2 = heap.ExtractMinElement().Value;

                var newNode = new HuffmanTreeNode<TSymbol>(default(TSymbol), node1.Frequency + node2.Frequency);
                newNode.LeftChild = node1;
                newNode.RightChild = node2;
                heap.Add(newNode.Frequency, newNode);
            }

            return heap.ExtractMinElement().Value;
        }
    }
}
