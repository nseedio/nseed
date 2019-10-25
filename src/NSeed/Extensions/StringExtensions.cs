using System;

namespace NSeed.Extensions
{
    internal static class StringExtensions
    {
        internal static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source.IndexOf(value, comparisonType) >= 0;
        }
    }
}
