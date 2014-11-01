﻿using System;
using EdlinSoftware.Algorithms.Collections.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Sorting
{
    [TestClass]
    public class MergerTest
    {
        private Merger _merger;

        [TestInitialize]
        public void TestInitialize()
        {
            _merger = new Merger();
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Merge_ShouldThrowException_IfFirstArrayIsNull()
        {
            _merger.Merge(null, new[] { 1 });
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Merge_ShouldThrowException_IfSecondArrayIsNull()
        {
            _merger.Merge(new[] { 1 }, null);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Merge_ShouldReturnEmptyArray_IfBothArraysAreEmpty()
        {
            var merged = _merger.Merge(new int[0], new int[0]);

            Assert.AreEqual(0, merged.Length);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Merge_ShouldReturnFirstArray_IfSecondArrayIsEmpty()
        {
            var merged = _merger.Merge(new[] { 1, 2, 3 }, new int[0]);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, merged);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Merge_ShouldReturnSecondArray_IfFirstArrayIsEmpty()
        {
            var merged = _merger.Merge(new int[0], new[] { 1, 2, 3 });

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, merged);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Merge_ShouldMergeTwoNonEmptyArrays_OfTheSameLength()
        {
            var merged = _merger.Merge(new[] { 1, 3, 5, 7 }, new[] { 2, 4, 6, 8 });

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, merged);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Merge_ShouldMergeTwoNonEmptyArrays_IfFirstArrayIsShorter()
        {
            var merged = _merger.Merge(new[] { 1, 3 }, new[] { 2, 4, 5, 6, 7, 8 });

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, merged);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Merge_ShouldMergeTwoNonEmptyArrays_IfSecondArrayIsShorter()
        {
            var merged = _merger.Merge(new[] { 1, 3, 5, 6, 7, 8 }, new[] { 2, 4 });

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, merged);
        }
    }
}
