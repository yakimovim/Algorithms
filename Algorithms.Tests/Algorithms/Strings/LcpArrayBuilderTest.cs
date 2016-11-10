using System.Collections.Generic;
using EdlinSoftware.Algorithms.Strings;
using EdlinSoftware.DataStructures.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    [TestClass]
    public class LcpArrayBuilderTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void ComputeLcpArray_Correct()
        {
            var text = "ababaa$";

            var suffixArray = SuffixArrayCreator<char>.GetSuffixArrayFast(text.ToCharArray(), StopSymbolCharComparer.Instance);

            var lcpArray = LcpArrayCreator<char>.ComputeLcpArray(text.ToCharArray(), suffixArray, StopSymbolCharComparer.Instance);

            CollectionAssert.AreEqual(new [] { 0, 1, 1, 3, 0, 2 }, lcpArray);
        }
    }
}