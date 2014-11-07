using System.Linq;
using EdlinSoftware.Algorithms;
using EdlinSoftware.Tests.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms
{
    [TestClass]
    public abstract class KnapsackFillerTest<TFiller>
        where TFiller : KnapsackFiller<KnapsackItem>
    {
        protected TFiller Filler;

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetItems_ShouldReturnEmptySet_IfThereAreNoItems()
        {
            var items = Filler.GetItems(100).ToArray();

            Assert.AreEqual(0, items.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetItems_ShouldReturnEmptySet_IfKnapsackCapacityIsNegative()
        {
            var items = Filler.GetItems(-100, new KnapsackItem(10, 1)).ToArray();

            Assert.AreEqual(0, items.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetItems_ShouldReturnOneItemFromOne_IfKnapsackCapacityIsEnought()
        {
            var item = new KnapsackItem(10, 1);

            var items = Filler.GetItems(2, item).ToArray();

            CollectionAssert.AreEqual(new[] { item }, items);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetItems_ShouldReturnOneItemFromSeveral_IfKnapsackCapacityIsEnought()
        {
            var item1 = new KnapsackItem(10, 1);
            var item2 = new KnapsackItem(10, 3);
            var item3 = new KnapsackItem(10, 4);

            var items = Filler.GetItems(2, item1, item2, item3).ToArray();

            CollectionAssert.AreEqual(new[] { item1 }, items);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetItems_ShouldReturnCorrectAnswer_FromSeveralCandidates()
        {
            var item1 = new KnapsackItem(5, 10);
            var item2 = new KnapsackItem(3, 3);
            var item3 = new KnapsackItem(3, 4);

            var items = Filler.GetItems(10, item1, item2, item3).ToArray();

            CollectionAssert.AreEquivalent(new[] { item2, item3 }, items);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetItems_ShouldReturnCorrectAnswer_FromSeveralCandidates2()
        {
            var item1 = new KnapsackItem(7, 10);
            var item2 = new KnapsackItem(3, 3);
            var item3 = new KnapsackItem(3, 4);

            var items = Filler.GetItems(10, item1, item2, item3).ToArray();

            CollectionAssert.AreEquivalent(new[] { item1 }, items);
        }
    }
}
