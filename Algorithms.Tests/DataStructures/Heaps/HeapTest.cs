using System;
using System.Linq;
using EdlinSoftware.DataStructures.Heaps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.DataStructures.Heaps
{
    [TestClass]
    public class HeapTest : BaseHeapTest<Heap<int, string>>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _heap = Heap.New<int, string>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void New_ShouldThrowException_IfInitialCapacityIsLessThenOne()
        {
            Heap.New<int, string>(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void New_ShouldThrowException_IfInitialHeapIsNull()
        {
            Heap.New<int, string>(null);
        }

        [TestMethod]
        public void New_ShouldBuildCorrectHeap_FromInitialHeap()
        {
            _heap = Heap.New(new[]
                                    {
                                        new HeapElement<int, string>(3, "A"), 
                                        new HeapElement<int, string>(5, "B"), 
                                        new HeapElement<int, string>(4, "C"), 
                                        new HeapElement<int, string>(1, "D"), 
                                        new HeapElement<int, string>(6, "E"), 
                                        new HeapElement<int, string>(2, "F") 
                                    });

            var keys = Enumerable.Range(1, 6).Select(i => _heap.ExtractMinElement().Key).ToArray();

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6 }, keys);
        }

        [TestMethod]
        public void HeapShouldWorkWithMaxKeys()
        {
            _heap = Heap.New(new[]
                                    {
                                        new HeapElement<int, string>(int.MaxValue, "A"), 
                                        new HeapElement<int, string>(int.MaxValue, "B"), 
                                        new HeapElement<int, string>(int.MaxValue, "C"), 
                                        new HeapElement<int, string>(int.MaxValue, "D"), 
                                        new HeapElement<int, string>(int.MaxValue, "E"), 
                                        new HeapElement<int, string>(int.MaxValue, "F") 
                                    });

            var keys = Enumerable.Range(1, 6).Select(i => _heap.ExtractMinElement().Key).ToArray();

            CollectionAssert.AreEqual(new[] { int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue }, keys);
        }
    }
}
