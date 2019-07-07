using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes a single concrete <see cref="ISeed"/> implementation.
    /// </summary>
    public sealed class SeedInfo : SeedableInfo
    {
        /// <summary>
        /// Entities yielded by this seed.
        /// </summary>
        public IReadOnlyCollection<EntityInfo> Entities { get; }

        internal SeedInfo(
            Type type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequires,
            IReadOnlyCollection<SeedableInfo> implicitlyRequires,
            IReadOnlyCollection<EntityInfo> entities)
            :base(type, fullName, friendlyName, description, explicitlyRequires, implicitlyRequires)
        {            
            System.Diagnostics.Debug.Assert(entities != null);
            System.Diagnostics.Debug.Assert(entities.All(entity => entity != null));

            Entities = entities;
        }
    }
}