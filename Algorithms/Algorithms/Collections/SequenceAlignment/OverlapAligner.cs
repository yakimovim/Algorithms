using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Collections.SequenceAlignment
{
    /// <summary>
    /// Represents algorithm of overlap alignment of two sequences.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols in sequences.</typeparam>
    public class OverlapAligner<TSymbol>
    {
        private delegate decimal BestChoiceDelegate(params decimal[] variants);

        private readonly TSymbol _alignSymbol;
        private readonly Func<TSymbol, TSymbol, decimal> _penalty;
        private readonly BestChoiceDelegate _bestChoice;

        private decimal _gapPenalty;
        private decimal _mismatchPenalty;

        public OverlapAligner(TSymbol alignSymbol)
        {
            _alignSymbol = alignSymbol;
            _bestChoice = variants => variants.Max();
            _penalty = (x, y) =>
            {
                if (x.Equals(y)) return 1.0m;
                if (_alignSymbol.Equals(x) || _alignSymbol.Equals(y)) return _gapPenalty;
                return _mismatchPenalty;
            };
        }

        public Tuple<int, int> GetOverlapLengths(IList<TSymbol> firstSequence, IList<TSymbol> secondSequence)
        {
            firstSequence = firstSequence ?? new TSymbol[0];
            secondSequence = secondSequence ?? new TSymbol[0];

            _gapPenalty = -Math.Max(firstSequence.Count, secondSequence.Count);
            _mismatchPenalty = _gapPenalty;

            var alignmentMatrix = GetAlignmentMatrix(firstSequence, secondSequence);

            return ExtractOverlapLength(alignmentMatrix, firstSequence, secondSequence);
        }

        private decimal[,] GetAlignmentMatrix(IList<TSymbol> firstSequence, IList<TSymbol> secondSequence)
        {
            var size1 = firstSequence.Count + 1;
            var size2 = secondSequence.Count + 1;

            var alignmentMatrix = new decimal[size1, size2];

            InitializeAlignmentMatrix(alignmentMatrix);

            for (int i = 1; i < size1; i++)
            {
                for (int j = 1; j < size2; j++)
                {
                    alignmentMatrix[i, j] = _bestChoice(
                        alignmentMatrix[i - 1, j - 1] + _penalty(firstSequence[i - 1], secondSequence[j - 1]),
                        alignmentMatrix[i, j - 1] + _penalty(_alignSymbol, secondSequence[j - 1]),
                        alignmentMatrix[i - 1, j] + _penalty(firstSequence[i - 1], _alignSymbol)
                        );
                }
            }

            return alignmentMatrix;
        }

        private void InitializeAlignmentMatrix(decimal[,] alignmentMatrix)
        {
            var size1 = alignmentMatrix.GetLength(0);
            var size2 = alignmentMatrix.GetLength(1);

            for (int i = 1; i < size1; i++)
            {
                alignmentMatrix[i, 0] = 0;
            }

            for (int i = 1; i < size2; i++)
            {
                alignmentMatrix[0, i] = 0;
            }
        }

        private Tuple<int, int> ExtractOverlapLength(decimal[,] alignmentMatrix, IList<TSymbol> firstSequence, IList<TSymbol> secondSequence)
        {
            var size1 = alignmentMatrix.GetLength(0);
            var size2 = alignmentMatrix.GetLength(1);

            var t1 = GetIndexesOnEdge(
                alignmentMatrix,
                firstSequence,
                secondSequence,
                size1 - 1,
                GetIndexOfMaximumElementInTheLastRow(alignmentMatrix));

            var t2 = GetIndexesOnEdge(
                alignmentMatrix,
                firstSequence,
                secondSequence,
                GetIndexOfMaximumElementInTheLastColumn(alignmentMatrix),
                size2 - 1);

            return Tuple.Create(firstSequence.Count - t1.Item1, secondSequence.Count - t2.Item2);
        }

        private int GetIndexOfMaximumElementInTheLastRow(decimal[,] alignmentMatrix)
        {
            var size1 = alignmentMatrix.GetLength(0);
            var size2 = alignmentMatrix.GetLength(1);

            var maxValue = alignmentMatrix[size1 - 1, 0];
            var maxValueIndex = 0;
            for (int k = 1; k < size2; k++)
            {
                if (maxValue < alignmentMatrix[size1 - 1, k])
                {
                    maxValue = alignmentMatrix[size1 - 1, k];
                    maxValueIndex = k;
                }
            }

            return maxValueIndex;
        }

        private int GetIndexOfMaximumElementInTheLastColumn(decimal[,] alignmentMatrix)
        {
            var size1 = alignmentMatrix.GetLength(0);
            var size2 = alignmentMatrix.GetLength(1);

            var maxValue = alignmentMatrix[0, size2 - 1];
            var maxValueIndex = 0;
            for (int k = 1; k < size1; k++)
            {
                if (maxValue < alignmentMatrix[k, size2 - 1])
                {
                    maxValue = alignmentMatrix[k, size2 - 1];
                    maxValueIndex = k;
                }
            }

            return maxValueIndex;
        }

        private Tuple<int, int> GetIndexesOnEdge(decimal[,] alignmentMatrix, IList<TSymbol> firstSequence, IList<TSymbol> secondSequence, int i, int j)
        {
            while ((i > 0) && (j > 0))
            {
                if (alignmentMatrix[i, j] == 0.0m)
                {
                    break;
                }

                if (alignmentMatrix[i, j] == alignmentMatrix[i - 1, j - 1] + _penalty(firstSequence[i - 1], secondSequence[j - 1]))
                {
                    i--;
                    j--;
                }
                else if (alignmentMatrix[i, j] == alignmentMatrix[i, j - 1] + _penalty(_alignSymbol, secondSequence[j - 1]))
                {
                    j--;
                }
                else
                {
                    i--;
                }
            }

            return Tuple.Create(i, j);
        }
    }
}