using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using number = System.Decimal;

namespace EdlinSoftware.Algorithms.LinearProgramming
{
    /// <summary>
    /// Represents solver of linear programming problem in standard form.
    /// </summary>
    public class SimplexAlgorithm
    {
        /// <summary>
        /// Represents canonical form of linear programming problem:
        /// z = v + C*Xnb, Xb = B - A*Xnb, where Xb - basis variables, Xnb - non-basis variables.
        /// </summary>
        private class CanonicalForm
        {
            public CanonicalForm()
            {
                NonBasisVariablesIndexes = new HashSet<int>();
                BasisVariablesIndexes = new HashSet<int>();
                A = new Dictionary<Tuple<int, int>, decimal>();
                B = new Dictionary<int, decimal>();
                C = new Dictionary<int, decimal>();
            }

            public HashSet<int> NonBasisVariablesIndexes { get; }
            public HashSet<int> BasisVariablesIndexes { get; }
            public Dictionary<Tuple<int, int>, number> A { get; private set; }
            public Dictionary<int, number> B { get; private set; }
            public Dictionary<int, number> C { get; private set; }
            // ReSharper disable once InconsistentNaming
            public number v { get; set; }
        }

        /// <summary>
        /// Solves linear programming problem in standard form:
        /// maximize C*X when A*X&lt;=B and X &gt;= 0.
        /// </summary>
        public static LinearProgrammingProblemResult Solve(
            [NotNull] number[,] a,
            [NotNull] number[] b,
            [NotNull] number[] c)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (a.GetLength(0) != b.Length) throw new ArgumentException();
            if (a.GetLength(1) != c.Length) throw new ArgumentException();

            var canonicalForm = InitializeSimplex(a, b, c);
            if (canonicalForm == null)
                return new LinearProgrammingProblemResult(LinearProgrammingProblemResultType.NoSolution);

            canonicalForm = SimplexIterations(canonicalForm);
            if (canonicalForm == null)
                return new LinearProgrammingProblemResult(LinearProgrammingProblemResultType.UnlimitedSolution);

            var variables = new number[c.Length];
            for (int i = 0; i < variables.Length; i++)
            {
                if (canonicalForm.BasisVariablesIndexes.Contains(i))
                {
                    variables[i] = canonicalForm.B[i];
                }
                else
                {
                    variables[i] = 0;
                }
            }

            return new LinearProgrammingProblemResult(LinearProgrammingProblemResultType.LimitedSolution, variables);
        }

        /// <summary>
        /// Procedure of replacement of old canonical form with new one.
        /// </summary>
        /// <param name="prevCanonicalForm">Old canonical form.</param>
        /// <param name="l">Index of out variable.</param>
        /// <param name="e">Index of in variable.</param>
        private static CanonicalForm Pivot(CanonicalForm prevCanonicalForm, int l, int e)
        {
            var nextCanonicalForm = new CanonicalForm();

            nextCanonicalForm.B[e] = prevCanonicalForm.B[l] / prevCanonicalForm.A[Tuple.Create(l, e)];

            foreach (var j in prevCanonicalForm.NonBasisVariablesIndexes)
            {
                if (j == e)
                    continue;

                nextCanonicalForm.A[Tuple.Create(e, j)] = prevCanonicalForm.A[Tuple.Create(l, j)] /
                                                          prevCanonicalForm.A[Tuple.Create(l, e)];
            }

            nextCanonicalForm.A[Tuple.Create(e, l)] = 1 / prevCanonicalForm.A[Tuple.Create(l, e)];

            foreach (var i in prevCanonicalForm.BasisVariablesIndexes)
            {
                if (i == l)
                    continue;

                nextCanonicalForm.B[i] = prevCanonicalForm.B[i] -
                                         prevCanonicalForm.A[Tuple.Create(i, e)] * nextCanonicalForm.B[e];
                foreach (var j in prevCanonicalForm.NonBasisVariablesIndexes)
                {
                    if (j == e)
                        continue;

                    nextCanonicalForm.A[Tuple.Create(i, j)] = prevCanonicalForm.A[Tuple.Create(i, j)] -
                                                              prevCanonicalForm.A[Tuple.Create(i, e)] *
                                                              nextCanonicalForm.A[Tuple.Create(e, j)];
                }

                nextCanonicalForm.A[Tuple.Create(i, l)] = -prevCanonicalForm.A[Tuple.Create(i, e)] *
                                                          nextCanonicalForm.A[Tuple.Create(e, l)];
            }

            nextCanonicalForm.v = prevCanonicalForm.v + prevCanonicalForm.C[e] * nextCanonicalForm.B[e];

            foreach (var j in prevCanonicalForm.NonBasisVariablesIndexes)
            {
                if (j == e)
                    continue;

                nextCanonicalForm.C[j] = prevCanonicalForm.C[j] -
                                         prevCanonicalForm.C[e] * nextCanonicalForm.A[Tuple.Create(e, j)];
            }

            nextCanonicalForm.C[l] = -prevCanonicalForm.C[e] * nextCanonicalForm.A[Tuple.Create(e, l)];

            nextCanonicalForm.NonBasisVariablesIndexes.UnionWith(prevCanonicalForm.NonBasisVariablesIndexes);
            nextCanonicalForm.NonBasisVariablesIndexes.Remove(e);
            nextCanonicalForm.NonBasisVariablesIndexes.Add(l);

            nextCanonicalForm.BasisVariablesIndexes.UnionWith(prevCanonicalForm.BasisVariablesIndexes);
            nextCanonicalForm.BasisVariablesIndexes.Remove(l);
            nextCanonicalForm.BasisVariablesIndexes.Add(e);

            return nextCanonicalForm;
        }

