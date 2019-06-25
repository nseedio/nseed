﻿namespace NSeed.Cli.Extensions
{
    public static class String
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
