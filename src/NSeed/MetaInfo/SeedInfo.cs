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
        /// Types of entities yielded by this seed.
        /// </summary>
        public IReadOnlyCollection<EntityInfo> Entities { get; }

        /// <summary>
        /// The yield seeded by this seed or null if this seed does not
        /// provide access to its yield.
        /// </summary>
        public ProvidedYieldInfo Yield { get; }

        /// <summary>
        /// Yields of other seeds used by this seed.
        /// </summary>
        public object UsesYields { get; }

        /// <summary>
        /// All seedables required by this seed.
        /// <br/>
        /// Returns union of all <see cref="SeedableInfo.ExplicitlyRequires"/> seedables and
        /// seeds whose yields are used in <see cref="UsesYields"/>.
        /// </summary>
        public override IEnumerable<SeedableInfo> Requires => ExplicitlyRequires; // TODO: Add UsesYields.

        internal SeedInfo(
            Type type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequires,
            IReadOnlyCollection<EntityInfo> entities,
            ProvidedYieldInfo yield)
            :base(type, fullName, friendlyName, description, explicitlyRequires)
        {
            System.Diagnostics.Debug.Assert(type == null || type.IsSeedType());
            System.Diagnostics.Debug.Assert(entities != null);
            System.Diagnostics.Debug.Assert(entities.All(entity => entity != null));
            System.Diagnostics.Debug.Assert(yield == null || yield.Type == null || type == null || yield.Type.IsYieldTypeOfSeed(type));

            Entities = entities;

            if (yield != null && yield.Type != null) yield.YieldingSeed = this;
            Yield = yield;
        }
    }
}