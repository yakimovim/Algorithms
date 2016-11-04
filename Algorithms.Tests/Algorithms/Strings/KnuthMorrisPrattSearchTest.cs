using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    [TestClass]
    public class KnuthMorrisPrattSearchTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void SearchEmptyString_ForNonEmptyPattern()
        {
            Assert.AreEqual(0, Search("", "AAA").Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void SearchNonEmptyString_ForEmptyPattern()
        {
            Assert.AreEqual(0, Search("AAA", "").Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheBeginning()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", "WE ");

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(0, searchResult[0].Start);
            Assert.AreEqual(3, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheMiddle()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", "TRUTH");

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(14, searchResult[0].Start);
            Assert.AreEqual(5, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheEnd()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", "EVIDENT");

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(32, searchResult[0].Start);
            Assert.AreEqual(7, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_SeveralMatches()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", "E ");

            Assert.AreEqual(3, searchResult.Length);
            Assert.AreEqual(1, searchResult[0].Start);
            Assert.AreEqual(2, searchResult[0].Length);
            Assert.AreEqual(12, searchResult[1].Start);
            Assert.AreEqual(2, searchResult[1].Length);
            Assert.AreEqual(25, searchResult[2].Start);
            Assert.AreEqual(2, searchResult[2].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_RussianText()
        {
            var searchResult = Search("Съешь ещё этих мягких французских булочек", "француз");

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(22, searchResult[0].Start);
            Assert.AreEqual(7, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_CustomComparer()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", "truth", new CaseInsensitiveCharEqualityComparer());

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(14, searchResult[0].Start);
            Assert.AreEqual(5, searchResult[0].Length);
        }

        private StringSearchMatch[] Search(string toSearchIn, string pattern, IEqualityComparer<char> comparer = null) => KnuthMorrisPrattSearch<char>.Search(toSearchIn.ToCharArray(), pattern.ToCharArray(), '$', comparer).ToArray();
    }
}