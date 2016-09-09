using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EdlinSoftware.Algorithms.Collections.Sorting;
using EdlinSoftware.DataStructures.Geometry;

namespace EdlinSoftware.Algorithms.Geometry
{
    /// <summary>
    /// Represents class returning from a list of 2D points a pair of closest points.
    /// </summary>
    public class ClosestPairSearcher
    {
        private readonly MergeSorter _sorter = new MergeSorter();

        private class PointContainer
        {
            public PointContainer(PointF point)
            {
                Point = point;
            }

            public PointF Point { get; }

            public float X => Point.X;
            public float Y => Point.Y;

            public bool InLeftPart { get; set; }

            public override string ToString()
            {
                return $"{X}:{Y}";
            }
        }

        private class PointComparerX : IComparer<PointContainer>
        {
            public int Compare(PointContainer x, PointContainer y)
            {
                return x.X.CompareTo(y.X);
            }
        }

        private class PointComparerY : IComparer<PointContainer>
        {
            public int Compare(PointContainer x, PointContainer y)
            {
                return x.Y.CompareTo(y.Y);
            }
        }

        /// <summary>
        /// Returns a pair of closest points from the list of 2D points.
        /// </summary>
        /// <param name="points">List of 2D points.</param>
        /// <returns>A pair of closest points.</returns>
        public PairOfPoints Search(PointF[] points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            if (points.Length < 2) throw new ArgumentException("There must be at least two points.");

            var pointContainers = points.Select(p => new PointContainer(p)).ToArray();
            var pX = _sorter.Sort(pointContainers, new PointComparerX());
            var pY = _sorter.Sort(pointContainers, new PointComparerY());

            return GetClosestPair(pX, pY);
        }

        private PairOfPoints GetClosestPair(PointContainer[] pX, PointContainer[] pY)
        {
            if (pX.Length < 2)
            {
                return null;
            }
            if (pX.Length == 2)
            {
                return new PairOfPoints { P = pX[0].Point, Q = pX[1].Point };
            }

            PointContainer[] lX, lY, rX, rY;
            SeparateArrays(pX, pY, out lX, out lY, out rX, out rY);

            var leftPair = GetClosestPair(lX, lY);
            var rightPair = GetClosestPair(rX, rY);

            double minDistanceAtSides = GetMinDistance(leftPair, rightPair);

            var splitPair = GetClosestSplitPair(pX, pY, minDistanceAtSides);

            return GetClosestPair(leftPair, rightPair, splitPair);
        }

        private PairOfPoints GetClosestSplitPair(PointContainer[] pX, PointContainer[] pY, double minDistanceAtSides)
        {
            var middleIndex = pX.Length / 2;
            var middleX = pX[middleIndex].X;

            var sY = pY
                .Where(point => point.X >= middleX - minDistanceAtSides && point.X <= middleX + minDistanceAtSides)
                .ToArray();

            PairOfPoints closestPair = null;
            double minDistance = minDistanceAtSides;

            for (int i = 0; i < sY.Length - 1; i++)
            {
                for (int j = i + 1; j <= i + 7; j++)
                {
                    if (j >= sY.Length)
                    { break; }

                    if (sY[j].Y - sY[i].Y > minDistance)
                    { break; }

                    var newPair = new PairOfPoints { P = sY[i].Point, Q = sY[j].Point };
                    var newDistance = GetDistance(newPair);
                    if (newDistance < minDistance)
                    {
                        minDistance = newDistance;
                        closestPair = newPair;
                    }
                }
            }

            return closestPair;
        }

        private void SeparateArrays(PointContainer[] pX, PointContainer[] pY, out PointContainer[] lX, out PointContainer[] lY, out PointContainer[] rX, out PointContainer[] rY)
        {
            var middleIndex = pX.Length / 2;
            var middleX = pX[middleIndex].X;

            lX = new PointContainer[middleIndex + 1];
            lY = new PointContainer[middleIndex + 1];
            rX = new PointContainer[pX.Length - lX.Length];
            rY = new PointContainer[pY.Length - lY.Length];

            int ilX, ilY, irX, irY;
            ilX = ilY = irX = irY = 0;

            for (int k = 0; k < pX.Length; k++)
            {
                if (pX[k].X <= middleX && ilX < lX.Length)
                {
                    lX[ilX++] = pX[k];
                    pX[k].InLeftPart = true;
                }
                else
                {
                    rX[irX++] = pX[k];
                    pX[k].InLeftPart = false;
                }
            }

            for (int k = 0; k < pX.Length; k++)
            {
                if (pY[k].InLeftPart)
                {
                    lY[ilY++] = pY[k];
                }
                else
                {
                    rY[irY++] = pY[k];
                }
            }
        }

        private PairOfPoints GetClosestPair(PairOfPoints leftPair, PairOfPoints rightPair, PairOfPoints splitPair)
        {
            var distance1 = GetDistance(leftPair);
            var distance2 = GetDistance(rightPair);
            var distance3 = GetDistance(splitPair);

            if (distance1 <= distance2 && distance1 <= distance3)
                return leftPair;
            if (distance2 <= distance1 && distance2 <= distance3)
                return rightPair;

            return splitPair;
        }

        private double GetMinDistance(PairOfPoints leftPair, PairOfPoints rightPair)
        {
            var distance1 = GetDistance(leftPair);
            var distance2 = GetDistance(rightPair);

            return Math.Min(distance1, distance2);
        }

        private double GetDistance(PairOfPoints pair)
        {
            if (pair == null)
                return double.MaxValue;

            return Math.Sqrt((pair.P.X - pair.Q.X) * (pair.P.X - pair.Q.X) + (pair.P.Y - pair.Q.Y) * (pair.P.Y - pair.Q.Y));
        }
    }
}
