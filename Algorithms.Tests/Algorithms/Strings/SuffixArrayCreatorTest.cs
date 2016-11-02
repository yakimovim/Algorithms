using System.Collections.Generic;
using EdlinSoftware.Algorithms.Strings;
using EdlinSoftware.DataStructures.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    [TestClass]
    public class SuffixArrayCreatorTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void GetNumberOfMatches()
        {
            CollectionAssert.AreEqual(new[] { 3, 1, 2, 0 }, GetSuffixArray("GAC$"));
            CollectionAssert.AreEqual(new[] { 8, 7, 5, 3, 1, 6, 4, 2, 0 }, GetSuffixArray("GAGAGAGA$"));
            CollectionAssert.AreEqual(new[] { 15, 14, 0, 1, 12, 6, 4, 2, 8, 13, 3, 7, 9, 10, 11, 5 }, GetSuffixArray("AACGATAGCGGTAGA$"));
        }

        private int[] GetSuffixArray(string text)
        {
            return SuffixArrayCreator<char>.GetSuffixArray(text.ToCharArray(),
                new StopSymbolFirstComparer<char>(Comparer<char>.Default, '$'));
        }
    }
}