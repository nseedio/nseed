// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

using System;
using System.ComponentModel;
using System.Reflection;
using static Alba.CsConsoleFormat.TypeConverterUtils;

namespace Alba.CsConsoleFormat
{
    /// <summary>
    /// Converts <see cref="Point"/> to and from <see cref="string"/>:
    /// <list type="bullet">
    /// <item>"1 2" - <c>new Point(1, 2)</c></item>
    /// </list> 
    /// Separator can be " " or ",".
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal sealed class PointConverter : SequenceTypeConverter<Point>
    {
        private static readonly Lazy<ConstructorInfo> PointConstructor = new Lazy<ConstructorInfo>(() =>
            typeof(Point).GetConstructor(new[] { typeof(int), typeof(int) }));

        protected override Point FromString(string str)
        {
            string[] parts = SplitNumbers(str, 2);
            if (parts.Length != 2)
                throw new FormatException($"Invalid Point format: '{str}'.");
            return new Point(ParseInt(parts[0]), ParseInt(parts[1]));
        }

        protected override ConstructorInfo InstanceConstructor => PointConstructor.Value;
        protected override object[] InstanceConstructorArgs(Point o) => new object[] { o.X, o.Y };
    }
}