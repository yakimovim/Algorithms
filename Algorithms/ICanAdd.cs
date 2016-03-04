using System.Collections.Generic;

namespace EdlinSoftware
{
    /// <summary>
    /// Represents all classes that have 'Add' method.
    /// </summary>
    /// <typeparam name="TValue">Type of 'Add' argument.</typeparam>
    public interface ICanAdd<in TValue>
    {
        void Add(TValue value);
    }

    public static class CanAddExtensions
    {
        /// <summary>
        /// Adds several values into the binary search tree.
        /// </summary>
        /// <typeparam name="TValue">Type of values</typeparam>
        /// <param name="tree">Binary search tree.</param>
        /// <param name="values">Values to add.</param>
        public static void AddRange<TValue>(this ICanAdd<TValue> tree, params TValue[] values)
        {
            AddRange(tree, (IEnumerable<TValue>)values);
        }

        /// <summary>
        /// Adds several values into the binary search tree.
        /// </summary>
        /// <typeparam name="TValue">Type of values</typeparam>
        /// <param name="tree">Binary search tree.</param>
        /// <param name="values">Values to add.</param>
        public static void AddRange<TValue>(this ICanAdd<TValue> tree, IEnumerable<TValue> values)
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    tree.Add(value);
                }
            }
        }
    }
}