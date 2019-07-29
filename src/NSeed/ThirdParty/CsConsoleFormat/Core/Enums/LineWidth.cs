// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;

namespace Alba.CsConsoleFormat
{
    internal enum LineWidth
    {
        None,
        Single,
        Heavy,
        Double,
    }

    internal static class LineWidthExts
    {
        public static int ToCharWidth(this LineWidth @this) => @this == LineWidth.None ? 0 : 1;

        [Pure]
        public static LineWidth Max(LineWidth left, LineWidth right) => (LineWidth)Math.Max((int)left, (int)right);

        [Pure]
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "API consistency.")]
        public static LineWidth Max([NotNull, InstantHandle] IEnumerable<LineWidth> items) => items.Aggregate(LineWidth.None, Max);
    }
}