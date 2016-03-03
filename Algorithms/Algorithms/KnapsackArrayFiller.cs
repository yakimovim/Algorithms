using System;
using System.Collections.Generic;

namespace EdlinSoftware.Algorithms
{
    /// <summary>
    /// Represents filler of knapsack with maximum value. Uses 2D array to solve the task.
    /// </summary>
    /// <typeparam name="TItem">Type of knapsack item.</typeparam>
    public class KnapsackArrayFiller<TItem> : KnapsackFiller<TItem>
    {
        public KnapsackArrayFiller(Func<TItem, double> itemValueProvider, Func<TItem, long> itemSizeProvider)
            : base(itemValueProvider, itemSizeProvider)
        { }

        public override IEnumerable<TItem> GetItems(long knapsackCapacity, params TItem[] items)
        {
            if (knapsackCapacity <= 0 || items.Length == 0)
            { return new TItem[0]; }

            Initialize(items);

            var optimalValues = GetOptimalValues(knapsackCapacity);

            return GetOptimalItems(knapsackCapacity, optimalValues);
        }

        private double[,] GetOptimalValues(long knapsackCapacity)
        {
            var size = OrderedItems.Length;

            var optimalValues = new double[size + 1, knapsackCapacity + 1];

            for (int restItemsNumber = 0; restItemsNumber <= size; restItemsNumber++)
            {
                for (int restKnapsackCapacity = 0; restKnapsackCapacity <= knapsackCapacity; restKnapsackCapacity++)
                {
                    if (restItemsNumber == 0)
                    {
                        optimalValues[restItemsNumber, restKnapsackCapacity] = 0.0;
                    }
                    else
                    {
                        var itemIndex = restItemsNumber - 1;
                        var nextRestKnapsackCapacity = restKnapsackCapacity - OrderedSizes[restItemsNumber - 1];

                        if (nextRestKnapsackCapacity >= 0)
                        {
                            optimalValues[restItemsNumber, restKnapsackCapacity] = Math.Max(
                                    optimalValues[itemIndex, restKnapsackCapacity],
                                    optimalValues[itemIndex, nextRestKnapsackCapacity] + OrderedValues[itemIndex]
                                );
                        }
                        else
                        {
                            optimalValues[restItemsNumber, restKnapsackCapacity] = optimalValues[itemIndex, restKnapsackCapacity];
                        }
                    }
                }
            }

            return optimalValues;
        }

        private IEnumerable<TItem> GetOptimalItems(long knapsackCapacity, double[,] optimalValues)
        {
            var itemNumber = OrderedItems.Length;
            var knapsackSize = knapsackCapacity;
            var optimalItems = new List<TItem>();

            while (true)
            {
                if (itemNumber == 0 || knapsackSize <= 0)
                { break; }

                var optimalValue = optimalValues[itemNumber, knapsackCapacity];

                if (optimalValue != optimalValues[itemNumber - 1, knapsackCapacity])
                {
                    optimalItems.Add(OrderedItems[itemNumber - 1]);
                    knapsackSize -= OrderedSizes[itemNumber - 1];
                }

                itemNumber--;
            }

            return optimalItems.ToArray();
        }
    }
}
