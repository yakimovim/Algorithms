using EdlinSoftware.DataStructures.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.DataStructures.Strings
{
    [TestClass]
    public class StringCyclicRotationComparerTest
    {
        private readonly StringCyclicRotationComparer<char> _comparer = new StringCyclicRotationComparer<char>();

        [TestMethod, Owner("Ivan Yakimov")]
        public void Compare_SameLength()
        {
            var rotation1 = GetRotation("ABC", -1);
            var rotation2 = GetRotation("ABC", -2);
            var rotation3 = GetRotation("ABC", -3);

            Assert.IsTrue(_comparer.Compare(rotation1, rotation2) > 0);
            Assert.IsTrue(_comparer.Compare(rotation2, rotation3) > 0);
            Assert.IsTrue(_comparer.Compare(rotation3, rotation1) < 0);

            Assert.IsTrue(_comparer.Compare(rotation1, rotation1) == 0);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Compare_DifferentLength()
        {
            var rotation1 = GetRotation("ABC", -1);
            var rotation2 = GetRotation("ABDC", -1);

            Assert.IsTrue(_comparer.Compare(rotation1, rotation2) < 0);
        }

        private StringCyclicRotation<char> GetRotation(string text, int offset) => new StringCyclicRotation<char>(text.ToCharArray(), offset);
    }
}