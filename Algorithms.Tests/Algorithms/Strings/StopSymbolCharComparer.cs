using System.Collections.Generic;
using EdlinSoftware.DataStructures.Strings;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    public class StopSymbolCharComparer : StopSymbolFirstComparer<char>
    {
        public static readonly StopSymbolCharComparer Instance = new StopSymbolCharComparer();

        private StopSymbolCharComparer() : base(Comparer<char>.Default, '$')
        {}
    }
}