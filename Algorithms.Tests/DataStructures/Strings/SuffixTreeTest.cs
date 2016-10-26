using EdlinSoftware.DataStructures.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.DataStructures.Strings
{
    [TestClass]
    public class SuffixTreeTest
    {
        [TestMethod]
        public void Test()
        {
            var suffixTree = new SuffixTree<char>("A#T$".ToCharArray());

            Assert.IsNotNull(suffixTree);
        }
    }
}