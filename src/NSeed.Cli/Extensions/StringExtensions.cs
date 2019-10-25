using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Cli.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNotProvidedByUser(this string item)
        {
            return string.IsNullOrEmpty(item);
        }

        public static bool Exists(this string item)
        {
            return !string.IsNullOrEmpty(item);
        }

        public static string TakeFirstOrEmpty(this string item, params char[] separators)
        {
            var index = item.LastIndexOfAny(separators);
            if (index >= 0)
            {
                var result = item.Substring(0, index);
                return result;
            }

            return item;
        }

        public static bool Contain(this IEnumerable<string> items, string item)
        {
            items ??= Enumerable.Empty<string>();
            return items.Any(d => d.Equals(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}
