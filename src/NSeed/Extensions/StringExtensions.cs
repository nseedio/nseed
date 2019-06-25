using System;
using NSeed.Guards;
using Light.GuardClauses;

namespace NSeed.Extensions
{
    internal static class StringExtensions
    {
        internal static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            source.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(source)));
            value.MustNotBeNull(() => new NSeedInternalErrorArgumentNullException(nameof(value)));

            return source.IndexOf(value, comparisonType) >= 0;
        }
    }
}