        /// <summary>
        /// Represents iterations of simplex algorithm.
        /// </summary>
        /// <param name="initialCanonicalForm">Initial canonical form.</param>
        /// <returns>Canonical form of answer or null, if solutions is unbounded (infinite).</returns>
        private static CanonicalForm SimplexIterations(CanonicalForm initialCanonicalForm)
        {
            CanonicalForm canonicalForm = initialCanonicalForm;

            while (canonicalForm.NonBasisVariablesIndexes.Any(i => canonicalForm.C[i] > 0))
            {
                var e = ChooseNonBasisVariableIndex(canonicalForm);

                var delta = new Dictionary<int, number>();
                foreach (var i in canonicalForm.BasisVariablesIndexes)
                {
                    if (canonicalForm.A[Tuple.Create(i, e)] > 0)
                        delta[i] = canonicalForm.B[i] / canonicalForm.A[Tuple.Create(i, e)];
                }

                var l = ChooseBasisVariableIndex(canonicalForm, delta);
                if (!delta.ContainsKey(l))
                    return null;
                canonicalForm = Pivot(canonicalForm, l, e);
            }

            return canonicalForm;
        }

        /// <summary>
        /// Chooses non-basis variable index using Bland's rule.
        /// </summary>
        /// <param name="canonicalForm">Caninical form.</param>
        private static int ChooseNonBasisVariableIndex(CanonicalForm canonicalForm)
        {
            bool isChoosen = false;
            int choosenIndex = 0;

            foreach (var e in canonicalForm.NonBasisVariablesIndexes)
            {
                if (canonicalForm.C[e] <= 0)
                    continue;

                if (!isChoosen || e < choosenIndex)
                {
                    choosenIndex = e;
                    isChoosen = true;
                }
            }

            return choosenIndex;
        }

        /// <summary>
        /// Chooses basis variable index using Bland's rule.
        /// </summary>
        /// <param name="canonicalForm">Caninical form.</param>
        /// <param name="delta">Delta values.</param>
        private static int ChooseBasisVariableIndex(CanonicalForm canonicalForm, Dictionary<int, decimal> delta)
        {
            if (delta.Count == 0)
                return canonicalForm.BasisVariablesIndexes.First();

            number? minDelta = null; // null represents infinity.
            int choosenIndex = 0;

            foreach (var l in delta.Keys)
            {
                if (!minDelta.HasValue || minDelta.Value > delta[l])
                {
                    minDelta = delta[l];
                    choosenIndex = l;
                }
                else if (minDelta.Value == delta[l] && l < choosenIndex)
                {
                    choosenIndex = l;
                }
            }

            return choosenIndex;
        }

        /// <summary>
        /// Creates canonical form for standard from.
        /// </summary>
        /// <returns>Canonical form for given standard from. Null, if problem has no solution.</returns>
        private static CanonicalForm InitializeSimplex(number[,] a,
            number[] b,
            number[] c)
        {
            var k = GetIndexMinimumOfMinimumB(b);

            if (b[k] >= 0)
            {
                var canonicalForm = new CanonicalForm();
                canonicalForm.NonBasisVariablesIndexes.UnionWith(Enumerable.Range(0, c.Length));
                canonicalForm.BasisVariablesIndexes.UnionWith(Enumerable.Range(c.Length, b.Length));
                canonicalForm.v = 0;
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    for (int j = 0; j < a.GetLength(1); j++)
                    {
                        canonicalForm.A[Tuple.Create(i + c.Length, j)] = a[i, j];
                    }
                }
                for (int i = 0; i < b.Length; i++)
                    canonicalForm.B[i + c.Length] = b[i];
                for (int j = 0; j < c.Length; j++)
                    canonicalForm.C[j] = c[j];
                return canonicalForm;
            }

