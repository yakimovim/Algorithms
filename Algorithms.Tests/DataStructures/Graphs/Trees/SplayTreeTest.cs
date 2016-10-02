using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Trees;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.DataStructures.Graphs.Trees
{
    [TestClass]
    public class SplayTreeTest
    {
        private SplayTree<int> _tree;

        [TestInitialize]
        public void TestInitialize()
        {
            _tree = new SplayTree<int>();
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

            CollectionAssert.AreEqual(new[] { 1 }, _tree.ToArray());
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
        public void TreeIsBalancedAfterEachAdd()
        {
            for (int i = 0; i < 100; i++)
            {
                _tree.Add(i);
            }

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
        public void TreeIsBalancedAfterEachRemove_DirectOrder()
        {
            _tree.AddRange(Enumerable.Range(0, 100));

            for (int i = 0; i < 100; i++)
            {
                _tree.Remove(i);
            }

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TreeIsBalancedAfterEachRemove_InverseOrder()
        {
            _tree.AddRange(Enumerable.Range(0, 100));

            for (int i = 99; i >= 0; i--)
            {
                _tree.Remove(i);
            }

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TreeIsBalancedAfterEachRemove_RandomOrder()
        {
            var rnd = new Random((int)DateTime.UtcNow.Ticks);

            var numbers = Enumerable.Range(0, 200).ToList();

            _tree.AddRange(numbers);

            for (int i = 0; i < 200; i++)
            {
                var index = rnd.Next(numbers.Count);
                var value = numbers[index];
                numbers.RemoveAt(index);

                _tree.Remove(value);
            }

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
        public void Remove_RightRotation()
        {
            _tree.AddRange(2, 1, 3, 4);

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, _tree.ToArray());

            _tree.Root.CheckBinarySearchTree(Comparer<int>.Default);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Next_DifferentValues()
        {
            _tree.AddRange(4, 2, 1, 3, 6, 5, 8, 7);

            var node = _tree.Root.FindNodeWithMinimalValue();

            var values = new List<int> { node.Value };
            values.AddRange(Enumerable.Range(0, 7).Select(i =>
            {
                node = node.Next();
                return node.Value;
            }));
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, values.ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Next_SameValues()
        {
            _tree.AddRange(2, 2, 2, 2);

            var node = _tree.Root.FindNodeWithMinimalValue();

            var values = new List<int> { node.Value };
            values.AddRange(Enumerable.Range(0, 3).Select(i =>
            {
                node = node.Next();
                return node.Value;
            }));
            CollectionAssert.AreEqual(new[] { 2, 2, 2, 2 }, values.ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Next_SomeDifferentValues()
        {
            _tree.AddRange(4, 2, 1, 2, 7, 2, 8, 7);

            var node = _tree.Root.FindNodeWithMinimalValue();

            var values = new List<int> { node.Value };
            values.AddRange(Enumerable.Range(0, 7).Select(i =>
            {
                node = node.Next();
                return node.Value;
            }));
            CollectionAssert.AreEqual(new[] { 1, 2, 2, 2, 4, 7, 7, 8 }, values.ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Next_OfMaximal_ReturnsNull()
        {
            _tree.AddRange(7, 7, 7);

            var node = _tree.Root.FindNodeWithMaximalValue();

            Assert.IsNull(node.Next());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Previous_DifferentValues()
        {
            _tree.AddRange(4, 2, 1, 3, 6, 5, 8, 7);

            var node = _tree.Root.FindNodeWithMaximalValue();

            var values = new List<int> { node.Value };
            values.AddRange(Enumerable.Range(0, 7).Select(i =>
            {
                node = node.Previous();
                return node.Value;
            }));
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, values.ToArray().Reverse().ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Previous_SameValues()
        {
            _tree.AddRange(2, 2, 2, 2);

            var node = _tree.Root.FindNodeWithMaximalValue();

            var values = new List<int> { node.Value };
            values.AddRange(Enumerable.Range(0, 3).Select(i =>
            {
                node = node.Previous();
                return node.Value;
            }));
            CollectionAssert.AreEqual(new[] { 2, 2, 2, 2 }, values.ToArray().Reverse().ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Previous_SomeDifferentValues()
        {
            _tree.AddRange(4, 2, 1, 2, 7, 2, 8, 7);

            var node = _tree.Root.FindNodeWithMaximalValue();

            var values = new List<int> { node.Value };
            values.AddRange(Enumerable.Range(0, 7).Select(i =>
            {
                node = node.Previous();
                return node.Value;
            }));
            CollectionAssert.AreEqual(new[] { 1, 2, 2, 2, 4, 7, 7, 8 }, values.ToArray().Reverse().ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Previous_OfMinimal_ReturnsNull()
        {
            _tree.AddRange(1, 1, 1);

            var node = _tree.Root.FindNodeWithMinimalValue();

            Assert.IsNull(node.Previous());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void RangeSearch_NoData()
        {
            _tree.AddRange(1, 2, 6, 7, 8);

            var range = _tree.Root.RangeSearch(3, 5, Comparer<int>.Default);

            Assert.AreEqual(0, range.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void RangeSearch_ExcludeBorders_DifferentData()
        {
            _tree.AddRange(1, 2, 3, 4, 5, 6, 7, 8, 9);

            var range = _tree.Root.RangeSearch(3, 7, Comparer<int>.Default);

            CollectionAssert.AreEqual(new[] { 4, 5, 6 }, range);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void RangeSearch_IncludeBorders_DifferentData()
        {
            _tree.AddRange(1, 2, 3, 4, 5, 6, 7, 8, 9);

            var range = _tree.Root.RangeSearch(3, 7, Comparer<int>.Default, true, true);

            CollectionAssert.AreEqual(new[] { 3, 4, 5, 6, 7 }, range);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void RangeSearch_ExcludeBorders_SameData()
        {
            _tree.AddRange(3, 3, 3, 3, 3, 3, 4, 5, 6, 7, 7, 7, 7, 7, 7);

            var range = _tree.Root.RangeSearch(3, 7, Comparer<int>.Default);

            CollectionAssert.AreEqual(new[] { 4, 5, 6 }, range);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void RangeSearch_IncludeBorders_SameData()
        {
            _tree.AddRange(1, 2, 3, 3, 3, 3, 3, 3, 4, 5, 6, 7, 7, 7, 7, 7, 7, 8, 9);

            var range = _tree.Root.RangeSearch(3, 7, Comparer<int>.Default, true, true);

            CollectionAssert.AreEqual(new[] { 3, 3, 3, 3, 3, 3, 4, 5, 6, 7, 7, 7, 7, 7, 7 }, range);
        }

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_TreesWithDifferentValues()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1,2,3,4
        //    };
        //    var rightTree = new AvlTree<int>
        //    {
        //        5,6,7,8
        //    };

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_TreesWithSameValues()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1,2,2,3,4,4,4
        //    };
        //    var rightTree = new AvlTree<int>
        //    {
        //        5,5,6,7,7,7,8,8
        //    };

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 2, 3, 4, 4, 4, 5, 5, 6, 7, 7, 7, 8, 8 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_LeftTreeIsSmaller()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1,2
        //    };
        //    var rightTree = new AvlTree<int>
        //    {
        //        3,3,3,3,4,4,4,5,6,6,6,6,6,7,8,8,8
        //    };

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 3, 3, 3, 3, 4, 4, 4, 5, 6, 6, 6, 6, 6, 7, 8, 8, 8 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_LeftTreeHasOneElement()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1
        //    };
        //    var rightTree = new AvlTree<int>
        //    {
        //        3,3,3,3,4,4,4,5,6,6,6,6,6,7,8,8,8
        //    };

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 3, 3, 3, 3, 4, 4, 4, 5, 6, 6, 6, 6, 6, 7, 8, 8, 8 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_LeftTreeIsEmpty()
        //{
        //    var leftTree = new AvlTree<int>();
        //    var rightTree = new AvlTree<int>
        //    {
        //        3,3,3,3,4,4,4,5,6,6,6,6,6,7,8,8,8
        //    };

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 3, 3, 3, 3, 4, 4, 4, 5, 6, 6, 6, 6, 6, 7, 8, 8, 8 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_RightTreeIsSmaller()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1,2,3,3,3,3,4,4,4,5,6,6,6,6,6,7,8
        //    };
        //    var rightTree = new AvlTree<int>
        //    {
        //        8,8
        //    };

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 3, 3, 3, 3, 4, 4, 4, 5, 6, 6, 6, 6, 6, 7, 8, 8, 8 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_RightTreeHasOneElement()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1,2,3,3,3,3,4,4,4,5,6,6,6,6,6,7,8
        //    };
        //    var rightTree = new AvlTree<int>
        //    {
        //        8
        //    };

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 3, 3, 3, 3, 4, 4, 4, 5, 6, 6, 6, 6, 6, 7, 8, 8 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_RightTreeIsEmpty()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1,2,3,3,3,3,4,4,4,5,6,6,6,6,6,7,8
        //    };
        //    var rightTree = new AvlTree<int>();

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 3, 3, 3, 3, 4, 4, 4, 5, 6, 6, 6, 6, 6, 7, 8 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_OneElementInEachTree()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1
        //    };
        //    var rightTree = new AvlTree<int>
        //    {
        //        1
        //    };

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 1 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_EmptyTrees()
        //{
        //    var leftTree = new AvlTree<int>();
        //    var rightTree = new AvlTree<int>();

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new int[0], mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_OnlyOneElementInLeftTree()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1
        //    };
        //    var rightTree = new AvlTree<int>();

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_OnlyOneElementInRightTree()
        //{
        //    var leftTree = new AvlTree<int>();
        //    var rightTree = new AvlTree<int>
        //    {
        //        1
        //    };


        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Merge_OverlappingTrees()
        //{
        //    var leftTree = new AvlTree<int>
        //    {
        //        1,2,2,3,4,4,4,5,5
        //    };
        //    var rightTree = new AvlTree<int>
        //    {
        //        5,5,6,7,7,7,8,8
        //    };

        //    var mergedTree = AvlTree.Merge(leftTree, rightTree, Comparer<int>.Default);

        //    Assert.IsTrue(mergedTree.Root.IsBalanced());
        //    mergedTree.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 2, 3, 4, 4, 4, 5, 5, 5, 5, 6, 7, 7, 7, 8, 8 }, mergedTree.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Split_TreeWithDifferentValues()
        //{
        //    _tree.AddRange(1, 2, 3, 4, 6, 7, 8, 9);

        //    var splitTrees = AvlTree.Split(_tree, 5, Comparer<int>.Default);

        //    splitTrees.Item1.Root.CheckBinarySearchTree(Comparer<int>.Default);
        //    splitTrees.Item2.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, splitTrees.Item1.ToArray());
        //    CollectionAssert.AreEqual(new[] { 6, 7, 8, 9 }, splitTrees.Item2.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Split_TreeWithSameValues()
        //{
        //    _tree.AddRange(1, 2, 2, 3, 4, 5, 5, 5, 6, 7, 8, 9, 9);

        //    var splitTrees = AvlTree.Split(_tree, 5, Comparer<int>.Default);

        //    splitTrees.Item1.Root.CheckBinarySearchTree(Comparer<int>.Default);
        //    splitTrees.Item2.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 2, 3, 4, 5, 5, 5 }, splitTrees.Item1.ToArray());
        //    CollectionAssert.AreEqual(new[] { 6, 7, 8, 9, 9 }, splitTrees.Item2.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Split_ByLower()
        //{
        //    _tree.AddRange(1, 2, 2, 3, 4, 5, 5, 5, 6, 7, 8, 9, 9);

        //    var splitTrees = AvlTree.Split(_tree, 0, Comparer<int>.Default);

        //    splitTrees.Item1.Root.CheckBinarySearchTree(Comparer<int>.Default);
        //    splitTrees.Item2.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new int[0], splitTrees.Item1.ToArray());
        //    CollectionAssert.AreEqual(new[] { 1, 2, 2, 3, 4, 5, 5, 5, 6, 7, 8, 9, 9 }, splitTrees.Item2.ToArray());
        //}

        //[TestMethod, Owner("Ivan Yakimov")]
        //public void Split_ByUpper()
        //{
        //    _tree.AddRange(1, 2, 2, 3, 4, 5, 5, 5, 6, 7, 8, 9, 9);

        //    var splitTrees = AvlTree.Split(_tree, 10, Comparer<int>.Default);

        //    splitTrees.Item1.Root.CheckBinarySearchTree(Comparer<int>.Default);
        //    splitTrees.Item2.Root.CheckBinarySearchTree(Comparer<int>.Default);

        //    CollectionAssert.AreEqual(new[] { 1, 2, 2, 3, 4, 5, 5, 5, 6, 7, 8, 9, 9 }, splitTrees.Item1.ToArray());
        //    CollectionAssert.AreEqual(new int[0], splitTrees.Item2.ToArray());
        //}
    }
}