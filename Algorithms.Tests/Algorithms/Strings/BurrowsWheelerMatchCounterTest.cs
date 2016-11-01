using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    [TestClass]
    public class BurrowsWheelerMatchCounterTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void GetNumberOfMatches()
        {
            CollectionAssert.AreEqual(new[] { 3 }, GetMathcesCount("AGGGAA$", "GA"));
            CollectionAssert.AreEqual(new[] { 2, 3 }, GetMathcesCount("ATT$AA", "ATA", "A"));
            CollectionAssert.AreEqual(new[] { 0, 0 }, GetMathcesCount("AT$TCTATG", "TCT", "TATG"));
            CollectionAssert.AreEqual(new [] { 2, 1, 1, 0, 1 }, GetMathcesCount("TCCTCTATGAGATCCTATTCTATGAAACCTTCA$GACCAAAATTCTCCGGC", "CCT", "CAC", "GAG", "CAG", "ATC"));
            CollectionAssert.AreEqual(new[] { 0, 1 }, GetMathcesCount("G$", "T", "G"));
        }
        private int[] GetMathcesCount(string transformation, params string[] patterns)
        {
            return BurrowsWheelerMatchCounter<char>.GetNumberOfMatches(transformation.ToCharArray(),
                patterns.Select(p => p.ToCharArray()), '$', Comparer<char>.Default).ToArray();
        }
    }
}