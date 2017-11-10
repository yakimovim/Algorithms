using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    [TestClass]
    public class SuffixTreeApproximateSearchTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void SearchEmptyString_ForNonEmptyPattern_NoErrorsAllowed()
        {
            Assert.AreEqual(0, Search("", new[] { "AAA" }, 0).Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void SearchEmptyString_ForNonEmptyPattern_SomeErrorsAllowed()
        {
            Assert.AreEqual(0, Search("", new[] { "AAA" }, 3).Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void SearchNonEmptyString_ForEmptyPattern_NoErrorsAllowed()
        {
            var searchResult = Search("AAA", new[] { "" }, 0);

            Assert.AreEqual(3, searchResult.Length);

            Assert.IsTrue(searchResult.All(m => m.Length == 0));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void SearchNonEmptyString_ForEmptyPattern_SomeErrorsAllowed()
        {
            var searchResult = Search("AAA", new[] { "" }, 1);

            Assert.AreEqual(3, searchResult.Length);

            Assert.IsTrue(searchResult.All(m => m.Length == 0));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheBeginning_NoErrorsAllowed()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "WE " }, 0);

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(0, searchResult[0].Start);
            Assert.AreEqual(3, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheBeginning_SomeErrorsAllowed()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "WED" }, 1);

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(0, searchResult[0].Start);
            Assert.AreEqual(3, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheMiddle_NoErrorsAllowed()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "TRUTH" }, 0);

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(14, searchResult[0].Start);
            Assert.AreEqual(5, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheMiddle_SomeErrorsAllowed()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "TRATH" }, 1);

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(14, searchResult[0].Start);
            Assert.AreEqual(5, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheEnd_NoErrorsAllowed()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "EVIDENT" }, 0);

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(32, searchResult[0].Start);
            Assert.AreEqual(7, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MatchInTheEnd_SomeErrorsAllowed()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "EVIDEKT" }, 1);

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(32, searchResult[0].Start);
            Assert.AreEqual(7, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_SeveralMatches_NoErrorsAllowed()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "E " }, 0);

            Assert.AreEqual(3, searchResult.Length);
            Assert.AreEqual(1, searchResult[0].Start);
            Assert.AreEqual(2, searchResult[0].Length);
            Assert.AreEqual(12, searchResult[1].Start);
            Assert.AreEqual(2, searchResult[1].Length);
            Assert.AreEqual(25, searchResult[2].Start);
            Assert.AreEqual(2, searchResult[2].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_SeveralMatches_SomeErrorsAllowed()
        {
            var searchResult = Search("WE HOLD THESE TRUTHS TO BE SELF EVIDENT", new[] { "E " }, 1);

            Assert.AreEqual(11, searchResult.Length);
            Assert.IsTrue(searchResult.All(sr => sr.Length == 2));
            CollectionAssert.AreEquivalent(new [] { 1, 6, 10, 12, 19, 22, 25, 28, 30, 32, 36 }, searchResult.Select(sr => sr.Start).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_RussianText_NoErrorsAllowed()
        {
            var searchResult = Search("Съешь ещё этих мягких французских булочек", new[] { "француз" }, 0);

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(22, searchResult[0].Start);
            Assert.AreEqual(7, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_RussianText_SomeErrorsAllowed()
        {
            var searchResult = Search("Съешь ещё этих мягких французских булочек", new[] { "френцух" }, 2);

            Assert.AreEqual(1, searchResult.Length);
            Assert.AreEqual(22, searchResult[0].Start);
            Assert.AreEqual(7, searchResult[0].Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MultipleMatches_NoErrorsAllowed()
        {
            var searchResult = Search("panamabanana", new[] { "ana", "ban" }, 0);

            Assert.AreEqual(4, searchResult.Length);
            Assert.IsTrue(searchResult.All(sr => sr.Length == 3));
            CollectionAssert.AreEqual(new[] { 1, 6, 7, 9 }, searchResult.Select(m => m.Start).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_MultipleMatches_SomeErrorsAllowed()
        {
            var searchResult = Search("panamabanana", new[] { "ana", "ban" }, 1);

            Assert.AreEqual(8, searchResult.Length);
            Assert.IsTrue(searchResult.All(sr => sr.Length == 3));
            CollectionAssert.AreEqual(new[] { 0, 1, 3, 5, 6, 7, 8, 9 }, searchResult.Select(m => m.Start).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_NoMatch_NoErrorsAllowed()
        {
            var searchResult = Search("ATCATATGGT", new[] { "AA" }, 0);

            Assert.AreEqual(0, searchResult.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_NoMatch_SomeErrorsAllowed()
        {
            var searchResult = Search("ATCATATGGT", new[] { "AAM" }, 1);

            Assert.AreEqual(0, searchResult.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_NumberOfErrorsEqualsPatternLength()
        {
            var searchResult = Search("ATCATA", new[] { "AA" }, 2);

            Assert.AreEqual(5, searchResult.Length);
            Assert.IsTrue(searchResult.All(sr => sr.Length == 2));
            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3, 4 }, searchResult.Select(m => m.Start).ToArray());
        }

        private StringSearchMatch[] Search(string toSearchIn, string[] patterns, uint numberOfErrors, IComparer<char> comparer = null) => SuffixTreeApproximateSearch<char>.Search(toSearchIn.ToCharArray(), '$', patterns.Select(p => p.ToCharArray()), numberOfErrors, comparer).OrderBy(m => m.Start).ToArray();
    }
}