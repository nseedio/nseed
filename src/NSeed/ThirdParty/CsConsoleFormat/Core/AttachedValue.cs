// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

using System.ComponentModel;

namespace Alba.CsConsoleFormat
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal struct AttachedValue<T>
    {
        public AttachedProperty<T> Property { get; }
        public T Value { get; }

        public AttachedValue(AttachedProperty<T> property, T value)
        {
            Property = property;
            Value = value;
        }
    }
}