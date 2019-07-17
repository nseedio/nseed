using NSeed.Discovery.Common.ReflectionBased;
using System;

namespace NSeed.Discovery.SeedBucket.ReflectionBased
{
    internal class ReflectionBasedSeedBucketInfoBuilder : BaseSeedBucketInfoBuilder<Type>
    {
        public ReflectionBasedSeedBucketInfoBuilder()
            : base(new ReflectionBasedTypeExtractor(),
                   new ReflectionBasedFullNameExtractor(),
                   new ReflectionBasedFriendlyNameExtractor(),
                   new ReflectionBasedDescriptionExtractor(),
                   new ReflectionBasedSeedBucketInfoPool())
        {
        }
    }
}