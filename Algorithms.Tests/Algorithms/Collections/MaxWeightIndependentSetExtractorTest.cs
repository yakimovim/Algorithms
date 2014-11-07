using System;
using EdlinSoftware.Algorithms.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections
{
    [TestClass]
    public class MaxWeightIndependentSetExtractorTest
    {
        private MaxWeightIndependentSetExtractor _extractor;

        [TestInitialize]
        public void TestInitialize()
        {
            _extractor = new MaxWeightIndependentSetExtractor();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Extract_ShouldThrowException_IfInputIsNull()
        {
            _extractor.Extract(null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Extract_ShouldReturnEmptySet_IfInputIsEmpty()
        {
            var independentSet = _extractor.Extract(new double[0]);

            Assert.AreEqual(0, independentSet.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Extract_ShouldReturnTheOnlyElement_IfInputHasOneElement()
        {
            var independentSet = _extractor.Extract(new[] { 10.0 });

            CollectionAssert.AreEqual(new long[] { 0 }, independentSet);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Extract_ShouldReturnTheBiggestElement_IfInputHasTwoElements()
        {
            var independentSet = _extractor.Extract(new[] { 5.0, 10.0 });

            CollectionAssert.AreEqual(new long[] { 1 }, independentSet);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Extract_ShouldReturnCorrectResult_IfInputIsArbitrary_1()
        {
            var independentSet = _extractor.Extract(new[] { 1.0, 4.0, 5.0, 4.0 });

            CollectionAssert.AreEqual(new long[] { 1, 3 }, independentSet);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Extract_ShouldReturnCorrectResult_IfInputIsArbitrary_2()
        {
            var independentSet = _extractor.Extract(new[] { 4.0, 1.0, 5.0, 4.0 });

            CollectionAssert.AreEqual(new long[] { 0, 2 }, independentSet);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Extract_ShouldReturnCorrectResult_IfInputIsArbitrary_3()
        {
            var independentSet = _extractor.Extract(new[] { 4.0, 1.0, 4.0, 5.0 });

            CollectionAssert.AreEqual(new long[] { 0, 3 }, independentSet);
        }
    }
}
