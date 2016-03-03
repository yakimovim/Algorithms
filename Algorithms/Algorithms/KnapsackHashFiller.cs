using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EdlinSoftware.Algorithms
{
    /// <summary>
    /// Represents filler of knapsack with maximum value. Uses hashset to solve the task.
    /// </summary>
    /// <typeparam name="TItem">Type of knapsack item.</typeparam>
    public class KnapsackHashFiller<TItem> : KnapsackFiller<TItem>
    {
        [DebuggerDisplay("Items {RestItemsNumber} : Capacity {RestKnapsackCapacity}")]
        private class TaskParameters
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly Func<long, long, int> _hashProvider;

            public long RestItemsNumber { get; set; }
            public long RestKnapsackCapacity { get; set; }

            [DebuggerStepThrough]
            public TaskParameters(long restItemsNumber, long restKnapsackCapacity, Func<long, long, int> hashProvider)
            {
                if (hashProvider == null) throw new ArgumentNullException(nameof(hashProvider));

                _hashProvider = hashProvider;
                RestItemsNumber = restItemsNumber;
                RestKnapsackCapacity = restKnapsackCapacity;
            }

            public override bool Equals(object obj)
            {
                var other = obj as TaskParameters;
                if (other == null)
                    return false;

                return RestItemsNumber == other.RestItemsNumber && RestKnapsackCapacity == other.RestKnapsackCapacity;
            }

            public override int GetHashCode()
            {
                return _hashProvider(RestItemsNumber, RestKnapsackCapacity);
            }
        }

        private readonly Func<long, long, int> _hashProvider;
        private Dictionary<TaskParameters, double> _subTaskResults;

        public KnapsackHashFiller(Func<TItem, double> itemValueProvider, Func<TItem, long> itemSizeProvider, Func<long, long, int> hashProvider)
            : base(itemValueProvider, itemSizeProvider)
        {
            if (hashProvider == null) throw new ArgumentNullException(nameof(hashProvider));
            _hashProvider = hashProvider;
        }

        public override IEnumerable<TItem> GetItems(long knapsackCapacity, params TItem[] items)
        {
            if (knapsackCapacity <= 0 || items.Length == 0)
            { return new TItem[0]; }

            Initialize(items);

            GetOptimalValue(items.Length, knapsackCapacity);

            return GetOptimalItems(knapsackCapacity);
        }

        protected override void Initialize(TItem[] items)
        {
            _subTaskResults = new Dictionary<TaskParameters, double>();

            base.Initialize(items);
        }

        private double GetOptimalValue(long restItemsNumber, long restKnapsackCapacity)
        {
            if (restItemsNumber <= 0 || restKnapsackCapacity == 0)
            { return 0; }

            var taskParameters = new TaskParameters(restItemsNumber, restKnapsackCapacity, _hashProvider);
            var stackOfUnknownResults = new Stack<TaskParameters>();
            stackOfUnknownResults.Push(taskParameters);

            while (stackOfUnknownResults.Count > 0)
            {
                var currentTaskParameters = stackOfUnknownResults.Peek();

                if (currentTaskParameters.RestItemsNumber <= 0
                    || currentTaskParameters.RestKnapsackCapacity == 0)
                {
                    _subTaskResults[currentTaskParameters] = 0;
                    stackOfUnknownResults.Pop();
                    continue;
                }

                var itemsNumberWithoutLast = currentTaskParameters.RestItemsNumber - 1;

                var lastItemSize = OrderedSizes[itemsNumberWithoutLast];
                var lastItemValue = OrderedValues[itemsNumberWithoutLast];

                var capacityWithoutLastItemSize = currentTaskParameters.RestKnapsackCapacity - lastItemSize;

                var subTask1Parameters = new TaskParameters(itemsNumberWithoutLast, currentTaskParameters.RestKnapsackCapacity, _hashProvider);
                var subTask2Parameters = new TaskParameters(itemsNumberWithoutLast, capacityWithoutLastItemSize, _hashProvider);

                double subTask1Result;
                double subTask2Result;

                var hasSubTask1Result = _subTaskResults.TryGetValue(subTask1Parameters, out subTask1Result);
                var hasSubTask2Result = _subTaskResults.TryGetValue(subTask2Parameters, out subTask2Result);

                if (capacityWithoutLastItemSize >= 0)
                {
                    if (hasSubTask1Result && hasSubTask2Result)
                    {
                        _subTaskResults[currentTaskParameters] = Math.Max(subTask1Result, subTask2Result + lastItemValue);
                        stackOfUnknownResults.Pop();
                        continue;
                    }
                    if (!hasSubTask1Result)
                    {
                        stackOfUnknownResults.Push(subTask1Parameters);
                    }
                    if (!hasSubTask2Result)
                    {
                        stackOfUnknownResults.Push(subTask2Parameters);
                    }
                }
                else
                {
                    if (hasSubTask1Result)
                    {
                        _subTaskResults[currentTaskParameters] = subTask1Result;
                        stackOfUnknownResults.Pop();
                        continue;
                    }
                    else
                    {
                        stackOfUnknownResults.Push(subTask1Parameters);
                    }
                }
            }

            taskParameters = new TaskParameters(restItemsNumber, restKnapsackCapacity, _hashProvider);
            return _subTaskResults[taskParameters];
        }

        private IEnumerable<TItem> GetOptimalItems(long knapsackCapacity)
        {
            var taskParameters = new TaskParameters(OrderedItems.Length, knapsackCapacity, _hashProvider);

            var optimalItems = new List<TItem>();

            while (true)
            {
                if (taskParameters.RestItemsNumber == 0 || taskParameters.RestKnapsackCapacity <= 0)
                { break; }

                var optimalValue = _subTaskResults[taskParameters];

                taskParameters.RestItemsNumber--;

                double previousOptimalValue;
                if (_subTaskResults.TryGetValue(taskParameters, out previousOptimalValue) == false)
                { previousOptimalValue = 0; }

                if (optimalValue != previousOptimalValue)
                {
                    optimalItems.Add(OrderedItems[taskParameters.RestItemsNumber]);
                    taskParameters.RestKnapsackCapacity -= OrderedSizes[taskParameters.RestItemsNumber];
                }
            }

            return optimalItems.ToArray();
        }
    }
}
