using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Trees;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.DataStructures.Graphs.Trees
{
    [TestClass]
    public class BinarySearchTreeTest
    {
        private BinarySearchTree<int> _tree;

        [TestInitialize]
        public void TestInitialize()
        {
            _tree = new BinarySearchTree<int>();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void EmptyTree_HasNoValues()
        {
            var elements = _tree.ToArray();

            Assert.AreEqual(0, elements.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void AddOneValue_TreeContainsOneValue()
        {
            _tree.Add(1);

            CollectionAssert.AreEqual(new [] { 1 }, _tree.ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void AddSeveralDifferentValues_TreeContainsTheseValuesInSortedOrder()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, _tree.ToArray());

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void AddSeveralSameValues_TreeContainsTheseValuesInSortedOrder()
        {
            _tree.AddRange(1, 5, 2, 2, 4, 4);

            CollectionAssert.AreEqual(new[] { 1, 2, 2, 4, 4, 5 }, _tree.ToArray());

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Contains_ReturnsTrueForTreeValues()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            Assert.IsTrue(new[] { 1, 2, 3, 4, 5 }.All(value => _tree.Contains(value)));

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Contains_ReturnsFalseForNonTreeValues()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            Assert.IsTrue(new[] { 0, -2, 6, 10 }.All(value => !_tree.Contains(value)));

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_ReturnsFalseForNonTreeValues()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            Assert.IsTrue(new[] { 0, -2, 6, 10 }.All(value => !_tree.Remove(value)));

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_ReturnsTrueForTreeValues()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            Assert.IsTrue(new[] { 1, 2, 3, 4, 5 }.All(value => _tree.Remove(value)));

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_RemovesExistingElementFromTheTree()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            _tree.Remove(1);

            Assert.IsFalse(_tree.Contains(1));

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_RemovesNodeWithNoRightChild()
        {
            _tree.AddRange(4, 2, 1, 3, 8, 6, 5, 7);

            _tree.Remove(8);

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7 }, _tree.ToArray());

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_RemovesNodeWhichRightChildHasNoLeftChild()
        {
            _tree.AddRange(4, 2, 1, 3, 6, 5, 7, 8);

            _tree.Remove(6);

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 7, 8 }, _tree.ToArray());

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_RemovesNodeWhichRightChildHaLeftChild()
        {
            _tree.AddRange(4, 2, 1, 3, 6, 5, 8, 7);

            _tree.Remove(6);

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 7, 8 }, _tree.ToArray());

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetStringPresentation_OnlyRoot()
        {
            _tree.Add(2);

            Assert.AreEqual("2", _tree.ToString());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetStringPresentation_RootAndLess()
        {
            _tree.AddRange(2, 1);

            Assert.AreEqual("   2\n1   ", _tree.ToString());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetStringPresentation_RootAndMore()
        {
            _tree.AddRange(2, 3);

            Assert.AreEqual("2   \n   3", _tree.ToString());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetStringPresentation_RootAndBoth()
        {
            _tree.AddRange(2, 1, 3);

            Assert.AreEqual("  2  \n1   3", _tree.ToString());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void FindNodeWithMinimalValue_AllDifferentValues()
        {
            _tree.AddRange(2, 1, 3, 4, 5);

            Assert.AreEqual(1, _tree.Root.FindNodeWithMinimalValue().Value);

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void FindNodeWithMinimalValue_AllSameValues()
        {
            _tree.AddRange(2, 2, 2, 2, 2);

            Assert.AreEqual(2, _tree.Root.FindNodeWithMinimalValue().Value);

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void FindNodeWithMinimalValue_SomeSameValues()
        {
            _tree.AddRange(2, 1, 3, 1, 1);

            Assert.AreEqual(1, _tree.Root.FindNodeWithMinimalValue().Value);

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void FindNodeWithMaximalValue_AllDifferentValues()
        {
            _tree.AddRange(2, 1, 3, 4, 5);

            Assert.AreEqual(5, _tree.Root.FindNodeWithMaximalValue().Value);

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void FindNodeWithMaximalValue_AllSameValues()
        {
            _tree.AddRange(2, 2, 2, 2, 2);

            Assert.AreEqual(2, _tree.Root.FindNodeWithMaximalValue().Value);

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void FindNodeWithMaximalValue_SomeSameValues()
        {
            _tree.AddRange(2, 5, 3, 1, 5);

            Assert.AreEqual(5, _tree.Root.FindNodeWithMaximalValue().Value);

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }
    }
}