using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Collections
{
    /// <summary>
    /// Represents extractor of independent set of maximum weight.
    /// </summary>
    public class MaxWeightIndependentSetExtractor
    {
        public long[] Extract(double[] set)
        {
            if (set == null) throw new ArgumentNullException("set");

            var weightsArray = GetWeightsArray(set);

            return ReconstructIndependentSet(weightsArray, set);
        }

        private double[] GetWeightsArray(double[] set)
        {
            var weightsArray = new double[set.LongLength];

            for (long i = 0; i < weightsArray.LongLength; i++)
            {
                weightsArray[i] = Math.Max(
                        GetWeight(i - 1, weightsArray),
                        GetWeight(i - 2, weightsArray) + set[i]
                    );
            }

            return weightsArray;
        }

        private double GetWeight(long index, double[] array)
        {
            if (index < 0) return 0;

            return array[index];
        }

        private long[] ReconstructIndependentSet(double[] weightsArray, double[] set)
        {
            var independentSet = new LinkedList<long>();

            for (long i = weightsArray.LongLength - 1; i >= 0; i--)
            {
                if (GetWeight(i - 1, weightsArray) < GetWeight(i - 2, weightsArray) + set[i])
                {
                    independentSet.AddFirst(i);

                    i--;
                }
            }

            return independentSet.ToArray();
        }
    }
}
