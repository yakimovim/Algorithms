using System;
using EdlinSoftware.Algorithms.Collections.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Sorting
{
    [TestClass]
    public class QuickSorterTest
    {
        private QuickSorter<int> _sorter;

        [TestInitialize]
        public void TestInitialize()
        {
            _sorter = QuickSorter.New<int>((arr, left, right) => left);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Sort_ShouldThrowException_IfArrayIsNull()
        {
            _sorter.Sort(null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldLeaveEmptyArray()
        {
            _sorter.Sort(new int[0]);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldLeaveArrayWithOneElement()
        {
            var array = new[] { 1 };

            _sorter.Sort(array);

            Assert.AreEqual(1, array[0]);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldSortArray_WithTwoElement()
        {
            var array = new[] { 2, 1 };

            _sorter.Sort(array);

            CollectionAssert.AreEqual(new[] { 1, 2 }, array);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldSortArray_WithSeveralElements()
        {
            var array = new[] { 2, 1, 8, 3, 5, 4, 0, 9, 6, 7 };

            _sorter.Sort(array);

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, array);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldLeaveSortedArrayIntact()
        {
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            _sorter.Sort(array);

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, array);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Sort_ShouldSortArray_WithEqualElementsElements()
        {
            var array = new[] { 2, 1, 8, 2, 5, 4, 0, 9, 9, 7, 5 };

            _sorter.Sort(array);

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 2, 4, 5, 5, 7, 8, 9, 9 }, array);
        }
    }
}
