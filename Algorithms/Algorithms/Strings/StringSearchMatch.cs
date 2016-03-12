using System.Diagnostics.Contracts;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents result of string search.
    /// </summary>
    public class StringSearchMatch
    {
        /// <summary>
        /// Gets position of start of match.
        /// </summary>
        public int Start { get; }
        /// <summary>
        /// Gets length of match.
        /// </summary>
        public int Length { get; }

        public StringSearchMatch(int start, int length)
        {
            Contract.Requires(start >= 0);
            Contract.Requires(length > 0);

            Contract.Ensures(Start >= 0);
            Contract.Ensures(Length > 0);

            Start = start;
            Length = length;
        }
    }
}