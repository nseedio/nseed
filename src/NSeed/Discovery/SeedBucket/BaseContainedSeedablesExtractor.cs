using System.Linq;
using NSeed.MetaInfo;
using System.Collections.Generic;
using NSeed.Discovery.Seedable;

namespace NSeed.Discovery.SeedBucket
{
    internal abstract class BaseContainedSeedablesExtractor<TSeedBucketImplementation, TSeedableImplementation> : IContainedSeedablesExtractor<TSeedBucketImplementation>
        where TSeedBucketImplementation : class
        where TSeedableImplementation : class
    {
        private readonly IContainedSeedablesDiscoverer<TSeedBucketImplementation, TSeedableImplementation> seedablesDiscoverer;
        private readonly ISeedableInfoBuilder<TSeedableImplementation> seedableBuilder;

        protected internal BaseContainedSeedablesExtractor(IContainedSeedablesDiscoverer<TSeedBucketImplementation, TSeedableImplementation> seedablesDiscoverer,
                                                           ISeedableInfoBuilder<TSeedableImplementation> seedableBuilder)
        {
            System.Diagnostics.Debug.Assert(seedablesDiscoverer != null);
            System.Diagnostics.Debug.Assert(seedableBuilder != null);

            this.seedablesDiscoverer = seedablesDiscoverer;
            this.seedableBuilder = seedableBuilder;
        }

        IReadOnlyCollection<SeedableInfo> IExtractor<TSeedBucketImplementation, IReadOnlyCollection<SeedableInfo>>.ExtractFrom(TSeedBucketImplementation seedBucketImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedBucketImplementation != null);
            System.Diagnostics.Debug.Assert(errorCollector != null);

            return seedablesDiscoverer.DiscoverIn(seedBucketImplementation)
                                .DiscoveredItems
                                .Select(seedableType => seedableBuilder.BuildFrom(seedableType))
                                .ToArray();
        }
    }
}