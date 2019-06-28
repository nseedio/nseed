using System;
using NSeed.Guards;

namespace NSeed
{
    /// <summary>
    /// Describes a single <see cref="ISeed"/>.
    /// </summary>
    public class SeedInfo
    {
        /// <summary>
        /// The underlying implementation <see cref="System.Type"/> of the seed, if exists, otherwise null.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// The full name of the seed.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Type"/> exists, this property is equal to its <see cref="Type.FullName"/>.
        /// </remarks>
        public string FullName { get; }

        /// <summary>
        /// The friendly name of the seed.
        /// </summary>
        /// <returns>
        /// If the seed implementation has the <see cref="FriendlyNameAttribute"/> applied
        /// the <see cref="FriendlyNameAttribute.FriendlyName"/> is returned.
        /// <br/>
        /// Otherwise, the humanized version of the implementation type name is returned.
        /// </returns>
        public string FriendlyName { get; }

        /// <summary>
        /// The description of the seed.
        /// </summary>
        /// <returns>
        /// If the seed implementation has the <see cref="DescriptionAttribute"/> applied
        /// the <see cref="DescriptionAttribute.Description"/> is returned.
        /// <br/>
        /// Otherwise, the <see cref="string.Empty"/> is returned.
        /// </returns>
        public string Description { get; }

        internal SeedInfo(Type type, string fullName, string friendlyName, string description)
        {
            System.Diagnostics.Debug.Assert(!fullName.IsNullOrWhiteSpace());
            System.Diagnostics.Debug.Assert(!friendlyName.IsNullOrWhiteSpace());
            System.Diagnostics.Debug.Assert(description != null);

            Type = type;
            FullName = fullName;
            FriendlyName = friendlyName;
            Description = description;
        }
    }
}