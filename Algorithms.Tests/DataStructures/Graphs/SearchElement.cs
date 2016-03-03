using System;

namespace EdlinSoftware.Tests.DataStructures.Graphs
{
    internal class SearchElement : IComparable<SearchElement>
    {
        public SearchElement(int value, double searchProbability)
        {
            Value = value;
            SearchProbability = searchProbability;
        }

        public int Value { get; }

        public double SearchProbability { get; }

        public int CompareTo(SearchElement other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}
