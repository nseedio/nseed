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
        public IReadOnlyCollection<EntityInfo> YieldedEntities { get; }

        /// <summary>
        /// The yield seeded by this seed or null if this seed does not
        /// provide access to its yield.
        /// </summary>
        public ProvidedYieldInfo Yield { get; }

        /// <summary>
        /// Yields of other seeds required by this seed.
        /// </summary>
        public IReadOnlyCollection<RequiredYieldInfo> RequiredYields { get; } = Array.Empty<RequiredYieldInfo>();

        /// <summary>
        /// All seedables required by this seed.
        /// <br/>
        /// Returns union of <see cref="SeedableInfo.ExplicitlyRequiredSeedables"/> and
        /// seeds whose yields are used in <see cref="RequiredYields"/>.
        /// </summary>
        public override IEnumerable<SeedableInfo> RequiredSeedables =>
            ExplicitlyRequiredSeedables
            .Union(RequiredYields.Select(requiredYield => requiredYield.YieldingSeed));

        internal SeedInfo(
            Type type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequiredSeedables,
            IReadOnlyCollection<EntityInfo> yieldedEntities,
            ProvidedYieldInfo yield)
            :base(type, fullName, friendlyName, description, explicitlyRequiredSeedables)
        {
            System.Diagnostics.Debug.Assert(type == null || type.IsSeedType());
            System.Diagnostics.Debug.Assert(yieldedEntities != null);
            System.Diagnostics.Debug.Assert(yieldedEntities.All(entity => entity != null));
            System.Diagnostics.Debug.Assert(yield == null || yield.Type == null || type == null || yield.Type.IsYieldTypeOfSeed(type));

            YieldedEntities = yieldedEntities;

            if (yield != null && yield.Type != null) yield.YieldingSeed = this;
            Yield = yield;
        }
    }
}