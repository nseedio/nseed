using NSeed.Discovery.Seedable;
using NSeed.MetaInfo;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Discovery.SeedBucket
{
    internal abstract class BaseContainedSeedablesExtractor<TSeedBucketImplementation, TSeedableImplementation> : IContainedSeedablesExtractor<TSeedBucketImplementation>
        where TSeedBucketImplementation : class
        where TSeedableImplementation : class
    {
        private readonly IContainedSeedablesDiscoverer<TSeedBucketImplementation, TSeedableImplementation> seedablesDiscoverer;
        private readonly ISeedableInfoBuilder<TSeedableImplementation> seedableBuilder;

        protected internal BaseContainedSeedablesExtractor(
            IContainedSeedablesDiscoverer<TSeedBucketImplementation, TSeedableImplementation> seedablesDiscoverer,
            ISeedableInfoBuilder<TSeedableImplementation> seedableBuilder)
        {
            this.seedablesDiscoverer = seedablesDiscoverer;
            this.seedableBuilder = seedableBuilder;
        }

        IReadOnlyCollection<SeedableInfo> IExtractor<TSeedBucketImplementation, IReadOnlyCollection<SeedableInfo>>.ExtractFrom(TSeedBucketImplementation seedBucketImplementation)
        {
            // CS8619:
            // We filter out nulls in "Where(seedable => seedable != null)", but
            // the compiler is not able to figure that out.
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return seedablesDiscoverer.DiscoverIn(seedBucketImplementation)
                                .DiscoveredItems
                                .Select(seedableType => seedableBuilder.BuildFrom(seedableType))
                                .Where(seedable => seedable != null)
                                .ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    }
}
