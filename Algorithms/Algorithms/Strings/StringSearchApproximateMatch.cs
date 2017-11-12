using System.Collections.Generic;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents result of approximate string search.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public class StringSearchApproximateMatch<TSymbol> : StringSearchMatch
    {
        /// <summary>
        /// Represents matched pattern.
        /// </summary>
        public IReadOnlyList<TSymbol> Pattern { get; }

        public StringSearchApproximateMatch(int start, IReadOnlyList<TSymbol> pattern) 
            : base(start, pattern.Count)
        {
            Pattern = pattern;
        }
    }
}