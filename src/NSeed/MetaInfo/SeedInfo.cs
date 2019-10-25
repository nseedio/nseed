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
        /// Gets types of entities yielded by this seed.
        /// </summary>
        public IReadOnlyCollection<EntityInfo> YieldedEntities { get; }

        /// <summary>
        /// Gets the yield seeded by this seed or null if this seed does not
        /// provide access to its yield.
        /// </summary>
        public ProvidedYieldInfo? Yield { get; }

        /// <summary>
        /// Gets yields of other seeds required by this seed.
        /// </summary>
        public IReadOnlyCollection<RequiredYieldInfo> RequiredYields { get; }

        /// <summary>
        /// Gets all seedables required by this seed.
        /// <br/>
        /// Returns union of <see cref="SeedableInfo.ExplicitlyRequiredSeedables"/> and
        /// seeds whose yields are used in <see cref="RequiredYields"/>.
        /// </summary>
        public override IEnumerable<SeedableInfo> RequiredSeedables =>
            ExplicitlyRequiredSeedables
            .Union(RequiredYields.Select(requiredYield => requiredYield.YieldingSeed));

        internal SeedInfo(
            object implementation,
            Type? type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequiredSeedables,
            IReadOnlyCollection<EntityInfo> yieldedEntities,
            ProvidedYieldInfo? yield,
            IReadOnlyCollection<RequiredYieldInfo> requiredYields,
            IReadOnlyCollection<Error> directErrors)
            : base(implementation, type, fullName, friendlyName, description, explicitlyRequiredSeedables, directErrors)
        {
            System.Diagnostics.Debug.Assert(type == null || type.IsSeedType());
            System.Diagnostics.Debug.Assert(yieldedEntities.All(entity => entity != null));
            System.Diagnostics.Debug.Assert(yield == null || yield.Type == null || type == null || (yield.Type.IsYieldTypeOfSeed(type) && yield.YieldingSeed == null));
            System.Diagnostics.Debug.Assert(requiredYields.All(requiredYield =>
                requiredYield != null &&
                requiredYield.Type != null &&
                requiredYield.YieldingSeed.Type != null &&
                requiredYield.Type.IsYieldTypeOfSeed(requiredYield.YieldingSeed.Type) &&
                requiredYield.RequiringSeed == null));

            YieldedEntities = yieldedEntities;

            if (yield != null && yield.Type != null) yield.YieldingSeed = this;
            Yield = yield;

            RequiredYields = requiredYields;
            foreach (var requiredYield in requiredYields) requiredYield.RequiringSeed = this;
        }

        /// <inheritdoc />
        protected override IEnumerable<MetaInfo> GetDirectChildMetaInfos()
        {
            foreach (var yieldedEntity in YieldedEntities)
                yield return yieldedEntity;

            if (Yield != null) yield return Yield;
        }
    }
}
