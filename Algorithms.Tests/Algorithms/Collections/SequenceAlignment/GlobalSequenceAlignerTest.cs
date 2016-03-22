using System;
using EdlinSoftware.Algorithms.Collections.SequenceAlignment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.SequenceAlignment
{
    [TestClass]
    public class GlobalSequenceAlignerTest : SequenceAlignerTestBase<GlobalSequenceAligner<char>>
    {
        private static readonly Func<char, char, decimal> Penalty = (a, b) => 
        {
            if (a == b) return 0.0m;
            if(a == ' ' || b == ' ') return 0.5m;
            return 1.0m;
        };

        [TestInitialize]
        public void TestInitialize()
        {
            Aligner = new GlobalSequenceAligner<char>(' ', Penalty);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfBothSequencesAreEmpty()
        {
            var alignment = Align("", "");

            Assert.AreEqual(0.0m, alignment.Penalty);
            Assert.AreEqual("", new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfFirstSequenceIsEmpty()
        {
            var alignment = Align("", "abcd");

            Assert.AreEqual(2.0m, alignment.Penalty);
            Assert.AreEqual("    ", new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("abcd", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfSecondSequenceIsEmpty()
        {
            var alignment = Align("abcd", "");

            Assert.AreEqual(2.0m, alignment.Penalty);
            Assert.AreEqual("abcd", new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("    ", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfBothSequencesAreEqual()
        {
            var text = "abcd";

            var alignment = Align(text, text);

            Assert.AreEqual(0.0m, alignment.Penalty);
            Assert.AreEqual(text, new string(alignment.FirstAlignedSequence));
            Assert.AreEqual(text, new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfOneSequenceIsShorter()
        {
            var text1 = "abcd";
            var text2 = "abc";

            var alignment = Align(text1, text2);

            Assert.AreEqual(0.5m, alignment.Penalty);
            Assert.AreEqual(text1, new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("abc ", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfOneSequenceIsShorter_GapInCenter()
        {
            var text1 = "abcd";
            var text2 = "abd";

            var alignment = Align(text1, text2);

            Assert.AreEqual(0.5m, alignment.Penalty);
            Assert.AreEqual(text1, new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("ab d", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_ForComplexCase()
        {
            var text1 = "agggct";
            var text2 = "aggca";

            var alignment = Align(text1, text2);

            Assert.AreEqual(1.5m, alignment.Penalty);
            Assert.AreEqual(text1, new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("a ggca", new string(alignment.SecondAlignedSequence));
        }
    }
}
