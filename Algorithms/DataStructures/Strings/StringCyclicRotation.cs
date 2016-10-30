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
}