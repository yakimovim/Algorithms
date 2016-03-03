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
        protected Func<TItem, double> ItemValueProvider;
        protected Func<TItem, long> ItemSizeProvider;

        protected TItem[] OrderedItems;
        protected double[] OrderedValues;
        protected long[] OrderedSizes;

        protected KnapsackFiller(Func<TItem, double> itemValueProvider, Func<TItem, long> itemSizeProvider)
        {
            if (itemValueProvider == null) throw new ArgumentNullException(nameof(itemValueProvider));
            if (itemSizeProvider == null) throw new ArgumentNullException(nameof(itemSizeProvider));
            ItemValueProvider = itemValueProvider;
            ItemSizeProvider = itemSizeProvider;
        }

        public abstract IEnumerable<TItem> GetItems(long knapsackCapacity, params TItem[] items);

        protected virtual void Initialize(TItem[] items)
        {
            OrderedItems = new TItem[items.Length];
            OrderedSizes = new long[items.Length];
            OrderedValues = new double[items.Length];

            var orderedItems = items
                .Select(i => new { Item = i, Value = ItemValueProvider(i), Size = ItemSizeProvider(i) })
                .OrderBy(i => i.Size)
                .ToArray();

            for (int i = 0; i < items.Length; i++)
            {
                OrderedItems[i] = orderedItems[i].Item;
                OrderedSizes[i] = orderedItems[i].Size;
                OrderedValues[i] = orderedItems[i].Value;
            }
        }
    }
}
