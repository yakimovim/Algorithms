using System;
using EdlinSoftware.Algorithms.Collections.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Sorting
{
    [TestClass]
    public class MergeSorterTest
    {
        private MergeSorter _sorter;

        [TestInitialize]
        public void TestInitialize()
        {
            _sorter = new MergeSorter();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Sort_ShouldThrowException_IfArrayIsEmpty()
        {
            _sorter.Sort<int>(null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldReturnSameArray_IfInputArrayIsEmpty()
        {
            var sorted = _sorter.Sort(new int[0]);

            Assert.AreEqual(0, sorted.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldReturnSameArray_IfInputArrayHasOneElement()
        {
            var sorted = _sorter.Sort(new[] { 1 });

            CollectionAssert.AreEqual(new[] { 1 }, sorted);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldSortArray_OfEvenLength()
        {
            var sorted = _sorter.Sort(new[] { 1, 3, 2, 8, 5, 4, 6, 7 });

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, sorted);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldSortArray_OfOddLength()
        {
            var sorted = _sorter.Sort(new[] { 1, 3, 2, 5, 4, 6, 7 });

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7 }, sorted);
        }
    }
}
