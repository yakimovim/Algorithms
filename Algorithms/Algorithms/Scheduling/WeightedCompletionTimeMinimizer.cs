using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Scheduling
{
    /// <summary>
    /// Represents scheduler which minimizes weighted sum of completion times of jobs.
    /// </summary>
    /// <typeparam name="TJob">Type of job.</typeparam>
    public class WeightedCompletionTimeMinimizer<TJob>
    {
        private readonly Func<TJob, double> _weightProvider;
        private readonly Func<TJob, double> _lengthProvider;

        public WeightedCompletionTimeMinimizer(Func<TJob, double> weightProvider, Func<TJob, double> lengthProvider)
        {
            if (weightProvider == null) throw new ArgumentNullException(nameof(weightProvider));
            if (lengthProvider == null) throw new ArgumentNullException(nameof(lengthProvider));

            _weightProvider = weightProvider;
            _lengthProvider = lengthProvider;
        }

        /// <summary>
        /// Returns jobs in the order which minimizes weighted sum of completion times of jobs.
        /// </summary>
        /// <param name="jobs">Jobs.</param>
        public IEnumerable<TJob> Schedule(IEnumerable<TJob> jobs)
        {
            return jobs?.OrderByDescending(j => {
                                                    var length = _lengthProvider(j);
                                                    if (length <= 0.0)
                                                        throw new ArgumentOutOfRangeException(nameof(length), "Length must be positive");
                                                    var weight = _weightProvider(j);
                                                    return weight / length;
            }).ToArray();
        }
    }
}
