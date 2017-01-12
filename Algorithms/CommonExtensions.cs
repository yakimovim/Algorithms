using System;
using System.Collections.Generic;

namespace EdlinSoftware
{
    /// <summary>
    /// Contains some extension methods for dictionary.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns value from <paramref name="dictionary"/> for given <paramref name="key"/> if such value exists in the <paramref name="dictionary"/>.
        /// Otherwise returns <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="TKey">Type of keys of the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">Type of values of the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">Dictionary.</param>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key,
            TValue defaultValue)
        {
            TValue result;
            if (dictionary.TryGetValue(key, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns value from <paramref name="dictionary"/> for given <paramref name="key"/> if such value exists in the <paramref name="dictionary"/>.
        /// Otherwise returns <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="TKey">Type of keys of the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">Type of values of the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">Dictionary.</param>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            TValue defaultValue)
        {
            TValue result;
            if (dictionary.TryGetValue(key, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Adds value to the dictionary of lists.
        /// </summary>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="dictionary">Dictionary of lists.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value to add.</param>
        public static void AddToDictionary<TKey, TValue>(
            this IDictionary<TKey, List<TValue>> dictionary,
            TKey key,
            TValue value)
        {
            List<TValue> values;
            if (!dictionary.TryGetValue(key, out values))
            {
                values = new List<TValue>();
                dictionary[key] = values;
            }

            values.Add(value);
        }
    }

    /// <summary>
    /// Contains some extension methods for collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Returns element of collection with mininum key.
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <typeparam name="TKey">Type of key</typeparam>
        /// <param name="collection">Collection of elements.</param>
        /// <param name="keySelector">Selector of key from element.</param>
        /// <param name="comparer">Comparer of keys.</param>
        public static T MinBy<T, TKey>(
            this IEnumerable<T> collection,
            Func<T, TKey> keySelector,
            IComparer<TKey> comparer)
            where T : class
        {
            T bestElement = null;
            TKey bestKey = default(TKey);

            foreach (var element in collection)
            {
                var elementKey = keySelector(element);

                if (bestElement == null || comparer.Compare(elementKey, bestKey) < 0)
                {
                    bestElement = element;
                    bestKey = elementKey;
                }
            }

            return bestElement;
        }
    }
}