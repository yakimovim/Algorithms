namespace EdlinSoftware.Algorithms.Collections.Selection
{
    /// <summary>
    /// Represents selector of n-th order statistics from unsorted array.
    /// </summary>
    /// <typeparam name="T">Type of statistics</typeparam>
    public interface IOrderStatisticSelector<T>
    {
        /// <summary>
        /// Returns statistics of given order.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="order">Order of statistic.</param>
        T Select(T[] array, int order);
    }
}
