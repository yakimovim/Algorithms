using EdlinSoftware.Algorithms.Collections.Selection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.Selection
{
    [TestClass]
    public class OrderStatisticSelectorTest : BaseOrderStatisticSelectorTest<OrderStatisticSelector<int>>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _selector = OrderStatisticSelector.New<int>();
        }
    }
}
