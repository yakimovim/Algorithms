using System.Collections.Generic;
using EdlinSoftware.Algorithms.Strings;
using EdlinSoftware.DataStructures.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    [TestClass]
    public class LcpArrayBuilderTest
    {
        private static readonly IComparer<char> Comparer = new StopSymbolFirstComparer<char>(Comparer<char>.Default, '$');

        [TestMethod, Owner("Ivan Yakimov")]
        public void ComputeLcpArray_Correct()
        {
            var text = "ababaa$";

            var suffixArray = SuffixArrayCreator<char>.GetSuffixArrayFast(text.ToCharArray(), Comparer);

            var lcpArray = LcpArrayBuilder<char>.ComputeLcpArray(text.ToCharArray(), suffixArray, Comparer);

            CollectionAssert.AreEqual(new [] { 0, 1, 1, 3, 0, 2 }, lcpArray);
        }
    }
}