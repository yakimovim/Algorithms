using EdlinSoftware.Algorithms;
using EdlinSoftware.Tests.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms
{
    [TestClass]
    public class KnapsackHashFillerTest : KnapsackFillerTest<KnapsackHashFiller<KnapsackItem>>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _filler = new KnapsackHashFiller<KnapsackItem>(
                i => i.Value, 
                i => i.Size,
                (itemNumber, restCapacity) => (int) ((((int)restCapacity) << ((int)10)) + itemNumber));
        }
    }
}
