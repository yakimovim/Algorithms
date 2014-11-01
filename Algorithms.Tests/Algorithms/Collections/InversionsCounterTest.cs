using System;
using EdlinSoftware.Algorithms.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections
{
    [TestClass]
    public class InversionsCounterTest
    {
        private InversionsCounter _inversionsCounter;

        [TestInitialize]
        public void TestInitialize()
        {
            _inversionsCounter = new InversionsCounter();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Count_ShouldThrowException_IfArrayIsNull()
        {
            _inversionsCounter.Count<int>(null);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Count_ShouldReturnZero_IfArrayIsEmpty()
        {
            var count = _inversionsCounter.Count(new int[0]);

            Assert.AreEqual(0, count);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Count_ShouldReturnZero_IfArrayHasOneElement()
        {
            var count = _inversionsCounter.Count(new[] { 1 });

            Assert.AreEqual(0, count);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Count_ShouldReturnZero_IfArrayIsSortedAscending()
        {
            var count = _inversionsCounter.Count(new[] { 1, 2, 3, 4, 5, 6 });

            Assert.AreEqual(0, count);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Count_ShouldReturnCorrectNumber()
        {
            var count = _inversionsCounter.Count(new[] { 1, 3, 5, 2, 4, 6 });

            Assert.AreEqual(3, count);
        }
    }
}
