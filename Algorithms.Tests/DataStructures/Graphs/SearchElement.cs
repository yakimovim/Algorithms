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

        public int Value { get; private set; }

        public double SearchProbability { get; private set; }

        public int CompareTo(SearchElement other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}
