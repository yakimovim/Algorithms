using System.Drawing;

namespace EdlinSoftware.DataStructures.Geometry
{
    public class PairOfPoints
    {
        public PointF P { get; set; }
        public PointF Q { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as PairOfPoints;
            if (other == null)
            {
                return false;
            }

            return (P.Equals(other.P) && Q.Equals(other.Q)) || (P.Equals(other.Q) && Q.Equals(other.P));
        }

        public override int GetHashCode()
        {
            return P.GetHashCode() * 37 + Q.GetHashCode();
        }

        public override string ToString()
        {
            return $"{P},{Q}";
        }
    }
}
