using NSeed.MetaInfo;
using System;

namespace NSeed.Discovery.SeedBucket
{
    internal abstract class BaseSeedBucketInfoBuilder<TSeedBucketImplementation, TSeedableImplementation> : ISeedBucketInfoBuilder<TSeedBucketImplementation>
        where TSeedBucketImplementation : class
        where TSeedableImplementation : class
    {
        private readonly ITypeExtractor<TSeedBucketImplementation> typeExtractor;
        private readonly IFullNameExtractor<TSeedBucketImplementation> fullNameExtractor;
        private readonly IFriendlyNameExtractor<TSeedBucketImplementation> friendlyNameExtractor;
        private readonly IDescriptionExtractor<TSeedBucketImplementation> descriptionExtractor;
        private readonly IContainedSeedablesExtractor<TSeedBucketImplementation> seedablesExtractor;
        private readonly ISeedBucketOfSeedableExtractor<TSeedableImplementation> seedBucketOfSeedableExtractor;
        private readonly IMetaInfoPool<TSeedBucketImplementation, SeedBucketInfo> seedBucketInfoPool;

        internal BaseSeedBucketInfoBuilder(
            ITypeExtractor<TSeedBucketImplementation> typeExtractor,
            IFullNameExtractor<TSeedBucketImplementation> fullNameExtractor,
            IFriendlyNameExtractor<TSeedBucketImplementation> friendlyNameExtractor,
            IDescriptionExtractor<TSeedBucketImplementation> descriptionExtractor,
            IContainedSeedablesExtractor<TSeedBucketImplementation> seedablesExtractor,
            Func<ISeedBucketInfoBuilder<TSeedBucketImplementation>, ISeedBucketOfSeedableExtractor<TSeedableImplementation>> seedBucketOfSeedableExtractorFactory,
            IMetaInfoPool<TSeedBucketImplementation, SeedBucketInfo> seedBucketInfoPool)
        {
            System.Diagnostics.Debug.Assert(typeExtractor != null);
            System.Diagnostics.Debug.Assert(fullNameExtractor != null);
            System.Diagnostics.Debug.Assert(friendlyNameExtractor != null);
            System.Diagnostics.Debug.Assert(descriptionExtractor != null);
            System.Diagnostics.Debug.Assert(seedablesExtractor != null);
            System.Diagnostics.Debug.Assert(seedBucketOfSeedableExtractorFactory != null);
            System.Diagnostics.Debug.Assert(seedBucketInfoPool != null);

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
            this.seedablesExtractor = seedablesExtractor;
            seedBucketOfSeedableExtractor = seedBucketOfSeedableExtractorFactory(this);
            this.seedBucketInfoPool = seedBucketInfoPool;
        }

        SeedBucketInfo IMetaInfoBuilder<TSeedBucketImplementation, SeedBucketInfo>.BuildFrom(TSeedBucketImplementation implementation)
        {
            System.Diagnostics.Debug.Assert(implementation != null);

            return seedBucketInfoPool.GetOrAdd(implementation, CreateSeedBucketInfo);
        }

        private SeedBucketInfo CreateSeedBucketInfo(TSeedBucketImplementation implementation)
        {
            var errorCollector = new DistinctErrorCollectorAndProvider();

            Type type = typeExtractor.ExtractFrom(implementation, errorCollector);
            string fullName = fullNameExtractor.ExtractFrom(implementation, errorCollector);
            string friendlyName = friendlyNameExtractor.ExtractFrom(implementation, errorCollector);
            string description = descriptionExtractor.ExtractFrom(implementation, errorCollector);
            var containedSeedables = seedablesExtractor.ExtractFrom(implementation, errorCollector);

            SetSeedBucketInfosForNonContainedSeedables();

            return new SeedBucketInfo
            (
                type,
                type,
                fullName,
                friendlyName,
                description,
                containedSeedables
            );

            void SetSeedBucketInfosForNonContainedSeedables()
            {
                foreach (var nonContainedSeedable in SeedableInfo.GetRequiredSeedableInfosNotContainedIn(containedSeedables))
                    nonContainedSeedable.SeedBucket = seedBucketOfSeedableExtractor.ExtractFrom((TSeedableImplementation)nonContainedSeedable.Implementation, errorCollector);
            }
        }
    }
}
