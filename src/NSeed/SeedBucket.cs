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
        private ISeedBucketInfoBuilder<Type> seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

        /// <summary>
        /// Returns <see cref="SeedBucketInfo"/> for this seed bucket.
        /// </summary>
        /// <returns></returns>
        public SeedBucketInfo GetMetaInfo()
        {
            return seedBucketInfoBuilder.BuildFrom(GetType());
        }
    }
}