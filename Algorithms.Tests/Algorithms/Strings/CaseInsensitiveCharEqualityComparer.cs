using System.Collections.Generic;

namespace EdlinSoftware.Tests.Algorithms.Strings
{
    public class CaseInsensitiveCharEqualityComparer : IEqualityComparer<char>
    {
        private readonly IEqualityComparer<char> _defaultComparer = EqualityComparer<char>.Default;

        public bool Equals(char x, char y)
        {
            return _defaultComparer.Equals(char.ToLower(x), char.ToLower(y));
        }

        public int GetHashCode(char obj)
        {
            return _defaultComparer.GetHashCode(char.ToLower(obj));
        }
    }
}