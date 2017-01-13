namespace EdlinSoftware.DataStructures
{
    /// <summary>
    /// Represents item with value.
    /// </summary>
    /// <typeparam name="TValue">Type of value stored in the node.</typeparam>
    public interface IValued<out TValue>
    {
        /// <summary>
        /// Gets value of the item.
        /// </summary>
        TValue Value { get; }
    }
}