            var auxCanonicalForm = new CanonicalForm();
            auxCanonicalForm.NonBasisVariablesIndexes.UnionWith(Enumerable.Range(0, c.Length + 1));
            auxCanonicalForm.BasisVariablesIndexes.UnionWith(Enumerable.Range(c.Length + 1, b.Length));
            auxCanonicalForm.v = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    auxCanonicalForm.A[Tuple.Create(i + c.Length + 1, j + 1)] = a[i, j];
                }
            }
            for (int i = 0; i < a.GetLength(0); i++)
            {
                auxCanonicalForm.A[Tuple.Create(i + c.Length + 1, 0)] = -1;
            }
            for (int i = 0; i < b.Length; i++)
                auxCanonicalForm.B[i + c.Length + 1] = b[i];
            for (int j = 0; j < c.Length; j++)
                auxCanonicalForm.C[j + 1] = 0;
            auxCanonicalForm.C[0] = -1;

            var l = c.Length + 1 + k;

            auxCanonicalForm = Pivot(auxCanonicalForm, l, 0);
            auxCanonicalForm = SimplexIterations(auxCanonicalForm);

            var x0 = auxCanonicalForm.BasisVariablesIndexes.Contains(0) ? auxCanonicalForm.B[0] : 0;

            if (x0 != 0)
                return null;

            var outCanonicalForm = new CanonicalForm();
            outCanonicalForm.BasisVariablesIndexes.UnionWith(auxCanonicalForm.BasisVariablesIndexes.Where(i => i != 0).Select(i => i - 1));
            outCanonicalForm.NonBasisVariablesIndexes.UnionWith(auxCanonicalForm.NonBasisVariablesIndexes.Where(i => i != 0).Select(i => i - 1));

            foreach (var key in auxCanonicalForm.A.Keys)
            {
                if (key.Item1 == 0 || key.Item2 == 0)
                    continue;

                outCanonicalForm.A[Tuple.Create(key.Item1 - 1, key.Item2 - 1)] = auxCanonicalForm.A[key];
            }

            foreach (var key in auxCanonicalForm.B.Keys)
            {
                if (key == 0)
                    continue;

                outCanonicalForm.B[key - 1] = auxCanonicalForm.B[key];
            }

            foreach (var i in auxCanonicalForm.NonBasisVariablesIndexes)
            {
                if (i == 0)
                    continue;

                outCanonicalForm.C[i - 1] = i <= c.Length ? c[i - 1] : 0;
            }

            foreach (var i in auxCanonicalForm.BasisVariablesIndexes)
            {
                if (i == 0)
                    continue;

                if (i <= c.Length)
                {
                    outCanonicalForm.v += c[i - 1] * auxCanonicalForm.B[i];

                    var keys = auxCanonicalForm.A.Keys.Where(key => key.Item1 == i && key.Item2 != 0).ToArray();
                    foreach (var key in keys)
                    {
                        outCanonicalForm.C[key.Item2 - 1] -= c[i - 1]*auxCanonicalForm.A[Tuple.Create(i, key.Item2)];
                    }
                }
            }

            return outCanonicalForm;
        }

        private static int GetIndexMinimumOfMinimumB(number[] b)
        {
            var index = 0;
            var value = b[0];

            for (int i = 1; i < b.Length; i++)
            {
                if (b[i] < value)
                {
                    value = b[i];
                    index = i;
                }
            }

            return index;
        }
    }

    /// <summary>
    /// Represents result of solving of linear programming problem.
    /// </summary>
    public sealed class LinearProgrammingProblemResult
    {
        private readonly number[] _variables;

        public LinearProgrammingProblemResult(LinearProgrammingProblemResultType type, number[] variables = null)
        {
            Type = type;
            _variables = variables;
        }

        public LinearProgrammingProblemResultType Type { get; }

        public number[] Variables
        {
            get
            {
                if (Type != LinearProgrammingProblemResultType.LimitedSolution)
                    throw new InvalidOperationException("Problem has no limited solution");
                return _variables;
            }
        }
    }

    /// <summary>
    /// Types of solutions of linear programming problem.
    /// </summary>
    public enum LinearProgrammingProblemResultType
    {
        NoSolution,
        LimitedSolution,
        UnlimitedSolution
    }
}