using System;
using EdlinSoftware.Algorithms.Collections.SequenceAlignment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Collections.SequenceAlignment
{
    [TestClass]
    public class OverlapAlignerTest
    {
        private OverlapAligner<char> _aligner;

        [TestInitialize]
        public void TestInitialize()
        {
            _aligner = new OverlapAligner<char>(' ');
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetOverlapLengths_FirstSequencesIsEmpty()
        {
            var overlapLength = Align("", "CCCCGAAA");

            Assert.AreEqual(Tuple.Create(0, 0), overlapLength);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetOverlapLengths_SecondSequencesIsEmpty()
        {
            var overlapLength = Align("AAATCCCC", "");

            Assert.AreEqual(Tuple.Create(0, 0), overlapLength);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetOverlapLengths_BothSequencesHaveOverlaps()
        {
            var overlapLength = Align("AAATCCCC", "CCCCGAAA");

            Assert.AreEqual(Tuple.Create(4, 3), overlapLength);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetOverlapLengths_FirstSequenceHasOverlap()
        {
            var overlapLength = Align("AAATCCCC", "CCCCGTTT");

            Assert.AreEqual(Tuple.Create(4, 0), overlapLength);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetOverlapLengths_SecondSequenceHasOverlap()
        {
            var overlapLength = Align("AAATGGGG", "CCCCGAAA");

            Assert.AreEqual(Tuple.Create(0, 3), overlapLength);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetOverlapLengths_BothSequencesDontHaveOverlaps()
        {
            var overlapLength = Align("ACAC", "GTGT");

            Assert.AreEqual(Tuple.Create(0, 0), overlapLength);
        }

        protected Tuple<int, int> Align(string firstSequence, string secondSequence)
            => _aligner.GetOverlapLengths(firstSequence.ToCharArray(), secondSequence.ToCharArray());
    }
}