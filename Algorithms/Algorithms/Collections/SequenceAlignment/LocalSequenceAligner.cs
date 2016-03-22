using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Collections;

namespace EdlinSoftware.Algorithms.Collections.SequenceAlignment
{
    /// <summary>
    /// Represents algorithm of local alignment of two sequences.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols in sequences.</typeparam>
    public class LocalSequenceAligner<TSymbol> : SequenceAligner<TSymbol>
    {
        public LocalSequenceAligner(TSymbol alignSymbol, Func<TSymbol, TSymbol, decimal> penalty, BestChoiceDelegate bestChoice = null)
            : base(alignSymbol, penalty, bestChoice)
        {}

        public override ISequenceAlignment<TSymbol> Align(IList<TSymbol> firstSequence, IList<TSymbol> secondSequence)
        {
            firstSequence = firstSequence ?? new TSymbol[0];
            secondSequence = secondSequence ?? new TSymbol[0];

            var alignmentMatrix = GetAlignmentMatrix(firstSequence, secondSequence, false);

            var alignment = new SequenceAlignment<TSymbol>();

            ReconstructSequences(alignment, alignmentMatrix, firstSequence, secondSequence);

            return alignment;
        }

        private void ReconstructSequences(SequenceAlignment<TSymbol> alignment, decimal[,] alignmentMatrix, IList<TSymbol> firstSequence, IList<TSymbol> secondSequence)
        {
            var firstAlignedSequence = new LinkedList<TSymbol>();
            var secondAlignedSequence = new LinkedList<TSymbol>();

            var bestPosition = GetAlignmentMatrixBestPosition(alignmentMatrix);

            var i = bestPosition.Item1;
            var j = bestPosition.Item2;

            alignment.Penalty = alignmentMatrix[i, j];


            while ((i > 0) && (j > 0))
            {
                if (alignmentMatrix[i, j] == 0.0m)
                {
                    break;
                }

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

            alignment.FirstAlignedSequence = firstAlignedSequence.ToArray();
            alignment.SecondAlignedSequence = secondAlignedSequence.ToArray();
        }

        private Tuple<int, int> GetAlignmentMatrixBestPosition(decimal[,] alignmentMatrix)
        {
            var bestValue = alignmentMatrix[0, 0];
            var bestRow = 0;
            var bestCol = 0;

            for (int i = 0; i < alignmentMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < alignmentMatrix.GetLength(1); j++)
                {
                    if (BestChoice(alignmentMatrix[i, j], bestValue) == alignmentMatrix[i, j])
                    {
                        bestValue = alignmentMatrix[i, j];
                        bestRow = i;
                        bestCol = j;
                    }
                }
            }

            return Tuple.Create(bestRow, bestCol);
        }
    }
}