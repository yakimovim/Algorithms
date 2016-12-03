using EdlinSoftware.Algorithms.LinearProgramming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.LinearProgramming
{
    [TestClass]
    public class SimplexAlgorithmTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void OneVariable_OneLimitation_LimitedSolution()
        {
            var solution = SimplexAlgorithm.Solve(new decimal[,] {{2}}, new decimal[] {10}, new decimal[] {5});
            Assert.AreEqual(LinearProgrammingProblemResultType.LimitedSolution, solution.Type);
            Assert.AreEqual(1, solution.Variables.Length);
            Assert.AreEqual(5M, solution.Variables[0]);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void OneVariable_OneLimitation_NoSolution()
        {
            var solution = SimplexAlgorithm.Solve(new decimal[,] { { 2 } }, new decimal[] { -2 }, new decimal[] { 5 });
            Assert.AreEqual(LinearProgrammingProblemResultType.NoSolution, solution.Type);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void OneVariable_OneLimitation_UnlimitedSolution()
        {
            var solution = SimplexAlgorithm.Solve(new decimal[,] { { -2 } }, new decimal[] { 10 }, new decimal[] { 5 });
            Assert.AreEqual(LinearProgrammingProblemResultType.UnlimitedSolution, solution.Type);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TwoVariables_ThreeLimitations_LimitedSolution()
        {
            var solution = SimplexAlgorithm.Solve(new decimal[,] { { -1, -1 }, { 1, 0 }, { 0, 1 } }, new decimal[] { -1, 2, 2 }, new decimal[] { -1, 2 });
            Assert.AreEqual(LinearProgrammingProblemResultType.LimitedSolution, solution.Type);
            Assert.AreEqual(2, solution.Variables.Length);
            Assert.AreEqual(0M, solution.Variables[0]);
            Assert.AreEqual(2M, solution.Variables[1]);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void TwoVariables_TwoLimitations_NoSolution()
        {
            var solution = SimplexAlgorithm.Solve(new decimal[,] { { 1, 1 }, { -1, -1 } }, new decimal[] { 1, -2 }, new decimal[] { 1, 1 });
            Assert.AreEqual(LinearProgrammingProblemResultType.NoSolution, solution.Type);
        }

        //[TestMethod, Owner("Ivan Yakimov"), Ignore]
        //public void TwoVariables_TwoLimitations()
        //{
        //    var solution = SimplexAlgorithm.Solve(new decimal[,] { { 2, -1 }, { 1, -5 } }, new decimal[] { 2, -4 }, new decimal[] { 2, -1 });
        //    Assert.AreEqual(LinearProgrammingProblemResultType.NoSolution, solution.Type);
        //}

        [TestMethod, Owner("Ivan Yakimov")]
        public void ThreeVariables_OneLimitation_UnlimitedSolution()
        {
            var solution = SimplexAlgorithm.Solve(new decimal[,] { { 0, 0, 1 } }, new decimal[] { 3 }, new decimal[] { 1, 1, 1 });
            Assert.AreEqual(LinearProgrammingProblemResultType.UnlimitedSolution, solution.Type);
        }
    }
}