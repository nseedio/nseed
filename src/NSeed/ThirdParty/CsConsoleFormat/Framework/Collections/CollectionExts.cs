// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Alba.CsConsoleFormat.Framework.Collections
{
    internal static class CollectionExts
    {
        public static void AddRange<T>([NotNull] this ICollection<T> @this, [NotNull, InstantHandle] IEnumerable<T> items)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            foreach (T item in items)
                @this.Add(item);
        }

        public static void Replace<T>([NotNull] this ICollection<T> @this, [NotNull, InstantHandle] IEnumerable<T> items)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            @this.Clear();
            @this.AddRange(items);
        }
    }
}