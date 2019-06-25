using System;
using Light.GuardClauses;

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
        /// If the <see cref="Type"/> exists, this property is equal to <see cref="SeedInfo.Type.FullName"/>.
        /// </remarks>
        public string FullName { get; }

        /// <summary>
        /// The friendly name of the seed.
        /// </summary>
        /// <remarks>
        /// If the seed implementation has the <see cref="FriendlyNameAttribute"/> applied
        /// the <see cref="FriendlyNameAttribute.FriendlyName"/> is returned.
        /// <br/>
        /// Otherwise, the humanized version of the implementation type name is returned.
        /// </remarks>
        public string FriendlyName { get; }

        internal SeedInfo(Type type, string fullName, string friendlyName)
        {
            System.Diagnostics.Debug.Assert(!fullName.IsNullOrWhiteSpace());
            System.Diagnostics.Debug.Assert(!friendlyName.IsNullOrWhiteSpace());

            Type = type;
            FullName = fullName;
            FriendlyName = friendlyName;
        }
    }
}