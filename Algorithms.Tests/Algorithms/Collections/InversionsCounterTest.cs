using System;
using EdlinSoftware.Algorithms.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections
{
    [TestClass]
    public class InversionsCounterTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Count_ShouldThrowException_IfArrayIsNull()
        {
            InversionsCounter.Count<int>(null);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Count_ShouldReturnZero_IfArrayIsEmpty()
        {
            var count = InversionsCounter.Count(new int[0]);

            Assert.AreEqual(0, count);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Count_ShouldReturnZero_IfArrayHasOneElement()
        {
            var count = InversionsCounter.Count(new[] { 1 });

            Assert.AreEqual(0, count);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Count_ShouldReturnZero_IfArrayIsSortedAscending()
        {
            var count = InversionsCounter.Count(new[] { 1, 2, 3, 4, 5, 6 });

            Assert.AreEqual(0, count);
        }

        [TestMethod, Owner(@"FIRM\Ivan")]
        public void Count_ShouldReturnCorrectNumber()
        {
            var count = InversionsCounter.Count(new[] { 1, 3, 5, 2, 4, 6 });

            Assert.AreEqual(3, count);
        }
    }
}
