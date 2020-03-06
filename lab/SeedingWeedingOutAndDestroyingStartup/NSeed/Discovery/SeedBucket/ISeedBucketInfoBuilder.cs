using NSeed.MetaInfo;

namespace NSeed.Discovery.SeedBucket
{
    internal interface ISeedBucketInfoBuilder<TSeedBucketImplementation> : IMetaInfoBuilder<TSeedBucketImplementation, SeedBucketInfo>
        where TSeedBucketImplementation : class
    {
    }
}
