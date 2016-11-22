using System;
using EdlinSoftware.Algorithms.LinearProgramming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.LinearProgramming
{
    [TestClass]
    public class GaussianEliminationTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void OneVariable_OneEquation()
        {
            var result = GaussianElimination.Solve(new[,] {{2.0M}}, new[] {4.0M});

            Assert.IsNotNull(result);
            Assert.AreEqual(SystemOfLinearEquationsResultType.OneSolution, result.Type);

            var variable0 = result.Variable(0);
            Assert.IsNotNull(variable0);
            Assert.IsFalse(variable0.IsFreeVariable);
            Assert.AreEqual(2.0M, variable0.RightValue);
            Assert.AreEqual(0, variable0.FreeVariableParts.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void OneVariable_TwoEquation_NoSolution()
        {
            var result = GaussianElimination.Solve(new[,] { { 2.0M }, { 1.0M } }, new[] { 4.0M, 3.0M });

            Assert.IsNotNull(result);
            Assert.AreEqual(SystemOfLinearEquationsResultType.NoSolution, result.Type);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void OneVariable_TwoEquation_LinearCombination_OneSolution()
        {
            var result = GaussianElimination.Solve(new[,] { { 2.0M }, { 1.0M } }, new[] { 4.0M, 2.0M });

            Assert.IsNotNull(result);
            Assert.AreEqual(SystemOfLinearEquationsResultType.OneSolution, result.Type);

            var variable0 = result.Variable(0);
            Assert.IsNotNull(variable0);
            Assert.IsFalse(variable0.IsFreeVariable);
            Assert.AreEqual(2.0M, variable0.RightValue);
            Assert.AreEqual(0, variable0.FreeVariableParts.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TwoVariables_TwoEquation_OneSolution()
        {
            var result = GaussianElimination.Solve(new[,] { { 1.0M, 1.0M }, { 1.0M, -1.0M } }, new[] { 8.0M, 6.0M });

            Assert.IsNotNull(result);
            Assert.AreEqual(SystemOfLinearEquationsResultType.OneSolution, result.Type);

            var variable0 = result.Variable(0);
            Assert.IsNotNull(variable0);
            Assert.IsFalse(variable0.IsFreeVariable);
            Assert.AreEqual(7.0M, variable0.RightValue);
            Assert.AreEqual(0, variable0.FreeVariableParts.Length);

            var variable1 = result.Variable(1);
            Assert.IsNotNull(variable1);
            Assert.IsFalse(variable1.IsFreeVariable);
            Assert.AreEqual(1.0M, variable1.RightValue);
            Assert.AreEqual(0, variable1.FreeVariableParts.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TwoVariables_TwoEquation_LinearCombination_InfiniteSolutions()
        {
            var result = GaussianElimination.Solve(new[,] { { 1.0M, 1.0M }, { 2.0M, 2.0M } }, new[] { 8.0M, 16.0M });

            Assert.IsNotNull(result);
            Assert.AreEqual(SystemOfLinearEquationsResultType.InfiniteNumberOfSolutions, result.Type);

            var variable0 = result.Variable(0);
            Assert.IsNotNull(variable0);
            Assert.IsFalse(variable0.IsFreeVariable);
            Assert.AreEqual(8.0M, variable0.RightValue);
            Assert.AreEqual(1, variable0.FreeVariableParts.Length);
            Assert.AreEqual(Tuple.Create(1, -1.0M), variable0.FreeVariableParts[0]);

            var variable1 = result.Variable(1);
            Assert.IsNotNull(variable1);
            Assert.IsTrue(variable1.IsFreeVariable);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TwoVariables_OneEquation_InfiniteSolutions()
        {
            var result = GaussianElimination.Solve(new[,] { { 2.0M, 6.0M } }, new[] { 8.0M });

            Assert.IsNotNull(result);
            Assert.AreEqual(SystemOfLinearEquationsResultType.InfiniteNumberOfSolutions, result.Type);

            var variable0 = result.Variable(0);
            Assert.IsNotNull(variable0);
            Assert.IsFalse(variable0.IsFreeVariable);
            Assert.AreEqual(4.0M, variable0.RightValue);
            Assert.AreEqual(1, variable0.FreeVariableParts.Length);
            Assert.AreEqual(Tuple.Create(1, -3.0M), variable0.FreeVariableParts[0]);

            var variable1 = result.Variable(1);
            Assert.IsNotNull(variable1);
            Assert.IsTrue(variable1.IsFreeVariable);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TwoVariables_TwoEquation_NoSolution()
        {
            var result = GaussianElimination.Solve(new[,] { { 1.0M, 1.0M }, { 2.0M, 2.0M } }, new[] { 8.0M, 18.0M });

            Assert.IsNotNull(result);
            Assert.AreEqual(SystemOfLinearEquationsResultType.NoSolution, result.Type);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TwoVariables_ThreeEquations_NoSolution()
        {
            var result = GaussianElimination.Solve(new[,] { { 1.0M, 1.0M }, { 1.0M, -1.0M }, { 2.0M, 3.0M } }, new[] { 8.0M, 6.0M, 18.0M });

            Assert.IsNotNull(result);
            Assert.AreEqual(SystemOfLinearEquationsResultType.NoSolution, result.Type);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TwoVariables_ThreeEquations_LinearCombination_OneSolution()
        {
            var result = GaussianElimination.Solve(new[,] { { 1.0M, 1.0M }, { 1.0M, -1.0M }, { 2.0M, 3.0M } }, new[] { 8.0M, 6.0M, 17.0M });

            Assert.IsNotNull(result);
            Assert.AreEqual(SystemOfLinearEquationsResultType.OneSolution, result.Type);

            var variable0 = result.Variable(0);
            Assert.IsNotNull(variable0);
            Assert.IsFalse(variable0.IsFreeVariable);
            Assert.AreEqual(7.0M, variable0.RightValue);
            Assert.AreEqual(0, variable0.FreeVariableParts.Length);

            var variable1 = result.Variable(1);
            Assert.IsNotNull(variable1);
            Assert.IsFalse(variable1.IsFreeVariable);
            Assert.AreEqual(1.0M, variable1.RightValue);
            Assert.AreEqual(0, variable1.FreeVariableParts.Length);
        }
    }
}