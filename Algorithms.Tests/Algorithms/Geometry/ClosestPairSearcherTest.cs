using System;
using System.Drawing;
using EdlinSoftware.Algorithms.Geometry;
using EdlinSoftware.DataStructures.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Geometry
{
    [TestClass]
    public class ClosestPairSearcherTest
    {
        private ClosestPairSearcher _searcher;

        [TestInitialize]
        public void TestInitialize()
        {
            _searcher = new ClosestPairSearcher();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfArrayOfPointsIsNull()
        {
            _searcher.Search(null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentException))]
        public void Search_ShouldThrowException_IfArrayOfPointsHasNoValues()
        {
            _searcher.Search(GetArrayOfPoints(new float[0, 2]));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentException))]
        public void Search_ShouldThrowException_IfArrayOfPointsHasOnePoint()
        {
            _searcher.Search(GetArrayOfPoints(new float[,] { { 0, 0 } }));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnFirstTwoPoints_IfArrayOfPointsHasTwoPoints()
        {
            var arrayOfPoints = GetArrayOfPoints(new float[,] { { 0, 0 }, { 1, 1 } });

            var pair = _searcher.Search(arrayOfPoints);

            Assert.IsNotNull(pair);

            Assert.AreEqual(new PairOfPoints { P = arrayOfPoints[0], Q = arrayOfPoints[1] }, pair);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnFirstTwoPoints_IfArrayOfPointsHasSamePoints()
        {
            var arrayOfPoints = GetArrayOfPoints(new float[,] { { 7, 7 }, { 1, 100 }, { 3, 4 }, { 7, 7 } });

            var pair = _searcher.Search(arrayOfPoints);

            Assert.IsNotNull(pair);

            Assert.AreEqual(new PairOfPoints { P = arrayOfPoints[0], Q = arrayOfPoints[3] }, pair);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnCorrectPairOfPoints()
        {
            var arrayOfPoints = GetArrayOfPoints(new float[,] { { 1, 5 }, { 6, 4 }, { 8, 0 }, { 7, 11 }, { 9, 7 }, { 15, 8 } });

            var pair = _searcher.Search(arrayOfPoints);

            Assert.IsNotNull(pair);

            Assert.AreEqual(new PairOfPoints { P = arrayOfPoints[1], Q = arrayOfPoints[4] }, pair);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnCorrectPairOfPoints_ForLongListsOfPoints()
        {
            var arrayOfPoints = GetArrayOfPoints(new float[,] { { 7, 8 }, { 4, -4 }, { 1, -8 }, { 1, -2 }, { 3, -6 }, { 0, -5 }, { 3, 8 }, { 2, 1 }, { 2, 6 }, { 2, 9} });

            var pair = _searcher.Search(arrayOfPoints);

            Assert.IsNotNull(pair);

            Assert.AreEqual(new PairOfPoints { P = arrayOfPoints[6], Q = arrayOfPoints[9] }, pair);
        }

        private PointF[] GetArrayOfPoints(float[,] arrayOfCoordinates)
        {
            var points = new PointF[arrayOfCoordinates.GetLength(0)];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new PointF
                {
                    X = arrayOfCoordinates[i, 0],
                    Y = arrayOfCoordinates[i, 1]
                };
            }

            return points;
        }
    }
}
