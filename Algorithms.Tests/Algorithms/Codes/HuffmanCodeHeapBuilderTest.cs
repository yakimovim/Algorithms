using EdlinSoftware.Algorithms.Codes;
using EdlinSoftware.Tests.DataStructures.Codes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Codes
{
    [TestClass]
    public class HuffmanCodeHeapBuilderTest : HuffmanCodeBuilderTest<HuffmanCodeHeapBuilder<SymbolAndFrequency<char>>>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _builder = new HuffmanCodeHeapBuilder<SymbolAndFrequency<char>>(a => a.Frequency);
        }
    }
}
