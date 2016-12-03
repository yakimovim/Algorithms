using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.LinearProgramming
{
    /// <summary>
    /// Represents solver of system of linear equations using Gaussian elimination.
    /// </summary>
    public static class GaussianElimination
    {
        public static SystemOfLinearEquationsResult Solve(
            [NotNull] decimal[,] leftMatrix,
            [NotNull] decimal[] rightColumn)
        {
            if (leftMatrix == null) throw new ArgumentNullException(nameof(leftMatrix));
            if (rightColumn == null) throw new ArgumentNullException(nameof(rightColumn));
            if(leftMatrix.GetLength(0) != rightColumn.Length)
                throw new ArgumentException("Number of rows in " + nameof(leftMatrix) + " should be equal to number of elements in " + nameof(rightColumn));

            var numberOfVariables = leftMatrix.GetLength(1);

            var augmentedMatrix = GetAugmentedMatrix(leftMatrix, rightColumn);

            var currentEquationIndex = 0;
            var currentVariableIndex = 0;

            var freeVariables = new HashSet<int>();

            while (true)
            {
                if(currentVariableIndex == numberOfVariables)
                    break;

                var equationIndex = GetEquationWithNonZeroCoefficient(augmentedMatrix, currentEquationIndex, currentVariableIndex);
                if (!equationIndex.HasValue)
                {
                    freeVariables.Add(currentVariableIndex);
                    currentVariableIndex++;
                    continue;
                }

                SwapTwoRows(augmentedMatrix, currentEquationIndex, equationIndex.Value);

                var scale = 1.0M / augmentedMatrix[currentEquationIndex, currentVariableIndex];
                ScaleRow(augmentedMatrix, currentEquationIndex, scale);

                AdjustRows(augmentedMatrix, currentEquationIndex, currentVariableIndex);
                currentEquationIndex++;
                currentVariableIndex++;
            }

            return BuildResult(augmentedMatrix, freeVariables, currentEquationIndex);
        }

        private static SystemOfLinearEquationsResult BuildResult(decimal[,] augmentedMatrix, HashSet<int> freeVariables, int currentEquationIndex)
        {
            var solutionExists = CheckSolutionExistence(augmentedMatrix, currentEquationIndex);
            if(!solutionExists)
                return new SystemOfLinearEquationsResult(SystemOfLinearEquationsResultType.NoSolution);

            var type = freeVariables.Count > 0
                ? SystemOfLinearEquationsResultType.InfiniteNumberOfSolutions
                : SystemOfLinearEquationsResultType.OneSolution;

            var numberOfVariables = augmentedMatrix.GetLength(1) - 1;
            var lastColumnIndex = numberOfVariables;

            var variableResults = new VariableResult[numberOfVariables];
            var row = 0;
            for (int col = 0; col < numberOfVariables; col++)
            {
                if (freeVariables.Contains(col))
                {
                    variableResults[col] = new VariableResult();
                }
                else
                {
                    variableResults[col] = new VariableResult(augmentedMatrix[row, lastColumnIndex], 
                        freeVariables.Select(variableIndex => Tuple.Create(variableIndex, -augmentedMatrix[row, variableIndex]))
                        .ToArray());
                    row++;
                }
            }

            return new SystemOfLinearEquationsResult(type, variableResults);
        }

        private static bool CheckSolutionExistence(decimal[,] augmentedMatrix, int currentEquationIndex)
        {
            var numberOfRows = augmentedMatrix.GetLength(0);
            var lastColumnIndex = augmentedMatrix.GetLength(1) - 1;

            for (int row = currentEquationIndex; row < numberOfRows; row++)
            {
                if (augmentedMatrix[row, lastColumnIndex] != 0.0M)
                    return false;
            }

            return true;
        }

        private static void AdjustRows(decimal[,] augmentedMatrix, int currentEquationIndex, int currentVariableIndex)
        {
            var numberOfRows = augmentedMatrix.GetLength(0);

            for (int row = 0; row < numberOfRows; row++)
            {
                if(row == currentEquationIndex)
                    continue;
                
                AddScaledRow(augmentedMatrix, row, currentEquationIndex, -augmentedMatrix[row, currentVariableIndex]);
            }
        }

        private static int? GetEquationWithNonZeroCoefficient(decimal[,] augmentedMatrix, int currentEquationIndex, int currentVariableIndex)
        {
            var numberOfRows = augmentedMatrix.GetLength(0);

            for (int row = currentEquationIndex; row < numberOfRows; row++)
            {
                if (augmentedMatrix[row, currentVariableIndex] != 0.0M)
                    return row;
            }

            return null;
        }

        private static decimal[,] GetAugmentedMatrix(decimal[,] leftMatrix, decimal[] rightColumn)
        {
            var numberOfRows = leftMatrix.GetLength(0);
            var numberOfColumns = leftMatrix.GetLength(1);
            var augmentedMatrix = new decimal[numberOfRows, numberOfColumns + 1];

            for (int row = 0; row < numberOfRows; row++)
            {
                for (int col = 0; col < numberOfColumns; col++)
                {
                    augmentedMatrix[row, col] = leftMatrix[row, col];
                }

                augmentedMatrix[row, numberOfColumns] = rightColumn[row];
            }

            return augmentedMatrix;
        }

        private static void AddScaledRow(decimal[,] matrix, int targetRow, int sourceRow, decimal scale)
        {
            var numberOfColumns = matrix.GetLength(1);

            for (int col = 0; col < numberOfColumns; col++)
            {
                matrix[targetRow, col] += matrix[sourceRow, col] * scale;
            }
        }

        private static void ScaleRow(decimal[,] matrix, int row, decimal scale)
        {
            var numberOfColumns = matrix.GetLength(1);

            for (int col = 0; col < numberOfColumns; col++)
            {
                matrix[row, col] *= scale;
            }
        }

        private static void SwapTwoRows(decimal[,] matrix, int row1, int row2)
        {
            if(row1 == row2)
                return;

            var numberOfColumns = matrix.GetLength(1);

            for (int col = 0; col < numberOfColumns; col++)
            {
                SwapTwoValues(matrix, row1, col, row2, col);
            }
        }

        private static void SwapTwoValues(decimal[,] matrix, int row1, int col1, int row2, int col2)
        {
            var temp = matrix[row1, col1];
            matrix[row1, col1] = matrix[row2, col2];
            matrix[row2, col2] = temp;
        }
    }

    /// <summary>
    /// Represents result of solving of system of linear equations.
    /// </summary>
    public class SystemOfLinearEquationsResult
    {
        private readonly VariableResult[] _variableResults;

        public SystemOfLinearEquationsResult(SystemOfLinearEquationsResultType type, VariableResult[] variableResults = null)
        {
            Type = type;
            _variableResults = variableResults;
        }

        public SystemOfLinearEquationsResultType Type { get; }

        public VariableResult Variable(int index)
        {
            if(Type == SystemOfLinearEquationsResultType.NoSolution)
                throw new InvalidOperationException("System has no solution");
            return _variableResults[index];
        }
    }

    public class VariableResult
    {
        public VariableResult()
        {
            IsFreeVariable = true;
        }

        public VariableResult(decimal rightValue, Tuple<int, decimal>[] freeVariableParts = null)
        {
            IsFreeVariable = false;
            RightValue = rightValue;
            FreeVariableParts = freeVariableParts ?? new Tuple<int, decimal>[0];
        }

        public bool IsFreeVariable { get; }

        public decimal RightValue { get; }

        public Tuple<int, decimal>[] FreeVariableParts { get; }
    }

    /// <summary>
    /// Types of solutions of system of linear equations.
    /// </summary>
    public enum SystemOfLinearEquationsResultType
    {
        NoSolution,
        OneSolution,
        InfiniteNumberOfSolutions
    }
}