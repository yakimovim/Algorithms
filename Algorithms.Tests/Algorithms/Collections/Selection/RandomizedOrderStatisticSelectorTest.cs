using System.Collections.Generic;
using EdlinSoftware.Algorithms.Collections.Selection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Selection
{
    [TestClass]
    public class RandomizedOrderStatisticSelectorTest : BaseOrderStatisticSelectorTest<RandomizedOrderStatisticSelector<int>>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _selector = new RandomizedOrderStatisticSelector<int>(Comparer<int>.Default);
        }
    }
}
