using System;
using System.Linq;
using EdlinSoftware.DataStructures.Heaps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.DataStructures.Heaps
{
    [TestClass]
    public abstract class BaseHeapTest<T>
        where T : IHeap<int, string>
    {
        protected T Heap;

        [TestMethod, Owner("Ivan Yakimov")]
        public void JustConstructedHeapMustBeEmpty()
        {
            Assert.AreEqual(0, Heap.Count);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Add_ShouldAddElementIntoEmptyHeap()
        {
            Heap.Add(3, "A");

            var minElement = Heap.GetMinElement();

            Assert.AreEqual(1, Heap.Count);
            Assert.AreEqual("A", minElement.Value);
            Assert.AreEqual(3, minElement.Key);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Add_ShouldAddElementIntoHeap_IfItIsMinimum()
        {
            Heap.Add(3, "A");
            Heap.Add(1, "B");

            var minElement = Heap.GetMinElement();

            Assert.AreEqual(2, Heap.Count);
            Assert.AreEqual("B", minElement.Value);
            Assert.AreEqual(1, minElement.Key);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Add_ShouldAddElementIntoHeap_IfItIsNotMinimum()
        {
            Heap.Add(3, "A");
            Heap.Add(5, "B");

            var minElement = Heap.GetMinElement();

            Assert.AreEqual(2, Heap.Count);
            Assert.AreEqual("A", minElement.Value);
            Assert.AreEqual(3, minElement.Key);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Add_CanInsertAnyNumberOfElements()
        {
            Heap.Add(3, "A");
            Heap.Add(5, "B");
            Heap.Add(4, "C");
            Heap.Add(1, "D");
            Heap.Add(6, "E");
            Heap.Add(2, "F");

            var minElement = Heap.GetMinElement();

            Assert.AreEqual(6, Heap.Count);
            Assert.AreEqual("D", minElement.Value);
            Assert.AreEqual(1, minElement.Key);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExtractMinElement_ShouldThrowException_IfHeapIsEmpty()
        {
            Heap.ExtractMinElement();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ExtractMinElement_ShouldExtractMinElement_IfHeapHasOneElement()
        {
            Heap.Add(3, "A");

            var minElement = Heap.ExtractMinElement();

            Assert.AreEqual(0, Heap.Count);
            Assert.AreEqual("A", minElement.Value);
            Assert.AreEqual(3, minElement.Key);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ExtractMinElement_ShouldExtractMinElement_IfHeapHasSeveralElements()
        {
            Heap.Add(3, "A");
            Heap.Add(5, "B");
            Heap.Add(4, "C");
            Heap.Add(1, "D");
            Heap.Add(6, "E");
            Heap.Add(2, "F");

            var minElement = Heap.ExtractMinElement();

            Assert.AreEqual(5, Heap.Count);
            Assert.AreEqual("D", minElement.Value);
            Assert.AreEqual(1, minElement.Key);

            minElement = Heap.GetMinElement();

            Assert.AreEqual("F", minElement.Value);
            Assert.AreEqual(2, minElement.Key);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ExtractMinElement_ShouldExtractMinElement_ForAllElements()
        {
            Heap.Add(3, "A");
            Heap.Add(5, "B");
            Heap.Add(4, "C");
            Heap.Add(1, "D");
            Heap.Add(6, "E");
            Heap.Add(2, "F");

            var keys = Enumerable.Range(1, 6).Select(i => Heap.ExtractMinElement().Key).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6 }, keys);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_ShouldRemoveOneElementFromSeveral()
        {
            Heap.Add(3, "A");
            Heap.Add(5, "B");
            Heap.Add(4, "C");
            Heap.Add(1, "D");
            Heap.Add(6, "E");
            Heap.Add(2, "F");

            Assert.IsTrue(Heap.Remove("C"));

            var keys = Enumerable.Range(1, 5).Select(i => Heap.ExtractMinElement().Key).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 5, 6 }, keys);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_ShouldRemoveOneElementFromOne()
        {
            Heap.Add(3, "A");

            Assert.IsTrue(Heap.Remove("A"));

            Assert.AreEqual(0, Heap.Count);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_ShouldBeAbleToRemoveAllElements()
        {
            Heap.Add(3, "A");
            Heap.Add(5, "B");
            Heap.Add(4, "C");
            Heap.Add(1, "D");
            Heap.Add(6, "E");
            Heap.Add(2, "F");

            Assert.IsTrue(Heap.Remove("C"));
            Assert.IsTrue(Heap.Remove("E"));
            Assert.IsTrue(Heap.Remove("A"));
            Assert.IsTrue(Heap.Remove("F"));
            Assert.IsTrue(Heap.Remove("D"));
            Assert.IsTrue(Heap.Remove("B"));

            Assert.AreEqual(0, Heap.Count);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Remove_ShouldPreserveCorrectOrderOfElements()
        {
            Heap.Add(3, "A");
            Heap.Add(5, "B");
            Heap.Add(4, "C");
            Heap.Add(1, "D");
            Heap.Add(6, "E");
            Heap.Add(2, "F");

            Assert.IsTrue(Heap.Remove("C"));
            Assert.AreEqual("D", Heap.GetMinElement().Value);
            Assert.IsTrue(Heap.Remove("E"));
            Assert.AreEqual("D", Heap.GetMinElement().Value);
            Assert.IsTrue(Heap.Remove("A"));
            Assert.AreEqual("D", Heap.GetMinElement().Value);
            Assert.IsTrue(Heap.Remove("F"));
            Assert.AreEqual("D", Heap.GetMinElement().Value);
            Assert.IsTrue(Heap.Remove("D"));
            Assert.AreEqual("B", Heap.GetMinElement().Value);
            Assert.IsTrue(Heap.Remove("B"));

            Assert.AreEqual(0, Heap.Count);
        }
    }
}
