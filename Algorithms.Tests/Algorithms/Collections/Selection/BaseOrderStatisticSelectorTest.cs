using System;
using EdlinSoftware.Algorithms.Collections.Selection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Selection
{
    [TestClass]
    public abstract class BaseOrderStatisticSelectorTest<TSelector>
        where TSelector : IOrderStatisticSelector<int>
    {
        protected TSelector Selector;

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Select_ShouldThrowException_IfArrayIsNull()
        {
            Selector.Select(null, 1);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Select_ShouldThrowException_IfArrayIsEmpty()
        {
            Selector.Select(new int[0], 1);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Select_ShouldThrowException_IfOrderIsLessThen0()
        {
            Selector.Select(new int[10], -1);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Select_ShouldThrowException_IfOrderIsBiggerThenArraySize()
        {
            Selector.Select(new int[10], 10);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Select_ShouldReturnTheOnlyElement_IfArrayHasOneElement()
        {
            var stat = Selector.Select(new[] { 3 }, 0);

            Assert.AreEqual(3, stat);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Select_ShouldReturnMinimumElement_ForFirstStatistic()
        {
            var stat = Selector.Select(new[] { 3, 1, 8, 2, 5, 7, 4, 6 }, 0);

            Assert.AreEqual(1, stat);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Select_ShouldReturnMaximumElement_ForLastStatistic()
        {
            var stat = Selector.Select(new[] { 3, 1, 8, 2, 5, 7, 4, 6 }, 7);

            Assert.AreEqual(8, stat);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Select_ShouldReturnCorrectElement_ForAnyStatistic()
        {
            var stat = Selector.Select(new[] { 3, 1, 8, 2, 5, 7, 4, 6 }, 3);

            Assert.AreEqual(4, stat);
        }
    }
}
