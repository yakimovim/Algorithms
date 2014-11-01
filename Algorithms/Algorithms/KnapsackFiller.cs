using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms
{
    /// <summary>
    /// Represents base class of filler of knapsack with maximum value.
    /// </summary>
    /// <typeparam name="TItem">Type of knapsack item.</typeparam>
    public abstract class KnapsackFiller<TItem>
    {
        protected Func<TItem, double> _itemValueProvider;
        protected Func<TItem, long> _itemSizeProvider;

        protected TItem[] _orderedItems;
        protected double[] _orderedValues;
        protected long[] _orderedSizes;

        protected KnapsackFiller(Func<TItem, double> itemValueProvider, Func<TItem, long> itemSizeProvider)
        {
            if (itemValueProvider == null) throw new ArgumentNullException("itemValueProvider");
            if (itemSizeProvider == null) throw new ArgumentNullException("itemSizeProvider");
            _itemValueProvider = itemValueProvider;
            _itemSizeProvider = itemSizeProvider;
        }

        public abstract IEnumerable<TItem> GetItems(long knapsackCapacity, params TItem[] items);

        protected virtual void Initialize(TItem[] items)
        {
            _orderedItems = new TItem[items.Length];
            _orderedSizes = new long[items.Length];
            _orderedValues = new double[items.Length];

            var orderedItems = items
                .Select(i => new { Item = i, Value = _itemValueProvider(i), Size = _itemSizeProvider(i) })
                .OrderBy(i => i.Size)
                .ToArray();

            for (int i = 0; i < items.Length; i++)
            {
                _orderedItems[i] = orderedItems[i].Item;
                _orderedSizes[i] = orderedItems[i].Size;
                _orderedValues[i] = orderedItems[i].Value;
            }
        }
    }
}
