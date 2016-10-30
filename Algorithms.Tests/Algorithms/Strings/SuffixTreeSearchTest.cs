using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    [TestClass]
    public class SuffixTreeSearchTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void SearchEmptyString_ForNonEmptyPattern()
        {
            Assert.AreEqual(0, Search("", new[] { "AAA" }).Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void SearchNonEmptyString_ForEmptyPattern()
        {
            var searchResult = Search("AAA", new[] { "" });

            Assert.AreEqual(3, searchResult.Length);

            Assert.IsTrue(searchResult.All(m => m.Length == 0));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheBeginning()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "WE " });

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(0, searchResult[0].Start);
            Assert.AreEqual(3, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheMiddle()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "TRUTH" });

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(14, searchResult[0].Start);
            Assert.AreEqual(5, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheEnd()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "EVIDENT" });

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(32, searchResult[0].Start);
            Assert.AreEqual(7, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_SeveralMatches()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "E " });

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
            var searchResult = Search("Съешь ещё этих мягких французских булочек", new[] { "француз" });

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(22, searchResult[0].Start);
            Assert.AreEqual(7, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MultipleMatches()
        {
            var searchResult = Search("panamabanana", new[] { "ana", "na", "a" });

            Assert.AreEqual(12, searchResult.Length);

            var anaMarches = searchResult.Where(m => m.Length == 3).ToArray();
            Assert.AreEqual(3, anaMarches.Length);
            var naMarches = searchResult.Where(m => m.Length == 2).ToArray();
            Assert.AreEqual(3, naMarches.Length);
            var aMarches = searchResult.Where(m => m.Length == 1).ToArray();
            Assert.AreEqual(6, aMarches.Length);

            CollectionAssert.AreEqual(new [] { 1, 7, 9 }, anaMarches.Select(m => m.Start).ToArray());
            CollectionAssert.AreEqual(new[] { 2, 8, 10 }, naMarches.Select(m => m.Start).ToArray());
            CollectionAssert.AreEqual(new[] { 1, 3, 5, 7, 9, 11 }, aMarches.Select(m => m.Start).ToArray());
        }

        private StringSearchMatch[] Search(string toSearchIn, string[] patterns, IComparer<char> comparer = null) => SuffixTreeSearch<char>.Search(toSearchIn.ToCharArray(), '$', patterns.Select(p => p.ToCharArray()), comparer).OrderBy(m => m.Start).ToArray();
    }
}