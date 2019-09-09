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
            var splitItems = item.Split(separators)?.ToList() ?? new List<string>();
            return splitItems.FirstOrDefault() ?? string.Empty;
        }
    }
}
