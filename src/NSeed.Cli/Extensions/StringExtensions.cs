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

        public static string SplitAndTakeFirst(this string item, params char[] separator)
        {
            var splitItems = item.Split(separator).ToList();
            if (splitItems.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return splitItems.FirstOrDefault();
        }
    }
}
