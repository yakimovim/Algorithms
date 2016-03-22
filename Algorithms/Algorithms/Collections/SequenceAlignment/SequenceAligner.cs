using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Collections;

namespace EdlinSoftware.Algorithms.Collections.SequenceAlignment
{
    /// <summary>
    /// Represents algorithm of alignment of two sequences.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols in sequences.</typeparam>
    public abstract class SequenceAligner<TSymbol>
    {
        public delegate decimal BestChoiceDelegate(params decimal[] variants);

        protected readonly TSymbol AlignSymbol;
        protected readonly Func<TSymbol, TSymbol, decimal> Penalty;
        protected readonly BestChoiceDelegate BestChoice;

        protected SequenceAligner(TSymbol alignSymbol, Func<TSymbol, TSymbol, decimal> penalty, BestChoiceDelegate bestChoice = null)
        {
            if (penalty == null) throw new ArgumentNullException(nameof(penalty));

            AlignSymbol = alignSymbol;
            Penalty = penalty;
            BestChoice = bestChoice ?? Min;
        }

        public abstract ISequenceAlignment<TSymbol> Align(IList<TSymbol> firstSequence, IList<TSymbol> secondSequence);

        protected decimal[,] GetAlignmentMatrix(IList<TSymbol> firstSequence, IList<TSymbol> secondSequence, bool isGlobal)
        {
            var size1 = firstSequence.Count + 1;
            var size2 = secondSequence.Count + 1;

            var alignmentMatrix = new decimal[size1, size2];

            InitializeAlignmentMatrix(alignmentMatrix, firstSequence, secondSequence, isGlobal);

            for (int i = 1; i < size1; i++)
            {
                for (int j = 1; j < size2; j++)
                {
                    alignmentMatrix[i, j] = GetAlignmentMatrixValue(BestChoice(
                        alignmentMatrix[i - 1, j - 1] + Penalty(firstSequence[i - 1], secondSequence[j - 1]),
                        alignmentMatrix[i, j - 1] + Penalty(AlignSymbol, secondSequence[j - 1]),
                        alignmentMatrix[i - 1, j] + Penalty(firstSequence[i - 1], AlignSymbol)
                        ), isGlobal);
                }
            }

            return alignmentMatrix;
        }

        private void InitializeAlignmentMatrix(decimal[,] alignmentMatrix, IList<TSymbol> firstSequence, IList<TSymbol> secondSequence, bool isGlobal)
        {
            var size1 = alignmentMatrix.GetLength(0);
            var size2 = alignmentMatrix.GetLength(1);

            for (int i = 1; i < size1; i++)
            {
                alignmentMatrix[i, 0] = GetAlignmentMatrixValue(alignmentMatrix[i - 1, 0] + Penalty(firstSequence[i - 1], AlignSymbol), isGlobal);
            }

            for (int i = 1; i < size2; i++)
            {
                alignmentMatrix[0, i] = GetAlignmentMatrixValue(alignmentMatrix[0, i - 1] + Penalty(AlignSymbol, secondSequence[i - 1]), isGlobal);
            }
        }

        private decimal GetAlignmentMatrixValue(decimal value, bool isGlobal)
        {
            if (isGlobal)
                return value;
            return BestChoice(0.0m, value);
        }

        private static decimal Min(params decimal[] values)
        {
            return values.Min();
        }
    }
}