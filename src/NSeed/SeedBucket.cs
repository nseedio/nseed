using NSeed.Discovery.SeedBucket;
using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.MetaInfo;
using System;

namespace NSeed
{
    /// <summary>
    /// A bucket of <see cref="ISeed"/>s.
    /// </summary>
    public abstract class SeedBucket
    {
        private readonly ISeedBucketInfoBuilder<Type> seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

        /// <summary>
        /// Gets <see cref="SeedBucketInfo"/> for this seed bucket.
        /// </summary>
        /// <returns><see cref="SeedBucketInfo"/> that describes this seed bucket.</returns>
        public SeedBucketInfo GetMetaInfo()
        {
            return seedBucketInfoBuilder.BuildFrom(GetType());
        }
    }
}
