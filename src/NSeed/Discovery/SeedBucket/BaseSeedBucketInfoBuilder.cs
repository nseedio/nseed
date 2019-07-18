using System;
using NSeed.MetaInfo;

namespace NSeed.Discovery.SeedBucket
{
    internal abstract class BaseSeedBucketInfoBuilder<TSeedBucketImplementation> : ISeedBucketInfoBuilder<TSeedBucketImplementation>
        where TSeedBucketImplementation : class
    {
        private readonly ITypeExtractor<TSeedBucketImplementation> typeExtractor;
        private readonly IFullNameExtractor<TSeedBucketImplementation> fullNameExtractor;
        private readonly IFriendlyNameExtractor<TSeedBucketImplementation> friendlyNameExtractor;
        private readonly IDescriptionExtractor<TSeedBucketImplementation> descriptionExtractor;
        private readonly IContainedSeedablesExtractor<TSeedBucketImplementation> seedablesExtractor;
        private readonly IMetaInfoPool<TSeedBucketImplementation, SeedBucketInfo> seedBucketInfoPool;

        internal BaseSeedBucketInfoBuilder(ITypeExtractor<TSeedBucketImplementation> typeExtractor,
                                     IFullNameExtractor<TSeedBucketImplementation> fullNameExtractor,
                                     IFriendlyNameExtractor<TSeedBucketImplementation> friendlyNameExtractor,
                                     IDescriptionExtractor<TSeedBucketImplementation> descriptionExtractor,
                                     IContainedSeedablesExtractor<TSeedBucketImplementation> seedablesExtractor,
                                     IMetaInfoPool<TSeedBucketImplementation, SeedBucketInfo> seedBucketInfoPool)
        {
            System.Diagnostics.Debug.Assert(typeExtractor != null);
            System.Diagnostics.Debug.Assert(fullNameExtractor != null);
            System.Diagnostics.Debug.Assert(friendlyNameExtractor != null);
            System.Diagnostics.Debug.Assert(descriptionExtractor != null);
            System.Diagnostics.Debug.Assert(seedablesExtractor != null);
            System.Diagnostics.Debug.Assert(seedBucketInfoPool != null);

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
            this.seedablesExtractor = seedablesExtractor;
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

            return new SeedBucketInfo
            (
                type,
                fullName,
                friendlyName,
                description,
                containedSeedables
            );
        }
    }
}