using System.Collections.Generic;

namespace EdlinSoftware.Algorithms.Strings
{
    public static class TextExtensions
    {
        /// <summary>
        /// Ensures that <paramref name="text"/> ends with <paramref name="finalSymbol"/>
        /// </summary>
        /// <typeparam name="TSymbol">Type of symbols.</typeparam>
        /// <param name="text">Text.</param>
        /// <param name="finalSymbol">Final symbol.</param>
        public static IReadOnlyList<TSymbol> FinishWith<TSymbol>(this IReadOnlyList<TSymbol> text, TSymbol finalSymbol)
        {
            if (text.Count == 0)
                return new List<TSymbol> { finalSymbol };

            return text[text.Count - 1].Equals(finalSymbol) ? text : new List<TSymbol>(text) { finalSymbol };
        }
    }
}