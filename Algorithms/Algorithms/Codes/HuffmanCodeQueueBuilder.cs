using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EdlinSoftware.Algorithms.Collections.Sorting;
using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Codes
{
    /// <summary>
    /// Represents builder of Huffman's code tree using queues.
    /// </summary>
    public class HuffmanCodeQueueBuilder<TSymbol> : IHuffmanCodeBuilder<TSymbol>
    {
        private class HuffmanTreeNodeComparer : IComparer<HuffmanTreeNode<TSymbol>>
        {
            public int Compare(HuffmanTreeNode<TSymbol> x, HuffmanTreeNode<TSymbol> y)
            {
                return x.Frequency.CompareTo(y.Frequency);
            }
        }

        private readonly Func<TSymbol, double> _frequencyProvider;

        [DebuggerStepThrough]
        public HuffmanCodeQueueBuilder(Func<TSymbol, double> frequencyProvider)
        {
            if (frequencyProvider == null) throw new ArgumentNullException(nameof(frequencyProvider));
            _frequencyProvider = frequencyProvider;
        }

        public IBinaryTreeNode<TSymbol> Generate(IEnumerable<TSymbol> alphabet)
        {
            if (alphabet == null || alphabet.Any() == false) throw new ArgumentNullException(nameof(alphabet));


            var queue1 = new Queue<HuffmanTreeNode<TSymbol>>(GetSortedAlphabet(alphabet));
            var queue2 = new Queue<HuffmanTreeNode<TSymbol>>();

            var numberOfSymbols = queue1.Count;

            for (int i = 0; i < numberOfSymbols - 1; i++)
            {
                var node1 = GetLeastFrequentItem(queue1, queue2);
                var node2 = GetLeastFrequentItem(queue1, queue2);

                var newNode = new HuffmanTreeNode<TSymbol>(default(TSymbol), node1.Frequency + node2.Frequency)
                {
                    LeftChild = node1,
                    RightChild = node2
                };
                queue2.Enqueue(newNode);
            }

            return queue2.Peek();
        }

        private IEnumerable<HuffmanTreeNode<TSymbol>> GetSortedAlphabet(IEnumerable<TSymbol> alphabet)
        {
            var symbolsArray = alphabet.Select(a => new HuffmanTreeNode<TSymbol>(a, _frequencyProvider(a))).ToArray();

            if (symbolsArray.Length < 2)
            { throw new ArgumentNullException(nameof(alphabet), "Alphabet should contain at least two symbols."); }

            var rnd = new Random((int)DateTime.UtcNow.Ticks);

            var pivotSelector = new Func<IList<HuffmanTreeNode<TSymbol>>, int, int, int>((list, left, right) => 1 + rnd.Next(left - 1, right));

            var sorter = new QuickSorter<HuffmanTreeNode<TSymbol>>(pivotSelector, new HuffmanTreeNodeComparer());

            sorter.Sort(symbolsArray);

            return symbolsArray;
        }

        private HuffmanTreeNode<TSymbol> GetLeastFrequentItem(Queue<HuffmanTreeNode<TSymbol>> queue1, Queue<HuffmanTreeNode<TSymbol>> queue2)
        {
            if (queue1.Count == 0)
                return queue2.Dequeue();
            if (queue2.Count == 0)
                return queue1.Dequeue();

            var node1 = queue1.Peek();
            var node2 = queue2.Peek();

            if (node1.Frequency < node2.Frequency)
                return queue1.Dequeue();

            return queue2.Dequeue();
        }
    }
}
