using System;
using EdlinSoftware.Algorithms.Graphs;
using EdlinSoftware.Tests.DataStructures.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs
{
    [TestClass]
    public class OptimalSearchTreeBuilderTest
    {
        private OptimalSearchTreeBuilder<SearchElement> _builder;

        [TestInitialize]
        public void TestInitialize()
        {
            _builder = OptimalSearchTreeBuilder.New<SearchElement>(e => e.SearchProbability);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAverageSearchTime_ShouldThrowException_IfThereAreNoElements()
        {
            _builder.GetAverageSearchTime();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetAverageSearchTime_ShouldReturnElementWeight_IfThereIsOnlyOneElement()
        {
            var searchTime = _builder.GetAverageSearchTime(new SearchElement(1, 0.45));

            Assert.AreEqual(0.45, searchTime, 0.001);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetAverageSearchTime_ShouldReturnCorrectTime_IfThereAreOnlyTwoElements()
        {
            var searchTime = _builder.GetAverageSearchTime(new SearchElement(1, 0.8), new SearchElement(2, 0.2));

            Assert.AreEqual(1.2, searchTime, 0.001);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetAverageSearchTime_ShouldReturnCorrectTime_IfThereAreOnlyThreeElements()
        {
            var searchTime = _builder.GetAverageSearchTime(new SearchElement(1, 0.8), new SearchElement(2, 0.1), new SearchElement(3, 0.1));

            Assert.AreEqual(1.3, searchTime, 0.001);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetAverageSearchTime_ShouldReturnCorrectTime_IfThereAreOnlyFourElements()
        {
            var searchTime = _builder.GetAverageSearchTime(new SearchElement(1, 0.02), new SearchElement(2, 0.23), new SearchElement(3, 0.73), new SearchElement(4, 0.01));

            Assert.AreEqual(1.27, searchTime, 0.001);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetAverageSearchTime_ShouldReturnCorrectTime_IfThereAreOnlyFourElements2()
        {
            var searchTime = _builder.GetAverageSearchTime(new SearchElement(1, 0.01), new SearchElement(2, 0.34), new SearchElement(3, 0.33), new SearchElement(4, 0.22));

            Assert.AreEqual(1.48, searchTime, 0.001);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetAverageSearchTime_ShouldReturnCorrectTime_IfThereAreOnlySevenElements()
        {
            var searchTime = _builder.GetAverageSearchTime(
                new SearchElement(1, 0.05),
                new SearchElement(2, 0.40),
                new SearchElement(3, 0.08),
                new SearchElement(4, 0.04),
                new SearchElement(5, 0.10),
                new SearchElement(6, 0.10),
                new SearchElement(7, 0.23));

            Assert.AreEqual(2.18, searchTime, 0.001);
        }
    }
}
