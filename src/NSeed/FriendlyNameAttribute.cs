using System;
using NSeed.Guards;

namespace NSeed
{
    /// <summary>
    /// Provides a friendly, human-readable name for classes
    /// that implement NSeed abstractions like e.g. <see cref="ISeed"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class FriendlyNameAttribute : Attribute
    {
        public string FriendlyName { get; }

        public FriendlyNameAttribute(string friendlyName)
        {
            FriendlyName = friendlyName.MustNotBeNull(nameof(friendlyName));
        }
    }
}