using System;
using EdlinSoftware.Algorithms.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections
{
    [TestClass]
    public class SequenceAlignerTest
    {
        private static readonly Func<char, char, double> Penalty = (a, b) => 
        {
            if (a == b) return 0.0;
            if(a == ' ' || b == ' ') return 0.5;
            return 1.0;
        };

        private SequenceAligner<char> _aligner;

        [TestInitialize]
        public void TestInitialize()
        {
            _aligner = new SequenceAligner<char>(' ', Penalty);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfBothSequencesAreEmpty()
        {
            var alignment = _aligner.Align("".ToCharArray(), "".ToCharArray());

            Assert.AreEqual(0.0, alignment.Penalty, 0.0001);
            Assert.AreEqual("", new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfBothSequencesAreEqual()
        {
            var text = "abcd";

            var alignment = _aligner.Align(text.ToCharArray(), text.ToCharArray());

            Assert.AreEqual(0.0, alignment.Penalty, 0.0001);
            Assert.AreEqual(text, new string(alignment.FirstAlignedSequence));
            Assert.AreEqual(text, new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfOneSequenceIsShorter()
        {
            var text1 = "abcd";
            var text2 = "abc";

            var alignment = _aligner.Align(text1.ToCharArray(), text2.ToCharArray());

            Assert.AreEqual(0.5, alignment.Penalty, 0.0001);
            Assert.AreEqual(text1, new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("abc ", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfOneSequenceIsShorter_GapInCenter()
        {
            var text1 = "abcd";
            var text2 = "abd";

            var alignment = _aligner.Align(text1.ToCharArray(), text2.ToCharArray());

            Assert.AreEqual(0.5, alignment.Penalty, 0.0001);
            Assert.AreEqual(text1, new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("ab d", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_ForComplexCase()
        {
            var text1 = "agggct";
            var text2 = "aggca";

            var alignment = _aligner.Align(text1.ToCharArray(), text2.ToCharArray());

            Assert.AreEqual(1.5, alignment.Penalty, 0.0001);
            Assert.AreEqual(text1, new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("a ggca", new string(alignment.SecondAlignedSequence));
        }
    }
}
