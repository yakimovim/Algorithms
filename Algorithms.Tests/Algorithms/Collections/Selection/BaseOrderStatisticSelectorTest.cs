using System;
using EdlinSoftware.Algorithms.Collections.Selection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Selection
{
    [TestClass]
    public abstract class BaseOrderStatisticSelectorTest<TSelector>
        where TSelector : IOrderStatisticSelector<int>
    {
        protected TSelector _selector;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Select_ShouldThrowException_IfArrayIsNull()
        {
            _selector.Select(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Select_ShouldThrowException_IfArrayIsEmpty()
        {
            _selector.Select(new int[0], 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Select_ShouldThrowException_IfOrderIsLessThen0()
        {
            _selector.Select(new int[10], -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Select_ShouldThrowException_IfOrderIsBiggerThenArraySize()
        {
            _selector.Select(new int[10], 10);
        }

        [TestMethod]
        public void Select_ShouldReturnTheOnlyElement_IfArrayHasOneElement()
        {
            var stat = _selector.Select(new[] { 3 }, 0);

            Assert.AreEqual(3, stat);
        }

        [TestMethod]
        public void Select_ShouldReturnMinimumElement_ForFirstStatistic()
        {
            var stat = _selector.Select(new[] { 3, 1, 8, 2, 5, 7, 4, 6 }, 0);

            Assert.AreEqual(1, stat);
        }

        [TestMethod]
        public void Select_ShouldReturnMaximumElement_ForLastStatistic()
        {
            var stat = _selector.Select(new[] { 3, 1, 8, 2, 5, 7, 4, 6 }, 7);

            Assert.AreEqual(8, stat);
        }

        [TestMethod]
        public void Select_ShouldReturnCorrectElement_ForAnyStatistic()
        {
            var stat = _selector.Select(new[] { 3, 1, 8, 2, 5, 7, 4, 6 }, 3);

            Assert.AreEqual(4, stat);
        }
    }
}
