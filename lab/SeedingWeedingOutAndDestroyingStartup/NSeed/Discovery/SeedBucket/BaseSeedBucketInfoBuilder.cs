using NSeed.Discovery.SeedBucketStartup;
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
        private readonly ISeedBucketStartupOfSeedBucketExtractor<TSeedBucketImplementation> seedBucketStartupExtractor;

        internal BaseSeedBucketInfoBuilder(
            ITypeExtractor<TSeedBucketImplementation> typeExtractor,
            IFullNameExtractor<TSeedBucketImplementation> fullNameExtractor,
            IFriendlyNameExtractor<TSeedBucketImplementation> friendlyNameExtractor,
            IDescriptionExtractor<TSeedBucketImplementation> descriptionExtractor,
            IContainedSeedablesExtractor<TSeedBucketImplementation> seedablesExtractor,
            Func<ISeedBucketInfoBuilder<TSeedBucketImplementation>, ISeedBucketOfSeedableExtractor<TSeedableImplementation>> seedBucketOfSeedableExtractorFactory,
            IMetaInfoPool<TSeedBucketImplementation, SeedBucketInfo> seedBucketInfoPool,
            ISeedBucketStartupOfSeedBucketExtractor<TSeedBucketImplementation> seedBucketStartupExtractor)
        {
            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
            this.seedablesExtractor = seedablesExtractor;
            seedBucketOfSeedableExtractor = seedBucketOfSeedableExtractorFactory(this);
            this.seedBucketInfoPool = seedBucketInfoPool;
            this.seedBucketStartupExtractor = seedBucketStartupExtractor;
        }

        SeedBucketInfo IMetaInfoBuilder<TSeedBucketImplementation, SeedBucketInfo>.BuildFrom(TSeedBucketImplementation implementation)
        {
            return seedBucketInfoPool.GetOrAdd(implementation, CreateSeedBucketInfo);
        }

        private SeedBucketInfo CreateSeedBucketInfo(TSeedBucketImplementation implementation)
        {
            Type? type = typeExtractor.ExtractFrom(implementation);
            string fullName = fullNameExtractor.ExtractFrom(implementation);
            string friendlyName = friendlyNameExtractor.ExtractFrom(implementation);
            string description = descriptionExtractor.ExtractFrom(implementation);
            var containedSeedables = seedablesExtractor.ExtractFrom(implementation);
            var startups = seedBucketStartupExtractor.ExtractFrom(implementation);

            SetSeedBucketInfosForNonContainedSeedables();

            return new SeedBucketInfo
            (
                implementation,
                type,
                fullName,
                friendlyName,
                description,
                containedSeedables,
                startups,
                Array.Empty<Error>()
            );

            void SetSeedBucketInfosForNonContainedSeedables()
            {
                foreach (var nonContainedSeedable in SeedableInfo.GetRequiredSeedableInfosNotContainedIn(containedSeedables))
                    nonContainedSeedable.SeedBucket = seedBucketOfSeedableExtractor.ExtractFrom((TSeedableImplementation)nonContainedSeedable.Implementation);
            }
        }
    }
}
