using System;
using System.Collections.Generic;

namespace EdlinSoftware.Algorithms.Collections.Selection
{
    public static class RandomizedOrderStatisticSelector
    {
        public static RandomizedOrderStatisticSelector<T> New<T>()
            where T : IComparable<T>
        {
            return new RandomizedOrderStatisticSelector<T>(Comparer<T>.Default);
        }
    }

    public class RandomizedOrderStatisticSelector<T> : OrderStatisticSelector<T>
    {
        private readonly Random _rnd = new Random((int)DateTime.UtcNow.Ticks);

        public RandomizedOrderStatisticSelector(IComparer<T> comparer)
            : base(comparer, (array, left, rigth) => left)
        {
            PivotSelector = GetRandomSelector;
        }

        private int GetRandomSelector(T[] array, int leftBound, int rightBound)
        {
            return 1 + _rnd.Next(leftBound - 1, rightBound);
        }
    }

}
