using EdlinSoftware.Algorithms.Codes;
using EdlinSoftware.Tests.DataStructures.Codes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Codes
{
    [TestClass]
    public class HuffmanCodeQueueBuilderTest : HuffmanCodeBuilderTest<HuffmanCodeQueueBuilder<SymbolAndFrequency<char>>>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Builder = new HuffmanCodeQueueBuilder<SymbolAndFrequency<char>>(a => a.Frequency);
        }
    }
}
