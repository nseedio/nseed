using NSeed.Extensions;
using NSeed.MetaInfo;
using System;
using System.Linq;

namespace NSeed.Discovery.SeedBucket.ReflectionBased
{
    internal class ReflectionBasedSeedBucketOfSeedableExtractor : ISeedBucketOfSeedableExtractor<Type>
    {
        private readonly ISeedBucketOfSeedableDiscoverer<Type, Type> seedBucketOfSeedableDiscoverer;
        private readonly ISeedBucketInfoBuilder<Type> seedBucketInfoBuilder;

        public ReflectionBasedSeedBucketOfSeedableExtractor(ISeedBucketInfoBuilder<Type> seedBucketInfoBuilder)
        {
            System.Diagnostics.Debug.Assert(seedBucketInfoBuilder != null);

            seedBucketOfSeedableDiscoverer = new ReflectionBasedSeedBucketOfSeedableDiscoverer();
            this.seedBucketInfoBuilder = seedBucketInfoBuilder;
        }

        SeedBucketInfo? IExtractor<Type, SeedBucketInfo?>.ExtractFrom(Type seedableImplementation, IErrorCollector errorCollector)
        {
            System.Diagnostics.Debug.Assert(seedableImplementation.IsSeedableType());
            System.Diagnostics.Debug.Assert(errorCollector != null);

            var discoverdSeedBucketTypes = seedBucketOfSeedableDiscoverer.DiscoverIn(seedableImplementation).DiscoveredItems;

            return discoverdSeedBucketTypes.Count == 1
                ? seedBucketInfoBuilder.BuildFrom(discoverdSeedBucketTypes.First())
                : null;
        }
    }
}
