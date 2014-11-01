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
        private readonly Func<TJob, double> _weigthProvider;
        private readonly Func<TJob, double> _lengthProvider;

        public WeightedCompletionTimeMinimizer(Func<TJob, double> weigthProvider, Func<TJob, double> lengthProvider)
        {
            if (weigthProvider == null) throw new ArgumentNullException("weigthProvider");
            if (lengthProvider == null) throw new ArgumentNullException("lengthProvider");

            _weigthProvider = weigthProvider;
            _lengthProvider = lengthProvider;
        }

        /// <summary>
        /// Returns jobs in the order which minimizes weighted sum of completion times of jobs.
        /// </summary>
        /// <param name="jobs">Jobs.</param>
        public IEnumerable<TJob> Schedule(IEnumerable<TJob> jobs)
        {
            if (jobs == null)
            { return null; }

            return jobs.OrderByDescending(j => {
                var length = _lengthProvider(j);
                if (length <= 0.0)
                    throw new ArgumentOutOfRangeException("Length must be positive");
                var weight = _weigthProvider(j);
                return weight / length;
            }).ToArray();
        }
    }
}
