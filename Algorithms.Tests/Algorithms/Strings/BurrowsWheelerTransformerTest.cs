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

        [TestMethod, Owner("Ivan Yakimov")]
        public void InverseTransform()
        {
            Assert.AreEqual("AA$", GetInverseTransformation("AA$"));
            Assert.AreEqual("ACA$", GetInverseTransformation("AC$A"));
            Assert.AreEqual("ACACACAC$", GetInverseTransformation("CCCC$AAAA"));
            Assert.AreEqual("AGACATA$", GetInverseTransformation("ATG$CAAA"));
            Assert.AreEqual("GAGAGA$", GetInverseTransformation("AGGGAA$"));
        }

        private string GetTransformation(string text)
        {
            return string.Join("", BurrowsWheelerTransformer<char>.Transform(text.ToCharArray(), '$', Comparer<char>.Default));
        }
        private string GetInverseTransformation(string text)
        {
            return string.Join("", BurrowsWheelerTransformer<char>.InverseTransform(text.ToCharArray(), '$', Comparer<char>.Default));
        }
    }
}