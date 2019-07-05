using System;
using System.Collections.Generic;
using NSeed.Guards;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes a single concrete <see cref="ISeed"/> implementation.
    /// </summary>
    public sealed class SeedInfo : BaseMetaInfo
    {
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

        /// <summary>
        /// Entities created by this seed.
        /// </summary>
        public IReadOnlyCollection<EntityInfo> Entities { get; }

        internal SeedInfo(
            Type type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<EntityInfo> entities)
            :base(type, fullName)
        {            
            System.Diagnostics.Debug.Assert(!friendlyName.IsNullOrWhiteSpace());
            System.Diagnostics.Debug.Assert(description != null);
            System.Diagnostics.Debug.Assert(entities != null);

            FriendlyName = friendlyName;
            Description = description;
            Entities = entities;
        }
    }
}