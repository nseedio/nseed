using System;
using static System.Diagnostics.Debug;

namespace NSeed.Seeding.Extensions
{
    internal static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison stringComparison)
        {
            Assert(source != null);

            return source.IndexOf(value, stringComparison) >= 0;
        }
    }
}
