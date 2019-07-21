// Taken and adapted from:
// https://github.com/Humanizr/Humanizer/blob/10e1be770c00ce59c08d5efbe551800400f02c53/src/Humanizer/StringHumanizeExtensions.cs
// https://github.com/Humanizr/Humanizer/blob/10e1be770c00ce59c08d5efbe551800400f02c53/src/Humanizer/RegexOptionsUtil.cs

// We ignore some errors in order to not to have to change the orignal code too much.
#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1202 // Elements should be ordered by access
#pragma warning disable SA1402 // File may only contain a single type

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NSeed.Extensions
{
    /// <summary>
    /// Contains extension methods for humanizing string values.
    /// </summary>
    internal static class StringHumanizeExtensions
    {
        private static readonly Regex PascalCaseWordPartsRegex;
        private static readonly Regex FreestandingSpacingCharRegex;

        static StringHumanizeExtensions()
        {
            PascalCaseWordPartsRegex = new Regex(
                @"[\p{Lu}]?[\p{Ll}]+|[0-9]+[\p{Ll}]*|[\p{Lu}]+(?=[\p{Lu}][\p{Ll}]|[0-9]|\b)|[\p{Lo}]+",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptionsUtil.Compiled);
            FreestandingSpacingCharRegex = new Regex(@"\s[-_]|[-_]\s", RegexOptionsUtil.Compiled);
        }

        private static string FromUnderscoreDashSeparatedWords(string input)
        {
            return string.Join(" ", input.Split(new[] { '_', '-' }));
        }

        private static string FromPascalCase(string input)
        {
            var result = string.Join(" ", PascalCaseWordPartsRegex
                .Matches(input).Cast<Match>()
                .Select(match => match.Value.ToCharArray().All(char.IsUpper) &&
                    (match.Value.Length > 1 || (match.Index > 0 && input[match.Index - 1] == ' ') || match.Value == "I")
                    ? match.Value
                    : match.Value.ToLower()));

            return result.Length > 0 ? char.ToUpper(result[0]) +
                result.Substring(1, result.Length - 1) : result;
        }

        /// <summary>
        /// Humanizes the input string; e.g. Underscored_input_String_is_turned_INTO_sentence -> 'Underscored input String is turned INTO sentence'.
        /// </summary>
        /// <param name="input">The string to be humanized.</param>
        public static string Humanize(this string input)
        {
            System.Diagnostics.Debug.Assert(input != null);

            // if input is all capitals (e.g. an acronym) then return it without change
            if (input.ToCharArray().All(char.IsUpper))
            {
                return input;
            }

            // if input contains a dash or underscore which preceeds or follows a space (or both, e.g. free-standing)
            // remove the dash/underscore and run it through FromPascalCase
            if (FreestandingSpacingCharRegex.IsMatch(input))
            {
                return FromPascalCase(FromUnderscoreDashSeparatedWords(input));
            }

            if (input.Contains("_") || input.Contains("-"))
            {
                return FromUnderscoreDashSeparatedWords(input);
            }

            return FromPascalCase(input);
        }
    }

    internal static class RegexOptionsUtil
    {
        static RegexOptionsUtil()
        {
            Compiled = Enum.TryParse("Compiled", out RegexOptions compiled) ? compiled : RegexOptions.None;
        }

        public static RegexOptions Compiled { get; }
    }
}
