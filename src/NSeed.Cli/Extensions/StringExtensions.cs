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
    }
}
