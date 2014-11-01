using System;
using System.Collections.Generic;
using EdlinSoftware.Algorithms.Codes;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.Tests.DataStructures.Codes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Codes
{
    [TestClass]
    public abstract class HuffmanCodeBuilderTest<TBuilder>
        where TBuilder : IHuffmanCodeBuilder<SymbolAndFrequency<char>>
    {
        protected TBuilder _builder;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Generate_ShouldThrowException_IfThereAreNoSymbolsInTheAlphabet()
        {
            _builder.Generate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Generate_ShouldThrowException_IfThereIsOneSymbolInTheAlphabet()
        {
            var tree = _builder.Generate(new SymbolAndFrequency<char>('a', 1.0));
        }

        [TestMethod]
        public void Generate_ShouldReturnCorrectTree_IfThereAreTwoSymbolsInTheAlphabet()
        {
            var tree = _builder.Generate(new SymbolAndFrequency<char>('a', 0.4),
                new SymbolAndFrequency<char>('b', 0.6));

            ValidateTree(tree, new object[,] { { 'a', 1 }, { 'b', 1 } });
        }

        [TestMethod]
        public void Generate_ShouldReturnCorrectLadderTree()
        {
            var tree = _builder.Generate(new SymbolAndFrequency<char>('a', 0.87),
                new SymbolAndFrequency<char>('b', 0.1),
                new SymbolAndFrequency<char>('c', 0.02),
                new SymbolAndFrequency<char>('d', 0.01));

            ValidateTree(tree, new object[,] { { 'a', 1 }, { 'b', 2 }, { 'c', 3 }, { 'd', 3 } });
        }

        [TestMethod]
        public void Generate_ShouldReturnCorrectBalancedTree()
        {
            var tree = _builder.Generate(new SymbolAndFrequency<char>('a', 0.3),
                new SymbolAndFrequency<char>('b', 0.3),
                new SymbolAndFrequency<char>('c', 0.2),
                new SymbolAndFrequency<char>('d', 0.2));

            ValidateTree(tree, new object[,] { { 'a', 2 }, { 'b', 2 }, { 'c', 2 }, { 'd', 2 } });
        }

        private void ValidateTree(IBinaryTreeNode<SymbolAndFrequency<char>> tree, object[,] expected)
        {
            var symbolDepth = new Dictionary<char, long>();

            GetSymbolsDepth(tree, 0, symbolDepth);

            for (int i = 0; i < expected.GetLength(0); i++)
            {
                var symbol = Convert.ToChar(expected[i, 0]);
                var depth = Convert.ToInt64(expected[i, 1]);

                Assert.IsTrue(symbolDepth.ContainsKey(symbol));
                Assert.AreEqual(depth, symbolDepth[symbol]);
            }
        }

        private void GetSymbolsDepth(IBinaryTreeNode<SymbolAndFrequency<char>> node, long level, Dictionary<char, long> symbolDepth)
        {
            if (node.IsLeaf())
            {
                symbolDepth[node.Value.Symbol] = level;
                return;
            }

            if (node.LeftChild != null)
                GetSymbolsDepth(node.LeftChild, level + 1, symbolDepth);
            if (node.RightChild != null)
                GetSymbolsDepth(node.RightChild, level + 1, symbolDepth);
        }
    }
}
