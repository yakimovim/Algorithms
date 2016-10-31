using System.Collections.Generic;
using EdlinSoftware.Algorithms.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    [TestClass]
    public class BurrowsWheelerTransformerTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void Transform()
        {
            Assert.AreEqual("$", GetTransformation(""));
            Assert.AreEqual("$", GetTransformation("$"));
            Assert.AreEqual("AA$", GetTransformation("AA$"));
            Assert.AreEqual("CCCC$AAAA", GetTransformation("ACACACAC$"));
            Assert.AreEqual("ATG$CAAA", GetTransformation("AGACATA$"));
        }

        private string GetTransformation(string text)
        {
            return string.Join("", BurrowsWheelerTransformer<char>.Transform(text.ToCharArray(), '$', Comparer<char>.Default));
        }
    }
}