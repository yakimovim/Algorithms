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
        protected T _heap;

        [TestMethod]
        public void JustConstructedHeapMustBeEmpty()
        {
            Assert.AreEqual(0, _heap.Count);
        }

        [TestMethod]
        public void Add_ShouldAddElementIntoEmptyHeap()
        {
            _heap.Add(3, "A");

            var minElement = _heap.GetMinElement();

            Assert.AreEqual(1, _heap.Count);
            Assert.AreEqual("A", minElement.Value);
            Assert.AreEqual(3, minElement.Key);
        }

        [TestMethod]
        public void Add_ShouldAddElementIntoHeap_IfItIsMinimum()
        {
            _heap.Add(3, "A");
            _heap.Add(1, "B");

            var minElement = _heap.GetMinElement();

            Assert.AreEqual(2, _heap.Count);
            Assert.AreEqual("B", minElement.Value);
            Assert.AreEqual(1, minElement.Key);
        }

        [TestMethod]
        public void Add_ShouldAddElementIntoHeap_IfItIsNotMinimum()
        {
            _heap.Add(3, "A");
            _heap.Add(5, "B");

            var minElement = _heap.GetMinElement();

            Assert.AreEqual(2, _heap.Count);
            Assert.AreEqual("A", minElement.Value);
            Assert.AreEqual(3, minElement.Key);
        }

        [TestMethod]
        public void Add_CanInsertAnyNumberOfElements()
        {
            _heap.Add(3, "A");
            _heap.Add(5, "B");
            _heap.Add(4, "C");
            _heap.Add(1, "D");
            _heap.Add(6, "E");
            _heap.Add(2, "F");

            var minElement = _heap.GetMinElement();

            Assert.AreEqual(6, _heap.Count);
            Assert.AreEqual("D", minElement.Value);
            Assert.AreEqual(1, minElement.Key);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExtractMinElement_ShouldThrowException_IfHeapIsEmpty()
        {
            _heap.ExtractMinElement();
        }

        [TestMethod]
        public void ExtractMinElement_ShouldExtractMinElement_IfHeapHasOneElement()
        {
            _heap.Add(3, "A");

            var minElement = _heap.ExtractMinElement();

            Assert.AreEqual(0, _heap.Count);
            Assert.AreEqual("A", minElement.Value);
            Assert.AreEqual(3, minElement.Key);
        }

        [TestMethod]
        public void ExtractMinElement_ShouldExtractMinElement_IfHeapHasSeveralElements()
        {
            _heap.Add(3, "A");
            _heap.Add(5, "B");
            _heap.Add(4, "C");
            _heap.Add(1, "D");
            _heap.Add(6, "E");
            _heap.Add(2, "F");

            var minElement = _heap.ExtractMinElement();

            Assert.AreEqual(5, _heap.Count);
            Assert.AreEqual("D", minElement.Value);
            Assert.AreEqual(1, minElement.Key);

            minElement = _heap.GetMinElement();

            Assert.AreEqual("F", minElement.Value);
            Assert.AreEqual(2, minElement.Key);
        }

        [TestMethod]
        public void ExtractMinElement_ShouldExtractMinElement_ForAllElements()
        {
            _heap.Add(3, "A");
            _heap.Add(5, "B");
            _heap.Add(4, "C");
            _heap.Add(1, "D");
            _heap.Add(6, "E");
            _heap.Add(2, "F");

            var keys = Enumerable.Range(1, 6).Select(i => _heap.ExtractMinElement().Key).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6 }, keys);
        }

        [TestMethod]
        public void Remove_ShouldRemoveOneElementFromSeveral()
        {
            _heap.Add(3, "A");
            _heap.Add(5, "B");
            _heap.Add(4, "C");
            _heap.Add(1, "D");
            _heap.Add(6, "E");
            _heap.Add(2, "F");

            Assert.IsTrue(_heap.Remove("C"));

            var keys = Enumerable.Range(1, 5).Select(i => _heap.ExtractMinElement().Key).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 5, 6 }, keys);
        }

        [TestMethod]
        public void Remove_ShouldRemoveOneElementFromOne()
        {
            _heap.Add(3, "A");

            Assert.IsTrue(_heap.Remove("A"));

            Assert.AreEqual(0, _heap.Count);
        }

        [TestMethod]
        public void Remove_ShouldBeAbleToRemoveAllElements()
        {
            _heap.Add(3, "A");
            _heap.Add(5, "B");
            _heap.Add(4, "C");
            _heap.Add(1, "D");
            _heap.Add(6, "E");
            _heap.Add(2, "F");

            Assert.IsTrue(_heap.Remove("C"));
            Assert.IsTrue(_heap.Remove("E"));
            Assert.IsTrue(_heap.Remove("A"));
            Assert.IsTrue(_heap.Remove("F"));
            Assert.IsTrue(_heap.Remove("D"));
            Assert.IsTrue(_heap.Remove("B"));

            Assert.AreEqual(0, _heap.Count);
        }

        [TestMethod]
        public void Remove_ShouldPreserveCorrectOrderOfElements()
        {
            _heap.Add(3, "A");
            _heap.Add(5, "B");
            _heap.Add(4, "C");
            _heap.Add(1, "D");
            _heap.Add(6, "E");
            _heap.Add(2, "F");

            Assert.IsTrue(_heap.Remove("C"));
            Assert.AreEqual("D", _heap.GetMinElement().Value);
            Assert.IsTrue(_heap.Remove("E"));
            Assert.AreEqual("D", _heap.GetMinElement().Value);
            Assert.IsTrue(_heap.Remove("A"));
            Assert.AreEqual("D", _heap.GetMinElement().Value);
            Assert.IsTrue(_heap.Remove("F"));
            Assert.AreEqual("D", _heap.GetMinElement().Value);
            Assert.IsTrue(_heap.Remove("D"));
            Assert.AreEqual("B", _heap.GetMinElement().Value);
            Assert.IsTrue(_heap.Remove("B"));

            Assert.AreEqual(0, _heap.Count);
        }
    }
}
