using System;
using EdlinSoftware.Algorithms.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections
{
    [TestClass]
    public class InversionsCounterTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Count_ShouldThrowException_IfArrayIsNull()
        {
            InversionsCounter.Count<int>(null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Count_ShouldReturnZero_IfArrayIsEmpty()
        {
            var count = InversionsCounter.Count(new int[0]);

            Assert.AreEqual(0, count);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Count_ShouldReturnZero_IfArrayHasOneElement()
        {
            var count = InversionsCounter.Count(new[] { 1 });

            Assert.AreEqual(0, count);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Count_ShouldReturnZero_IfArrayIsSortedAscending()
        {
            var count = InversionsCounter.Count(new[] { 1, 2, 3, 4, 5, 6 });

            Assert.AreEqual(0, count);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Count_ShouldReturnCorrectNumber()
        {
            var count = InversionsCounter.Count(new[] { 1, 3, 5, 2, 4, 6 });

            Assert.AreEqual(3, count);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Count_ShouldReturnCorrectNumber_IfThereAreEqualNumbers()
        {
            var count = InversionsCounter.Count(new[] { 2, 3, 9, 2, 9 });

            Assert.AreEqual(2, count);
        }
    }
}
