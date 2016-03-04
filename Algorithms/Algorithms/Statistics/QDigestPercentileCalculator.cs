using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EdlinSoftware.Algorithms.Statistics
{
    /// <summary>
    /// Represents calculator of percentiles using QDigest algorithm.
    /// </summary>
    public class QDigestPercentileCalculator : ICanAdd<long>
    {
        private class Range
        {
            public Range(long index, long count, long sigma)
            {
                Lower = GetLower(index, sigma);
                Upper = GetUpper(index, sigma);
                Count = count;
            }

            private long GetLower(long index, long sigma)
            {
                while (index < sigma)
                { index = index * 2; }
                return index - sigma;
            }

            private long GetUpper(long index, long sigma)
            {
                while (index < sigma)
                { index = index * 2 + 1; }
                return index - sigma;
            }

            public long Lower { get; }
            public long Upper { get; }
            public long Count { get; }
        }

        private class RangeComparer : IComparer<Range>
        {
            public int Compare(Range x, Range y)
            {
                if (x.Upper < y.Upper)
                { return -1; }
                if (x.Upper > y.Upper)
                { return 1; }
                if (x.Lower > y.Lower)
                { return -1; }
                if (x.Lower < y.Lower)
                { return 1; }

                return 0;
            }
        }

        private readonly IDictionary<long, long> _nodes = new Dictionary<long, long>();
        private readonly long _sigma;
        private readonly double _compressionFactor;

        private long _max;
        private long _count;

        public QDigestPercentileCalculator(uint maxPowerOfTwo, double compressionFactor)
        {
            _sigma = 1 << ((int)maxPowerOfTwo);
            _compressionFactor = compressionFactor;
        }

        public void Add(long value)
        {
            value = Math.Max(0L, Math.Min(value, _sigma - 1));

            var index = _sigma + value;

            _count++;
            _max = Math.Max(_max, value);

            _nodes[index] = _nodes.GetOrDefault(index) + 1;

            Compress(index);
        }

        private void Compress(long index)
        {
            var breakOver = _count / _compressionFactor;

            while (index > 1)
            {
                var siblingIndex = (index % 2 == 0) ? index + 1 : index - 1;
                var parentIndex = (index % 2 == 0) ? index / 2 : (index - 1) / 2;

                var count = _nodes.GetOrDefault(index);
                var siblingCount = _nodes.GetOrDefault(siblingIndex);
                var parentCount = _nodes.GetOrDefault(parentIndex);

                var sum = count + siblingCount + parentCount;

                if (sum >= breakOver)
                { break; }

                _nodes[parentIndex] = sum;
                _nodes.Remove(index);
                _nodes.Remove(siblingIndex);

                index = parentIndex;
            }
        }

        public long GetPercentile(double q)
        {
            var ranges = _nodes
                .Select(n => new Range(n.Key, n.Value, _sigma))
                .OrderBy(r => r, new RangeComparer())
                .ToArray();

            long count = 0L;
            for (int i = 0; i < ranges.Length; i++)
            {
                count += ranges[i].Count;
                if (count > q * _count)
                { return ranges[i].Upper; }
            }

            return ranges[ranges.Length - 1].Upper;
        }
    }

    internal static class DictionaryExtender
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue result;
            if (!dictionary.TryGetValue(key, out result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}