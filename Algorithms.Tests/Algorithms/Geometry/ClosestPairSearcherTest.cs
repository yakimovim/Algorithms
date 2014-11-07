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
        public void Search_ShouldReturnCorrectPairOfPoints()
        {
            var arrayOfPoints = GetArrayOfPoints(new float[,] { { 1, 5 }, { 6, 4 }, { 8, 0 }, { 7, 11 }, { 9, 7 }, { 15, 8 } });

            var pair = _searcher.Search(arrayOfPoints);

            Assert.IsNotNull(pair);

            Assert.AreEqual(new PairOfPoints { P = arrayOfPoints[1], Q = arrayOfPoints[4] }, pair);
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
