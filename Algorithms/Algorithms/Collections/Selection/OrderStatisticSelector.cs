using System;
using System.Collections.Generic;
using System.Diagnostics;
using EdlinSoftware.Algorithms.Collections.Sorting;

namespace EdlinSoftware.Algorithms.Collections.Selection
{
    public static class OrderStatisticSelector
    {
        public static OrderStatisticSelector<T> New<T>(Func<T[], int, int, int> pivotSelector = null)
            where T : IComparable<T>
        {
            return new OrderStatisticSelector<T>(Comparer<T>.Default, pivotSelector ?? ((array, left, rigth) => left));
        }
    }

    public class OrderStatisticSelector<T> : IOrderStatisticSelector<T>
    {
        private readonly Partitioner<T> _partitioner;
        protected Func<T[], int, int, int> PivotSelector;

        [DebuggerStepThrough]
        public OrderStatisticSelector(IComparer<T> comparer, Func<T[], int, int, int> pivotSelector)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            if (pivotSelector == null) throw new ArgumentNullException(nameof(pivotSelector));
            PivotSelector = pivotSelector;

            _partitioner = new Partitioner<T>(comparer);
        }

        public T Select(T[] array, int order)
        {
            if (array == null || array.Length == 0) throw new ArgumentNullException(nameof(array));
            if (order < 0 || order >= array.Length) throw new ArgumentOutOfRangeException(nameof(order));

            return InternalSelect(array, 0, array.Length - 1, order);
        }

        protected T InternalSelect(T[] array, int left, int right, int order)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (left < 0 || left >= array.Length) throw new ArgumentOutOfRangeException(nameof(left), "Index is out of array");
            if (right < 0 || right >= array.Length) throw new ArgumentOutOfRangeException(nameof(right), "Index is out of array");
            if (left > right) throw new ArgumentOutOfRangeException(nameof(right), "Left index should be smaller then right");
            if (order < 0 || order > right - left) throw new ArgumentOutOfRangeException(nameof(order));

            if (left == right)
            {
                if (order == 0)
                { return array[left]; }
                throw new InvalidProgramException("Array of length 1 can have only 0-order statistic.");
            }

            var pivotIndex = PivotSelector(array, left, right);

            var newPivotIndex = _partitioner.Partition(array, left, right, pivotIndex);

            var newOrder = newPivotIndex - left;

            if (newOrder == order) return array[newPivotIndex];

            if (newOrder > order) return InternalSelect(array, left, newPivotIndex - 1, order);

            return InternalSelect(array, newPivotIndex + 1, right, order - newOrder - 1);
        }
    }
}
