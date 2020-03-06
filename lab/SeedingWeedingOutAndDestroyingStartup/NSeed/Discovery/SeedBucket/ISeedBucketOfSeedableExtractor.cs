using NSeed.MetaInfo;

namespace NSeed.Discovery.SeedBucket
{
    internal interface ISeedBucketOfSeedableExtractor<TSeedableImplementation> : IExtractor<TSeedableImplementation, SeedBucketInfo?>
        where TSeedableImplementation : class
    {
    }
}
