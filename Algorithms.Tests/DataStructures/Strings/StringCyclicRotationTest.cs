using EdlinSoftware.DataStructures.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.DataStructures.Strings
{
    [TestClass]
    public class StringCyclicRotationTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void CheckNegativeRotation()
        {
            Assert.AreEqual("$ABC", GetRotation("ABC$", -1));
            Assert.AreEqual("C$AB", GetRotation("ABC$", -2));
            Assert.AreEqual("BC$A", GetRotation("ABC$", -3));
            Assert.AreEqual("ABC$", GetRotation("ABC$", -4));
            Assert.AreEqual("$ABC", GetRotation("ABC$", -5));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void CheckZeroRotation()
        {
            Assert.AreEqual("ABC$", GetRotation("ABC$", 0));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void CheckPositiveRotation()
        {
            Assert.AreEqual("BC$A", GetRotation("ABC$", 1));
            Assert.AreEqual("C$AB", GetRotation("ABC$", 2));
            Assert.AreEqual("$ABC", GetRotation("ABC$", 3));
            Assert.AreEqual("ABC$", GetRotation("ABC$", 4));
            Assert.AreEqual("BC$A", GetRotation("ABC$", 5));
        }

        private string GetRotation(string text, int offset) => new StringCyclicRotation<char>(text.ToCharArray(), offset).ToString();
    }
}