using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EdlinSoftware.Algorithms.Collections.Selection
{
    public static class DeterministicOrderStatisticSelector
    {
        public static DeterministicOrderStatisticSelector<T> New<T>()
            where T : IComparable<T>
        {
            return new DeterministicOrderStatisticSelector<T>(Comparer<T>.Default);
        }
    }

    public class DeterministicOrderStatisticSelector<T> : OrderStatisticSelector<T>
    {
        private readonly IComparer<T> _comparer;

        [DebuggerStepThrough]
        public DeterministicOrderStatisticSelector(IComparer<T> comparer)
            : base(comparer, (array, left, rigth) => left)
        {
            _pivotSelector = SelectPivotIndex;
            _comparer = comparer;
        }

        private int SelectPivotIndex(T[] array, int left, int right)
        {
            var arrayOfMedians = GetArrayOfMedians(array, left, right);

            var middleElement = InternalSelect(arrayOfMedians, 0, arrayOfMedians.Length - 1, arrayOfMedians.Length / 2);

            for (int i = left; i <= right; i++)
            {
                if (array[i].Equals(middleElement))
                {
                    return i;
                }
            }

            throw new InvalidOperationException();
        }

        private T[] GetArrayOfMedians(T[] array, int left, int right)
        {
            var fiveElements = new HashSet<T>();
            var listOfMedians = new List<T>();

            for (int i = left; i <= right; i++)
            {
                fiveElements.Add(array[i]);

                if (fiveElements.Count == 5)
                {
                    listOfMedians.Add(GetMedian(fiveElements));
                    fiveElements.Clear();
                }

            }

            if (fiveElements.Count != 0)
            {
                listOfMedians.Add(GetMedian(fiveElements));
                fiveElements.Clear();
            }

            return listOfMedians.ToArray();
        }

        private T GetMedian(IEnumerable<T> fiveElements)
        {
            var sortedArray = fiveElements.OrderBy(i => i, _comparer).ToArray();

            return sortedArray[sortedArray.Length / 2];
        }
    }
}
