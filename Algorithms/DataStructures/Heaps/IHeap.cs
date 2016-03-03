namespace EdlinSoftware.DataStructures.Heaps
{
    /// <summary>
    /// Represents heap.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public interface IHeap<TKey, TValue>
    {
        /// <summary>
        /// Gets number of elements in the heap.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Extracts from the heap the element with the minimum key.
        /// </summary>
        /// <returns>Element with the minimum key.</returns>
        HeapElement<TKey, TValue> ExtractMinElement();
        /// <summary>
        /// Get from the heap the element with the minimum key without removing it from the heap.
        /// </summary>
        /// <returns>Element with the minimum key.</returns>
        HeapElement<TKey, TValue> GetMinElement();
        /// <summary>
        /// Inserts new element into the heap.
        /// </summary>
        /// <param name="key">Key of the element.</param>
        /// <param name="value">Value of the element.</param>
        void Add(TKey key, TValue value);
        /// <summary>
        /// Removes value from the heap.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        /// <returns>True, if value was removed from the heap, false otherwise.</returns>
        bool Remove(TValue value);
    }
}
