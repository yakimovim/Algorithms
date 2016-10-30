using System;
using EdlinSoftware.Algorithms.Collections.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Sorting
{
    [TestClass]
    public class ThreeWayPartitionerTest
    {
        private ThreeWayPartitioner<int> _partitioner;

        [TestInitialize]
        public void TestInitialize()
        {
            _partitioner = ThreeWayPartitioner.New<int>();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Partition_ShouldThrowException_IfArrayIsNull()
        {
            _partitioner.Partition(null, 0, 10, 5);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Partition_ShouldThrowException_IfLeftIndexIsOutOfArray()
        {
            _partitioner.Partition(new int[10], -3, 10, 5);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Partition_ShouldThrowException_IfRightIndexIsOutOfArray()
        {
            _partitioner.Partition(new int[10], 0, 10, 5);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Partition_ShouldThrowException_IfLeftIndexIsNotSmallerThenRight()
        {
            _partitioner.Partition(new int[10], 8, 2, 5);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Partition_ShouldThrowException_IfPivotIndexIsNotBetweenLeftAndRight()
        {
            _partitioner.Partition(new int[10], 2, 5, 7);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldNotChangePositionOfTheOnlyElement_IfArrayContainsOneElement()
        {
            var partitionPositions = _partitioner.Partition(new[] { 1 }, 0, 0, 0);

            Assert.AreEqual(-1, partitionPositions.Item1);
            Assert.AreEqual(1, partitionPositions.Item2);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldNotChangePositionOfTheOnlyElement_IfArrayContainsSeveralElements()
        {
            var partitionPositions = _partitioner.Partition(new[] { 1, 2, 3 }, 1, 1, 1);

            Assert.AreEqual(0, partitionPositions.Item1);
            Assert.AreEqual(2, partitionPositions.Item2);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldPartitionWholeArray_IfPivotElementIsTheFirst()
        {
            var array = new[] { 4, 2, 1, 8, 6, 3, 7, 0, 5 };

            var partitionPositions = _partitioner.Partition(array, 0, 8, 0);

            Assert.AreEqual(3, partitionPositions.Item1);
            Assert.AreEqual(5, partitionPositions.Item2);

            CheckArrayIsPartitioned(array, partitionPositions);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldPartitionWholeArray_IfPivotElementIsTheLast()
        {
            var array = new[] { 5, 2, 1, 8, 6, 3, 7, 0, 4 };

            var partitionPositions = _partitioner.Partition(array, 0, 8, 8);

            Assert.AreEqual(3, partitionPositions.Item1);
            Assert.AreEqual(5, partitionPositions.Item2);

            CheckArrayIsPartitioned(array, partitionPositions);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldPartitionWholeArray_IfPivotElementIsInTheMiddle()
        {
            var array = new[] { 5, 2, 1, 8, 4, 3, 7, 0, 6 };

            var partitionPositions = _partitioner.Partition(array, 0, 8, 4);

            Assert.AreEqual(3, partitionPositions.Item1);
            Assert.AreEqual(5, partitionPositions.Item2);

            CheckArrayIsPartitioned(array, partitionPositions);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldPartitionPartOfArray_IfPivotElementIsTheFirst()
        {
            var array = new[] { 0, 4, 2, 3, 1, 5, 6, 7, 8, 9 };

            var partitionPositions = _partitioner.Partition(array, 1, 8, 1);

            Assert.AreEqual(3, partitionPositions.Item1);
            Assert.AreEqual(5, partitionPositions.Item2);

            CheckArrayIsPartitioned(array, partitionPositions);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldPartitionPartOfArray_IfPivotElementIsTheLast()
        {
            var array = new[] { 0, 1, 2, 3, 8, 5, 6, 7, 4, 9 };

            var partitionPositions = _partitioner.Partition(array, 1, 8, 8);

            Assert.AreEqual(3, partitionPositions.Item1);
            Assert.AreEqual(5, partitionPositions.Item2);

            CheckArrayIsPartitioned(array, partitionPositions);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldPartitionPartOfArray_IfPivotElementIsInTheMiddle()
        {
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var partitionPositions = _partitioner.Partition(array, 1, 8, 4);

            Assert.AreEqual(3, partitionPositions.Item1);
            Assert.AreEqual(5, partitionPositions.Item2);

            CheckArrayIsPartitioned(array, partitionPositions);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldPartitionPartOfArray_IfArrayConsistsOfSameElements()
        {
            var array = new[] { 1, 1, 1 };

            var partitionPositions = _partitioner.Partition(array, 0, 2, 1);

            Assert.AreEqual(-1, partitionPositions.Item1);
            Assert.AreEqual(3, partitionPositions.Item2);

            CheckArrayIsPartitioned(array, partitionPositions);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Partition_ShouldPartitionPartOfArray_IfArrayContainsSameElements()
        {
            var array = new[] { 0, 1, 7, 4, 4, 4, 6, 2, 8, 9 };

            var partitionPositions = _partitioner.Partition(array, 1, 8, 4);

            Assert.AreEqual(2, partitionPositions.Item1);
            Assert.AreEqual(6, partitionPositions.Item2);

            CheckArrayIsPartitioned(array, partitionPositions);
        }

        private static void CheckArrayIsPartitioned(int[] array, Tuple<int, int> partitionPositions)
        {
            var pivotElement = array[partitionPositions.Item1 + 1];

            for (int i = 0; i < array.Length; i++)
            {
                if (i <= partitionPositions.Item1)
                    Assert.IsTrue(array[i] < pivotElement);
                else if(i >= partitionPositions.Item2)
                    Assert.IsTrue(array[i] > pivotElement);
                else
                    Assert.IsTrue(array[i] == pivotElement);
            }
        }
    }
}
