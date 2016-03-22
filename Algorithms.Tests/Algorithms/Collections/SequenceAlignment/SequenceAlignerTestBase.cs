using EdlinSoftware.Algorithms.Collections.SequenceAlignment;
using EdlinSoftware.DataStructures.Collections;

namespace EdlinSoftware.Tests.Algorithms.Collections.SequenceAlignment
{
    public abstract class SequenceAlignerTestBase<TAligner>
        where TAligner : SequenceAligner<char>
    {
        protected TAligner Aligner;

        protected ISequenceAlignment<char> Align(string firstSequence, string secondSequence)
            => Aligner.Align(firstSequence.ToCharArray(), secondSequence.ToCharArray());

    }
}