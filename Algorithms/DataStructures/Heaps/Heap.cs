using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EdlinSoftware.DataStructures.Heaps
{
    public static class Heap
    {
        public static Heap<TKey, TValue> New<TKey, TValue>()
            where TKey : IComparable<TKey>
        {
            return new Heap<TKey, TValue>(Comparer<TKey>.Default);
        }

        public static Heap<TKey, TValue> New<TKey, TValue>(int initialCapacity)
            where TKey : IComparable<TKey>
        {
            return new Heap<TKey, TValue>(Comparer<TKey>.Default, initialCapacity);
        }

        public static Heap<TKey, TValue> New<TKey, TValue>(HeapElement<TKey, TValue>[] initialHeap)
            where TKey : IComparable<TKey>
        {
            return new Heap<TKey, TValue>(Comparer<TKey>.Default, initialHeap);
        }
    }

    /// <summary>
    /// Represents heap of elements.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public class Heap<TKey, TValue> : IHeap<TKey, TValue>
    {
        private readonly IComparer<TKey> _comparer;
        private HeapElement<TKey, TValue>[] _elements;

        public int Count { get; private set; }

        [DebuggerStepThrough]
        public Heap(IComparer<TKey> comparer, int initialCapacity = 5)
        {
            if (initialCapacity < 1) throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Initial capacity must be positive.");
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            _comparer = comparer;
            _elements = new HeapElement<TKey, TValue>[initialCapacity];
        }

        [DebuggerStepThrough]
        public Heap(IComparer<TKey> comparer, HeapElement<TKey, TValue>[] initialHeap)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            if (initialHeap == null) throw new ArgumentNullException(nameof(initialHeap));
            _comparer = comparer;
            _elements = initialHeap;
            Count = _elements.Length;
            if (Count > 0)
            {
                Heapify();
            }
        }

        private void Heapify()
        {
            for (int i = (Count / 2); i >= 0; i--)
            {
                RestoreHeap(i);
            }
        }

        [DebuggerStepThrough]
        public HeapElement<TKey, TValue> GetMinElement()
        {
            if (Count == 0)
                ThrowHeapEmptyException();

            return _elements[0];
        }

        private static void ThrowHeapEmptyException()
        {
            throw new InvalidOperationException("Heap is empty");
        }

        public void Add(TKey key, TValue value)
        {
            UpdateStorageArray();

            _elements[Count++] = new HeapElement<TKey, TValue>(key, value);
            UpdateHeapAfterInsert();
        }

        private void UpdateStorageArray()
        {
            if (_elements.Length > Count)
                return;

            var newElements = new HeapElement<TKey, TValue>[GetNewCapacity()];
            _elements.CopyTo(newElements, 0);
            _elements = newElements;
        }

        private int GetNewCapacity()
        {
            var newCapacity = Count + Count / 2;
            if (newCapacity == Count) newCapacity++;
            return newCapacity;
        }

        private void UpdateHeapAfterInsert()
        {
            var currentIndex = Count - 1;

            while (true)
            {
                if (currentIndex == 0)
                    return;

                var parentIndex = (currentIndex - 1) / 2;

                if (CorrectOrder(parentIndex, currentIndex))
                    return;

                SwapElements(parentIndex, currentIndex);

                currentIndex = parentIndex;
            }
        }

        private bool CorrectOrder(int parentIndex, int childIndex)
        {
            return _comparer.Compare(_elements[parentIndex].Key, _elements[childIndex].Key) <= 0;
        }

        private void SwapElements(int firstIndex, int secondIndex)
        {
            var tempElement = _elements[firstIndex];
            _elements[firstIndex] = _elements[secondIndex];
            _elements[secondIndex] = tempElement;
        }

        public HeapElement<TKey, TValue> ExtractMinElement()
        {
            if (Count == 0)
                ThrowHeapEmptyException();

            var minElement = _elements[0];

            _elements[0] = _elements[Count - 1];
            Count--;

            RestoreHeap(0);

            return minElement;
        }

        private void RestoreHeap(int index)
        {
            if (index >= Count)
                return;

            var leftChildIndex = 2 * index + 1;
            var rightChildIndex = 2 * index + 2;
            var largestElementIndex = index;

            if (leftChildIndex < Count && _comparer.Compare(_elements[leftChildIndex].Key, _elements[largestElementIndex].Key) < 0)
                largestElementIndex = leftChildIndex;
            if (rightChildIndex < Count && _comparer.Compare(_elements[rightChildIndex].Key, _elements[largestElementIndex].Key) < 0)
                largestElementIndex = rightChildIndex;

            if (largestElementIndex != index)
            {
                SwapElements(index, largestElementIndex);
                RestoreHeap(largestElementIndex);
            }
        }

        public bool Remove(TValue value)
        {
            for (int i = 0; i < Count; i++)
            {
                var elementValue = _elements[i].Value;

                if (ValuesEqual(value, elementValue))
                {
                    RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private bool ValuesEqual(TValue value1, TValue value2)
        {
            if (ReferenceEquals(value1, null) && ReferenceEquals(value2, null)) return true;
            if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null)) return false;
            return value1.Equals(value2);
        }

        private void RemoveAt(int index)
        {
            if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException("index");

            _elements[index] = _elements[Count - 1];
            Count--;

            RestoreHeap(index);
        }
    }
}
