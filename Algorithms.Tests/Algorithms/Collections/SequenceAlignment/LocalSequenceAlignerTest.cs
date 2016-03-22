using System;
using System.Linq;
using EdlinSoftware.Algorithms.Collections.SequenceAlignment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.SequenceAlignment
{
    [TestClass]
    public class LocalSequenceAlignerTest : SequenceAlignerTestBase<LocalSequenceAligner<char>>
    {
        private static readonly Func<char, char, decimal> Penalty = (a, b) =>
        {
            if (a == b) return 10.0m;
            if (a == ' ' || b == ' ') return -8.0m;
            return 4.0m;
        };

        private static decimal Max(params decimal[] values) => values.Max();

        [TestInitialize]
        public void TestInitialize()
        {
            Aligner = new LocalSequenceAligner<char>(' ', Penalty, Max);
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

            Assert.AreEqual(0.0m, alignment.Penalty);
            Assert.AreEqual("", new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfSecondSequenceIsEmpty()
        {
            var alignment = Align("abcd", "");

            Assert.AreEqual(0.0m, alignment.Penalty);
            Assert.AreEqual("", new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfBothSequencesAreEqual()
        {
            var text = "abcd";

            var alignment = Align(text, text);

            Assert.AreEqual(40.0m, alignment.Penalty);
            Assert.AreEqual(text, new string(alignment.FirstAlignedSequence));
            Assert.AreEqual(text, new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfOneSequenceIsShorter()
        {
            var text1 = "abcd";
            var text2 = "abc";

            var alignment = Align(text1, text2);

            Assert.AreEqual(30.0m, alignment.Penalty);
            Assert.AreEqual("abc", new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("abc", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_IfOneSequenceIsShorter_GapInCenter()
        {
            var text1 = "abcd";
            var text2 = "abd";

            var alignment = Align(text1, text2);

            Assert.AreEqual(24.0m, alignment.Penalty);
            Assert.AreEqual("abc", new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("abd", new string(alignment.SecondAlignedSequence));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Align_ShouldReturnCorrectResult_ForComplexCase()
        {
            Func<char, char, decimal> penalty = (a, b) => {
                if (a == b) return 10.0m;
                if (a == ' ' || b == ' ') return -4.0m;
                return 2.0m;
            };

            Aligner = new LocalSequenceAligner<char>(' ', penalty, Max);

            var text1 = "acc";
            var text2 = "tttacacgg";

            var alignment = Align(text1, text2);

            Assert.AreEqual(26.0m, alignment.Penalty);
            Assert.AreEqual("ac c", new string(alignment.FirstAlignedSequence));
            Assert.AreEqual("acac", new string(alignment.SecondAlignedSequence));
        }
    }
}