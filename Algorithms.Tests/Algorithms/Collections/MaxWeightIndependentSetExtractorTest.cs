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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Extract_ShouldThrowException_IfInputIsNull()
        {
            _extractor.Extract(null);
        }

        [TestMethod]
        public void Extract_ShouldReturnEmptySet_IfInputIsEmpty()
        {
            var independentSet = _extractor.Extract(new double[0]);

            Assert.AreEqual(0, independentSet.Length);
        }

        [TestMethod]
        public void Extract_ShouldReturnTheOnlyElement_IfInputHasOneElement()
        {
            var independentSet = _extractor.Extract(new double[] { 10.0 });

            CollectionAssert.AreEqual(new long[] { 0 }, independentSet);
        }

        [TestMethod]
        public void Extract_ShouldReturnTheBiggestElement_IfInputHasTwoElements()
        {
            var independentSet = _extractor.Extract(new double[] { 5.0, 10.0 });

            CollectionAssert.AreEqual(new long[] { 1 }, independentSet);
        }

        [TestMethod]
        public void Extract_ShouldReturnCorrectResult_IfInputIsArbitrary_1()
        {
            var independentSet = _extractor.Extract(new double[] { 1.0, 4.0, 5.0, 4.0 });

            CollectionAssert.AreEqual(new long[] { 1, 3 }, independentSet);
        }

        [TestMethod]
        public void Extract_ShouldReturnCorrectResult_IfInputIsArbitrary_2()
        {
            var independentSet = _extractor.Extract(new double[] { 4.0, 1.0, 5.0, 4.0 });

            CollectionAssert.AreEqual(new long[] { 0, 2 }, independentSet);
        }

        [TestMethod]
        public void Extract_ShouldReturnCorrectResult_IfInputIsArbitrary_3()
        {
            var independentSet = _extractor.Extract(new double[] { 4.0, 1.0, 4.0, 5.0 });

            CollectionAssert.AreEqual(new long[] { 0, 3 }, independentSet);
        }
    }
}
