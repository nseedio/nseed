using System;

namespace NSeed
{
    /// <summary>
    /// Provides a friendly, human-readable name for classes
    /// that implement NSeed abstractions like e.g. <see cref="ISeed"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class FriendlyNameAttribute : Attribute
    {
        /// <summary>
        /// The friendly name.
        /// </summary>
        public string FriendlyName { get; }

        /// <summary>
        /// Creates new <see cref="FriendlyNameAttribute"/> with the specified friendly name.
        /// </summary>
        /// <param name="friendlyName">The friendly name. It must not be null, empty, or whitespace.</param>
        public FriendlyNameAttribute(string friendlyName)
        {
            FriendlyName = friendlyName;
        }
    }
}