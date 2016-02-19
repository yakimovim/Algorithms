using System.Linq;
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

        [TestMethod]
        public void EmptyTree_HasNoValues()
        {
            var elements = _tree.ToArray();

            Assert.AreEqual(0, elements.Length);
        }

        [TestMethod]
        public void AddOneValue_TreeContainsOneValue()
        {
            _tree.Add(1);

            CollectionAssert.AreEqual(new [] { 1 }, _tree.ToArray());
        }

        [TestMethod]
        public void AddSeveralDifferentValues_TreeContainsTheseValuesInSortedOrder()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, _tree.ToArray());
        }

        [TestMethod]
        public void AddSeveralSameValues_TreeContainsTheseValuesInSortedOrder()
        {
            _tree.AddRange(1, 5, 2, 2, 4, 4);

            CollectionAssert.AreEqual(new[] { 1, 2, 2, 4, 4, 5 }, _tree.ToArray());
        }

        [TestMethod]
        public void Contains_ReturnsTrueForTreeValues()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            Assert.IsTrue(new[] { 1, 2, 3, 4, 5 }.All(value => _tree.Contains(value)));
        }

        [TestMethod]
        public void Contains_ReturnsFalseForNonTreeValues()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            Assert.IsTrue(new[] { 0, -2, 6, 10 }.All(value => !_tree.Contains(value)));
        }

        [TestMethod]
        public void Remove_ReturnsFalseForNonTreeValues()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            Assert.IsTrue(new[] { 0, -2, 6, 10 }.All(value => !_tree.Remove(value)));
        }

        [TestMethod]
        public void Remove_ReturnsTrueForTreeValues()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            Assert.IsTrue(new[] { 1, 2, 3, 4, 5 }.All(value => _tree.Remove(value)));
        }

        [TestMethod]
        public void Remove_RemovesExistingElementFromTheTree()
        {
            _tree.AddRange(1, 5, 2, 3, 4);

            _tree.Remove(1);

            Assert.IsFalse(_tree.Contains(1));
        }
    }
}