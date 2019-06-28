using System;
using NSeed.Guards;

namespace NSeed
{
    /// <summary>
    /// Provides a description for classes
    /// that implement NSeed abstractions like e.g. <see cref="ISeed"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// The description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Creates new <see cref="DescriptionAttribute"/> with the specified description.
        /// </summary>
        /// <param name="description">The description. It must not be null, empty, or white space.</param>
        public DescriptionAttribute(string description)
        {
            Description = description.MustNotBeNullOrWhiteSpace(nameof(description));
        }
    }
}