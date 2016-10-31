using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Strings
{
    public class StopSymbolFirstComparer<TSymbol> : IComparer<TSymbol>
    {
        private readonly IComparer<TSymbol> _comparer;
        private readonly TSymbol _stopSymbol;

        public StopSymbolFirstComparer([NotNull] IComparer<TSymbol> comparer, TSymbol stopSymbol)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            _comparer = comparer;
            _stopSymbol = stopSymbol;
        }

        public int Compare(TSymbol x, TSymbol y)
        {
            if (IsStopSymbol(x) && IsStopSymbol(y))
                return 0;
            if (IsStopSymbol(x))
                return -1;
            if (IsStopSymbol(y))
                return 1;
            return _comparer.Compare(x, y);
        }

        private bool IsStopSymbol(TSymbol symbol)
        {
            if (_stopSymbol == null)
                return symbol == null;
            return _stopSymbol.Equals(symbol);
        }
    }
}