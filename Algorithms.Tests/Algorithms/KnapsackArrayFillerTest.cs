using EdlinSoftware.Algorithms;
using EdlinSoftware.Tests.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms
{
    [TestClass]
    public class KnapsackArrayFillerTest : KnapsackFillerTest<KnapsackArrayFiller<KnapsackItem>>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _filler = new KnapsackArrayFiller<KnapsackItem>(i => i.Value, i => i.Size);
        }
    }
}
