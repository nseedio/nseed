using NSeed.Discovery.Common.ReflectionBased;
using System;

namespace NSeed.Discovery.SeedBucketStartup.ReflectionBased
{
    internal class ReflectionBasedSeedBucketStartupInfoBuilder : BaseSeedBucketStartupInfoBuilder<Type>
    {
        public ReflectionBasedSeedBucketStartupInfoBuilder()
            : base(
                  new ReflectionBasedTypeExtractor(),
                  new ReflectionBasedFullNameExtractor(),
                  new ReflectionBasedFriendlyNameExtractor(),
                  new ReflectionBasedDescriptionExtractor())
        {
        }
    }
}
