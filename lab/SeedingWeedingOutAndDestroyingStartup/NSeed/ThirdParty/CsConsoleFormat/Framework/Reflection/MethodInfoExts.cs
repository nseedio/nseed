// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Alba.CsConsoleFormat.Framework.Reflection
{
    internal static class MethodInfoExts
    {
        public static bool IsVoid([NotNull] this MethodInfo @this) =>
            (@this ?? throw new ArgumentNullException(nameof(@this))).ReturnType == typeof(void);
    }
}