using NSeed.Extensions;
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

        /// <summary>
        /// The yield seeded by this seed or null if this seed does not
        /// provide access to its yield.
        /// </summary>
        public YieldInfo Yield { get; }

        internal SeedInfo(
            Type type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequires,
            IReadOnlyCollection<SeedInfo> implicitlyRequires,
            IReadOnlyCollection<EntityInfo> entities,
            YieldInfo yield)
            :base(type, fullName, friendlyName, description, explicitlyRequires, implicitlyRequires)
        {
            System.Diagnostics.Debug.Assert(type == null || type.IsSeedType());
            System.Diagnostics.Debug.Assert(entities != null);
            System.Diagnostics.Debug.Assert(entities.All(entity => entity != null));
            System.Diagnostics.Debug.Assert(yield == null || yield.Type == null || type == null || yield.Type.IsYieldTypeOfSeed(type));

            Entities = entities;

            if (yield != null && yield.Type != null) yield.Seed = this;
            Yield = yield;
        }
    }
}