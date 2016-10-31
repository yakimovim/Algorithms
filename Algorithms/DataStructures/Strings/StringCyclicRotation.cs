using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Strings
{
    /// <summary>
    /// Represents cyclic rotation of string.
    /// </summary>
    public class StringCyclicRotation<TSymbol>
    {
        private readonly IReadOnlyList<TSymbol> _text;
        private readonly int _offset;

        public int Length => _text.Count;

        public StringCyclicRotation([NotNull] IReadOnlyList<TSymbol> text, int offset)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if(text.Count == 0) throw new ArgumentException("Text should not be empty", nameof(text));
            _text = text;
            _offset = offset;
            while (_offset < 0)
            {
                _offset += _text.Count;
            }
        }

        private TSymbol GetSymbolAt(int index)
        {
            if(index < 0 || index >= _text.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _text[(index + _offset)%_text.Count];
        }

        public TSymbol this[int index] => GetSymbolAt(index);

        public override string ToString()
        {
            return string.Join("",
                Enumerable.Range(0, _text.Count)
                    .Select(GetSymbolAt)
                    .Select(s => s.ToString())
                    .ToArray());
        }
    }

    public class StringCyclicRotationComparer<TSymbol> : IComparer<StringCyclicRotation<TSymbol>>
    {
        private readonly IComparer<TSymbol> _symbolComparer;

        public StringCyclicRotationComparer(IComparer<TSymbol> symbolComparer = null)
        {
            _symbolComparer = symbolComparer ?? Comparer<TSymbol>.Default;
        }

        public int Compare(StringCyclicRotation<TSymbol> x, StringCyclicRotation<TSymbol> y)
        {
            var minLength = Math.Min(x.Length, y.Length);

            for (int i = 0; i < minLength; i++)
            {
                var symbolComparisonResult = _symbolComparer.Compare(x[i], y[i]);
                if (symbolComparisonResult != 0)
                    return symbolComparisonResult;
            }

            if (x.Length < y.Length)
                return -1;

            if (x.Length > y.Length)
                return 1;

            return 0;
        }
    }
}