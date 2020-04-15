using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Algorithms
{
    internal static class StringAlgorithms
    {
        private static readonly char[] NamespaceSeparator = new[] { '.' };

        /// <summary>
        /// Gets the length of the deepest common namespace of all the <paramref name="typeNames"/> if such exists, or 0 otherwise.
        /// If not zero, the length includes the final dot (.).
        /// </summary>
        /// <remarks>
        /// The method does not check if the <paramref name="typeNames"/> are valid type names.
        /// It just searches for the largest common starting substring that ends with dot (.).
        /// </remarks>
        public static int GetDeepestCommonNamespaceLength(IReadOnlyList<string> typeNames)
        {
            return GetCommonStartingPartLength(typeNames, NamespaceSeparator);
        }

        /// <summary>
        /// Gets the largest common starting substring of all the <paramref name="strings"/> if such exists, or <see cref="string.Empty"/> otherwise.
        /// The substring is made of parts where each part is separated by any of the <paramref name="partSeparators"/>.
        /// If not empty, the returned string contains the final part separator.
        /// </summary>
        public static string GetCommonStartingPart(IReadOnlyList<string> strings, params char[] partSeparators)
        {
            System.Diagnostics.Debug.Assert(strings.All(@string => @string != null));
            System.Diagnostics.Debug.Assert(partSeparators.Length > 0);

            return strings[0].Substring(0, GetCommonStartingPartLength(strings, partSeparators));
        }

        public static int GetCommonStartingPartLength(IReadOnlyList<string> strings, params char[] partSeparators)
        {
            // We do not want allocations in this method. So, no Linq at all.
            System.Diagnostics.Debug.Assert(strings.All(@string => @string != null));
            System.Diagnostics.Debug.Assert(partSeparators.Length > 0);

            if (strings.Count <= 0) return 0;

            int shortestStringLength = GetShortestStringLength();
            if (shortestStringLength <= 0) return 0;

            int longestCommonSubstringLength = GetLongestCommonSubstringLength();
            if (longestCommonSubstringLength <= 0) return 0;

            return GetLastIndexOfAnyPartSeparatorInLongestCommonSubstring() + 1;

            int GetShortestStringLength()
            {
                int result = strings[0].Length;
                for (int i = 1; i < strings.Count; i++)
                {
                    if (strings[i].Length < result)
                        result = strings[i].Length;
                }

                return result;
            }

            int GetLongestCommonSubstringLength()
            {
                for (int i = 0; i < shortestStringLength; i++)
                {
                    char currentCharacter = strings[0][i];
                    for (int stringIndex = 1; stringIndex < strings.Count; stringIndex++)
                    {
                        if (strings[stringIndex][i] != currentCharacter) return i;
                    }
                }

                return shortestStringLength;
            }

            int GetLastIndexOfAnyPartSeparatorInLongestCommonSubstring()
            {
                var @string = strings[0];

                for (int i = longestCommonSubstringLength - 1; i >= 0; i--)
                {
                    if (partSeparators.Contains(@string[i])) return i;
                }

                return -1;
            }
        }
    }
}
