using System.Linq;
using EdlinSoftware.Algorithms.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    [TestClass]
    public class PrefixFunctionTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void GetPrefixFunction_Correct()
        {
            CollectionAssert.AreEqual(new int[0], GetPrefixFunction(""));
            CollectionAssert.AreEqual(new[] { 0 }, GetPrefixFunction("A"));
            CollectionAssert.AreEqual(new [] { 0, 0, 1, 0, 1, 2, 3, 4, 5, 6, 7, 2, 3 }, GetPrefixFunction("ACATACATACACA") );
        }

        private int[] GetPrefixFunction(string text)
        {
            return PrefixFunction<char>.GetPrefixFunction(text.ToCharArray()).ToArray();
        }
    }
}