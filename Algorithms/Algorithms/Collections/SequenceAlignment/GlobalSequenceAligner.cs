using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Collections;

namespace EdlinSoftware.Algorithms.Collections.SequenceAlignment
{
    /// <summary>
    /// Represents algorithm of global alignment of two sequences.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols in sequences.</typeparam>
    public class GlobalSequenceAligner<TSymbol> : SequenceAligner<TSymbol>
    {
        public GlobalSequenceAligner(TSymbol alignSymbol, Func<TSymbol, TSymbol, decimal> penalty, BestChoiceDelegate bestChoice = null)
            : base(alignSymbol, penalty, bestChoice)
        {}

        public override ISequenceAlignment<TSymbol> Align(IList<TSymbol> firstSequence, IList<TSymbol> secondSequence)
        {
            firstSequence = firstSequence ?? new TSymbol[0];
            secondSequence = secondSequence ?? new TSymbol[0];

            var alignmentMatrix = GetAlignmentMatrix(firstSequence, secondSequence, true);

            var alignment = new SequenceAlignment<TSymbol>
            {
                Penalty = alignmentMatrix[alignmentMatrix.GetLength(0) - 1, alignmentMatrix.GetLength(1) - 1]
            };

            ReconstructSequences(alignment, alignmentMatrix, firstSequence, secondSequence);

            return alignment;
        }

        private void ReconstructSequences(SequenceAlignment<TSymbol> alignment, decimal[,] alignmentMatrix, IList<TSymbol> firstSequence, IList<TSymbol> secondSequence)
        {
            var firstAlignedSequence = new LinkedList<TSymbol>();
            var secondAlignedSequence = new LinkedList<TSymbol>();

            var i = alignmentMatrix.GetLength(0) - 1;
            var j = alignmentMatrix.GetLength(1) - 1;

            while ((i > 0) && (j > 0))
            {
                if (alignmentMatrix[i, j] == alignmentMatrix[i - 1, j - 1] + Penalty(firstSequence[i - 1], secondSequence[j - 1]))
                {
                    firstAlignedSequence.AddFirst(firstSequence[i - 1]);
                    secondAlignedSequence.AddFirst(secondSequence[j - 1]);
                    i--;
                    j--;
                }
                else if (alignmentMatrix[i, j] == alignmentMatrix[i, j - 1] + Penalty(AlignSymbol, secondSequence[j - 1]))
                {
                    firstAlignedSequence.AddFirst(AlignSymbol);
                    secondAlignedSequence.AddFirst(secondSequence[j - 1]);
                    j--;
                }
                else
                {
                    firstAlignedSequence.AddFirst(firstSequence[i - 1]);
                    secondAlignedSequence.AddFirst(AlignSymbol);
                    i--;
                }
            }

            while (i > 0)
            {
                firstAlignedSequence.AddFirst(firstSequence[i - 1]);
                secondAlignedSequence.AddFirst(AlignSymbol);
                i--;
            }

            while (j > 0)
            {
                firstAlignedSequence.AddFirst(AlignSymbol);
                secondAlignedSequence.AddFirst(secondSequence[j - 1]);
                j--;
            }

            alignment.FirstAlignedSequence = firstAlignedSequence.ToArray();
            alignment.SecondAlignedSequence = secondAlignedSequence.ToArray();
        }
    }
}
