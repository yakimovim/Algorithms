using System;
using System.Linq;
using EdlinSoftware.Algorithms.Scheduling;
using EdlinSoftware.Tests.DataStructures.Scheduling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Scheduling
{
    [TestClass]
    public class WeightedCompletionTimeMinimizerTest
    {
        private WeightedCompletionTimeMinimizer<Job> _scheduler;

        [TestInitialize]
        public void TestInitialize()
        {
            _scheduler = new WeightedCompletionTimeMinimizer<Job>(j => j.Weight, j => j.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Schedule_ShouldReturnNull_IfJobsAreNull()
        {
            Assert.IsNull(_scheduler.Schedule(null));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Schedule_ShouldReturnEmptySequence_IfJobsAreEmpty()
        {
            var scheduledJobs = _scheduler.Schedule(new Job[0]);

            Assert.AreEqual(0, scheduledJobs.Count());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Schedule_ShouldReturnOneJob_IfThereIsOnlyOneJob()
        {
            var scheduledJobs = _scheduler.Schedule(new[] { new Job { Weight = 1, Length = 1 } });

            Assert.AreEqual(1, scheduledJobs.Count());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Schedule_ShouldThrowException_IfThereAreNonPositiveLengths()
        {
            _scheduler.Schedule(new[] { new Job { Weight = 1, Length = -1 } });
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Schedule_ShouldReturnCorrectSchedule_IfThereAreSeveralJobs()
        {
            var scheduledJobs = _scheduler.Schedule(new[] 
            { 
                new Job { Index = 0, Weight = 1, Length = 3 },
                new Job { Index = 1, Weight = 2, Length = 2 },
                new Job { Index = 2, Weight = 3, Length = 1 } 
            });

            CollectionAssert.AreEqual(new[] { 2, 1, 0 }, scheduledJobs.Select(j => j.Index).ToArray());
        }
    }
}
