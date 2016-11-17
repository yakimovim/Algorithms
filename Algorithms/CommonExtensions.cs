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
}