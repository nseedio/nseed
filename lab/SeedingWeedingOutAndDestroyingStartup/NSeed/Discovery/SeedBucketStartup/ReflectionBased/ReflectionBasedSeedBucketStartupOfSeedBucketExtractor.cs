using System;

namespace NSeed.Discovery.SeedBucketStartup.ReflectionBased
{
    internal class ReflectionBasedSeedBucketStartupOfSeedBucketExtractor : BaseSeedBucketStartupOfSeedBucketExtractor<Type, Type>
    {
        public ReflectionBasedSeedBucketStartupOfSeedBucketExtractor()
            : base(
                  new ReflectionBasedSeedBucketStartupInSeedBucketDiscoverer(),
                  new ReflectionBasedSeedBucketStartupInfoBuilder())
        {
        }
    }
}
