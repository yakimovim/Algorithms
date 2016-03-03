using System.Diagnostics;

namespace EdlinSoftware.DataStructures.Heaps
{
    /// <summary>
    /// Represents element of a heap.
    /// </summary>
    /// <typeparam name="TKey">Type of element key.</typeparam>
    /// <typeparam name="TValue">Type of element value.</typeparam>
    [DebuggerDisplay("{Key}: {Value}")]
    public sealed class HeapElement<TKey, TValue>
    {
        [DebuggerStepThrough]
        public HeapElement(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets element key.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// Gets element value.
        /// </summary>
        public TValue Value { get; }
    }
}
