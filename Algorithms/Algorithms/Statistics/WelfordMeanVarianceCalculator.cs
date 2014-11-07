using System.Diagnostics;

namespace EdlinSoftware.Algorithms.Statistics
{
    /// <summary>
    /// Represents calculator of running mean and variance using Welford formulas.
    /// </summary>
    public class WelfordMeanVarianceCalculator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long _count;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _mean;
        private double _s;

        public long Count
        {
            [DebuggerStepThrough]
            get { return _count; }
        }

        public double Mean
        {
            [DebuggerStepThrough]
            get { return _mean; }
        }

        public double Variance
        {
            [DebuggerStepThrough]
            get { return _count > 1 ? _s / (_count - 1) : 0.0; }
        }

        public void Add(double nextValue)
        {
            var previousMean = _mean;

            _count++;
            _mean = previousMean + (nextValue - previousMean) / _count;
            _s = _s + (nextValue - previousMean) * (nextValue - _mean);
        }
    }
}