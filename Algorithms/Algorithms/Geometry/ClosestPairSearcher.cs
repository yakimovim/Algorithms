﻿using System;
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

        private class PointComparerX : IComparer<PointF>
        {
            public int Compare(PointF x, PointF y)
            {
                return x.X.CompareTo(y.X);
            }
        }

        private class PointComparerY : IComparer<PointF>
        {
            public int Compare(PointF x, PointF y)
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

            var pX = _sorter.Sort(points, new PointComparerX());
            var pY = _sorter.Sort(points, new PointComparerY());

            return GetClosestPair(pX, pY);
        }

        private PairOfPoints GetClosestPair(PointF[] pX, PointF[] pY)
        {
            if (pX.Length < 2)
            {
                return null;
            }
            if (pX.Length == 2)
            {
                return new PairOfPoints { P = pX[0], Q = pX[1] };
            }

            PointF[] lX, lY, rX, rY;
            SeparateArrays(pX, pY, out lX, out lY, out rX, out rY);

            var leftPair = GetClosestPair(lX, lY);
            var rightPair = GetClosestPair(rX, rY);

            double minDistanceAtSides = GetMinDistance(leftPair, rightPair);

            var splitPair = GetClosestSplitPair(pX, pY, minDistanceAtSides);

            return GetClosestPair(leftPair, rightPair, splitPair);
        }

        private PairOfPoints GetClosestSplitPair(PointF[] pX, PointF[] pY, double minDistanceAtSides)
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
                for (int j = i + 1; j <= 7; j++)
                {
                    if (j >= sY.Length)
                    { break; }

                    var newPair = new PairOfPoints { P = sY[i], Q = sY[j] };
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

        private void SeparateArrays(PointF[] pX, PointF[] pY, out PointF[] lX, out PointF[] lY, out PointF[] rX, out PointF[] rY)
        {
            var middleIndex = pX.Length / 2;
            var middleX = pX[middleIndex].X;

            lX = new PointF[middleIndex + 1];
            lY = new PointF[middleIndex + 1];
            rX = new PointF[pX.Length - lX.Length];
            rY = new PointF[pY.Length - lY.Length];

            int ilX, ilY, irX, irY;
            ilX = ilY = irX = irY = 0;

            for (int k = 0; k < pX.Length; k++)
            {
                if (pX[k].X <= middleX)
                {
                    lX[ilX++] = pX[k];
                }
                else
                {
                    rX[irX++] = pX[k];
                }

                if (pY[k].X <= middleX)
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
