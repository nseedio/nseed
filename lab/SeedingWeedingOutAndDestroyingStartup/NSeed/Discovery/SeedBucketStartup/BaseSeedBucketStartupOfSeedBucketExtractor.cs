using NSeed.MetaInfo;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Discovery.SeedBucketStartup
{
    internal abstract class BaseSeedBucketStartupOfSeedBucketExtractor<TSeedBucketImplementation, TSeedBucketStartupImplementation> : ISeedBucketStartupOfSeedBucketExtractor<TSeedBucketImplementation>
        where TSeedBucketImplementation : class
        where TSeedBucketStartupImplementation : class
    {
        private readonly ISeedBucketStartupInSeedBucketDiscoverer<TSeedBucketImplementation, TSeedBucketStartupImplementation> seedBucketStartupDiscoverer;
        private readonly ISeedBucketStartupInfoBuilder<TSeedBucketStartupImplementation> seedBucketStartupBuilder;

        protected internal BaseSeedBucketStartupOfSeedBucketExtractor(
            ISeedBucketStartupInSeedBucketDiscoverer<TSeedBucketImplementation, TSeedBucketStartupImplementation> seedBucketStartupDiscoverer,
            ISeedBucketStartupInfoBuilder<TSeedBucketStartupImplementation> seedBucketStartupBuilder)
        {
            this.seedBucketStartupDiscoverer = seedBucketStartupDiscoverer;
            this.seedBucketStartupBuilder = seedBucketStartupBuilder;
        }

        IReadOnlyCollection<SeedBucketStartupInfo> IExtractor<TSeedBucketImplementation, IReadOnlyCollection<SeedBucketStartupInfo>>.ExtractFrom(TSeedBucketImplementation seedBucketImplementation)
        {
            // CS8619:
            // We filter out nulls in "Where(seedable => seedable != null)", but
            // the compiler is not able to figure that out.
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return seedBucketStartupDiscoverer.DiscoverIn(seedBucketImplementation)
                                .DiscoveredItems
                                .Select(seedBucketStartupType => seedBucketStartupBuilder.BuildFrom(seedBucketStartupType))
                                .Where(seedBucketStartup => seedBucketStartup != null)
                                .ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    }
}
