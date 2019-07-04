using System;

namespace NSeed.Extensions
{
    internal static class StringExtensions
    {
        internal static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            System.Diagnostics.Debug.Assert(source != null);
            System.Diagnostics.Debug.Assert(value != null);

            return source.IndexOf(value, comparisonType) >= 0;
        }
    }
}