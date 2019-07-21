using System;

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
        /// Gets the description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionAttribute"/> class with the specified <paramref name="description"/>.
        /// </summary>
        /// <param name="description">The description. It must not be null, empty, or white space.</param>
        public DescriptionAttribute(string description) => Description = description;
    }
}
