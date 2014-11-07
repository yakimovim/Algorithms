using EdlinSoftware.Algorithms.Collections.Selection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Selection
{
    [TestClass]
    public class DeterministicOrderStatisticSelectorTest : BaseOrderStatisticSelectorTest<DeterministicOrderStatisticSelector<int>>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Selector = DeterministicOrderStatisticSelector.New<int>();
        }
    }
}
