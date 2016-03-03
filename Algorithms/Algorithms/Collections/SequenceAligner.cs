using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Collections;

namespace EdlinSoftware.Algorithms.Collections
{
    public class SequenceAligner<TSymbol>
    {
        private readonly TSymbol _alignSymbol;
        private readonly Func<TSymbol, TSymbol, double> _penalty;

        public SequenceAligner(TSymbol alignSymbol, Func<TSymbol, TSymbol, double> penalty)
        {
            if (penalty == null) throw new ArgumentNullException(nameof(penalty));

            _alignSymbol = alignSymbol;
            _penalty = penalty;
        }

        public ISequenceAlignment<TSymbol> Align(TSymbol[] firstSequence, TSymbol[] secondSequence)
        {
            firstSequence = firstSequence ?? new TSymbol[0];
            secondSequence = secondSequence ?? new TSymbol[0];

            var alignmentMatrix = GetAlignmentMatrix(firstSequence, secondSequence);

            var alignment = new SequenceAlignment<TSymbol>
            {
                Penalty = alignmentMatrix[alignmentMatrix.GetLength(0) - 1, alignmentMatrix.GetLength(1) - 1]
            };

            ReconstructSequences(alignment, alignmentMatrix, firstSequence, secondSequence);

            return alignment;
        }

        private double[,] GetAlignmentMatrix(TSymbol[] firstSequence, TSymbol[] secondSequence)
        {
            var size1 = firstSequence.Length + 1;
            var size2 = secondSequence.Length + 1;

            var alignmentMatrix = new double[size1, size2];

            InitializeAlignmentMatrix(alignmentMatrix, firstSequence, secondSequence);

            for (int i = 1; i < size1; i++)
			{
			    for (int j = 1; j < size2; j++)
			    {
                    alignmentMatrix[i,j] = Min(
                        alignmentMatrix[i - 1, j - 1] + _penalty(firstSequence[i - 1], secondSequence[j - 1]),
                        alignmentMatrix[i, j - 1] + _penalty(_alignSymbol, secondSequence[j - 1]),
                        alignmentMatrix[i - 1, j] + _penalty(firstSequence[i - 1], _alignSymbol)
                        );
			    }
			}

            return alignmentMatrix;
        }

        private void InitializeAlignmentMatrix(double[,] alignmentMatrix, TSymbol[] firstSequence, TSymbol[] secondSequence)
        {
            var size1 = alignmentMatrix.GetLength(0);
            var size2 = alignmentMatrix.GetLength(1);

            for (int i = 1; i < size1; i++)
            {
                alignmentMatrix[i, 0] = alignmentMatrix[i - 1, 0] + _penalty(firstSequence[i - 1], _alignSymbol);
            }

            for (int i = 1; i < size2; i++)
            {
                alignmentMatrix[0, i] = alignmentMatrix[0, i - 1] + _penalty(_alignSymbol, secondSequence[i - 1]);
            }
        }

        private double Min(params double[] values)
        {
            return values.Min();
        }

        private void ReconstructSequences(SequenceAlignment<TSymbol> alignment, double[,] alignmentMatrix, TSymbol[] firstSequence, TSymbol[] secondSequence)
        {
            var firstAlignedSequence = new LinkedList<TSymbol>();
            var secondAlignedSequence = new LinkedList<TSymbol>();

            var i = alignmentMatrix.GetLength(0) - 1;
            var j = alignmentMatrix.GetLength(1) - 1;

            while (i > 0 || j > 0)
            {
                if (alignmentMatrix[i, j] == alignmentMatrix[i - 1, j - 1] + _penalty(firstSequence[i - 1], secondSequence[j - 1])
                    && i > 0 && j > 0)
                {
                    firstAlignedSequence.AddFirst(firstSequence[i - 1]);
                    secondAlignedSequence.AddFirst(secondSequence[j - 1]);
                    i--;
                    j--;
                }
                else if(alignmentMatrix[i, j] == alignmentMatrix[i, j - 1] + _penalty(_alignSymbol, secondSequence[j - 1])
                    && j > 0)
                {
                    firstAlignedSequence.AddFirst(_alignSymbol);
                    secondAlignedSequence.AddFirst(secondSequence[j - 1]);
                    j--;
                }
                else
                {
                    firstAlignedSequence.AddFirst(firstSequence[i - 1]);
                    secondAlignedSequence.AddFirst(_alignSymbol);
                    i--;
                }
            }

            alignment.FirstAlignedSequence = firstAlignedSequence.ToArray();
            alignment.SecondAlignedSequence = secondAlignedSequence.ToArray();
        }
    }
}
